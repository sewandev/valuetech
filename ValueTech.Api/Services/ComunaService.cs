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
            // Validaciones de Dominio (Regla 3.3)
            if (request.Superficie < 0) throw new ArgumentException("La superficie no puede ser negativa.");
            if (request.Poblacion < 0) throw new ArgumentException("La población no puede ser negativa.");
            if (request.Densidad < 0) throw new ArgumentException("La densidad no puede ser negativa.");

            // Construccion Estricta de XML (Regla 3.2 Escritura)
            // CultureInfo.InvariantCulture asegurado
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
                    
                    // Parsing defensivo (Regla 3.2 Lectura)
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
                    // Regla 3.2: Registrar error pero NO lanzar excepción
                    _logger.LogError(ex, "Error parseando XML para Comuna {Id}. XML: {Xml}", comuna.IdComuna, comuna.InformacionAdicional);
                    // Se devuelven valores por defecto (0)
                }
            }

            return response;
        }
    }
}
