using System.Globalization;
using System.Xml.Linq;
using ValueTech.Data.Interfaces;
using ValueTech.Data.Models;
using ValueTech.Api.Contracts.Requests;
using ValueTech.Api.Contracts.Responses;

namespace ValueTech.Api.Services
{
    public class ComunaService : IComunaService
    {
        private readonly IComunaRepository _repository;

        public ComunaService(IComunaRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<ComunaResponse>> GetByRegionIdAsync(int regionId)
        {
            var comunas = await _repository.GetByRegionIdAsync(regionId);
            return comunas.Select(MapToResponse);
        }

        public async Task<ComunaResponse?> GetByIdAsync(int idComuna)
        {
            var comuna = await _repository.GetByIdAsync(idComuna);
            return comuna == null ? null : MapToResponse(comuna);
        }

        public async Task UpdateAsync(int idComuna, CreateComunaRequest request)
        {
            // Construir XML a mano o con XElement
            // <Info><Superficie>...</Superficie><Poblacion Densidad="...">...</Poblacion></Info>
            var xml = new XElement("Info",
                new XElement("Superficie", request.Superficie.ToString(CultureInfo.InvariantCulture)),
                new XElement("Poblacion", 
                    new XAttribute("Densidad", request.Densidad.ToString(CultureInfo.InvariantCulture)),
                    request.Poblacion
                )
            );

            var entity = new Comuna
            {
                IdComuna = idComuna,
                IdRegion = request.IdRegion,
                Nombre = request.Nombre,
                InformacionAdicional = xml.ToString(SaveOptions.DisableFormatting) 
            };

            await _repository.UpdateAsync(entity);
        }

        private ComunaResponse MapToResponse(Comuna comuna)
        {
            var response = new ComunaResponse
            {
                IdComuna = comuna.IdComuna,
                IdRegion = comuna.IdRegion,
                Nombre = comuna.Nombre
            };

            if (!string.IsNullOrEmpty(comuna.InformacionAdicional))
            {
                try 
                {
                    var xml = XElement.Parse(comuna.InformacionAdicional);
                    
                    // Parsing defensivo
                    if (decimal.TryParse(xml.Element("Superficie")?.Value, NumberStyles.Any, CultureInfo.InvariantCulture, out var sup))
                    {
                        response.Superficie = sup;
                    }

                    var pobNode = xml.Element("Poblacion");
                    if (pobNode != null)
                    {
                        if (int.TryParse(pobNode.Value, out var pob))
                        {
                            response.Poblacion = pob;
                        }
                        if (decimal.TryParse(pobNode.Attribute("Densidad")?.Value, NumberStyles.Any, CultureInfo.InvariantCulture, out var den))
                        {
                            response.Densidad = den;
                        }
                    }
                }
                catch
                {
                    // Si el XML está corrupto, devolvemos valores por defecto (0)
                    // Podríamos loggear el error aquí
                }
            }

            return response;
        }
    }
}
