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
        private readonly ILogger<ComunaService> _logger;

        public ComunaService(IComunaRepository repository, ILogger<ComunaService> logger)
        {
            _repository = repository;
            _logger = logger;
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

        public async Task UpdateAsync(int idComuna, UpdateComunaRequest request)
        {
            if (request.Superficie.HasValue && request.Superficie.Value < 0) throw new ArgumentException("La superficie no puede ser negativa.");
            if (request.Poblacion.HasValue && request.Poblacion.Value < 0) throw new ArgumentException("La población no puede ser negativa.");
            if (request.Densidad.HasValue && request.Densidad.Value < 0) throw new ArgumentException("La densidad no puede ser negativa.");

            var xml = new XElement("Info",
                new XElement("Superficie", request.Superficie.GetValueOrDefault().ToString(CultureInfo.InvariantCulture)),
                new XElement("Poblacion", 
                    new XAttribute("Densidad", request.Densidad.GetValueOrDefault().ToString(CultureInfo.InvariantCulture)),
                    request.Poblacion.GetValueOrDefault()
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
            _logger.LogInformation("Comuna {Id} actualizada exitosamente.", idComuna);
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
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error parseando XML para Comuna {Id}. XML: {Xml}", comuna.IdComuna, comuna.InformacionAdicional);
                }
            }

            return response;
        }
    }
}
