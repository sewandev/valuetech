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
        private readonly IAuditoriaRepository _auditRepository;
        private readonly ILogger<ComunaService> _logger;

        public ComunaService(IComunaRepository repository, IAuditoriaRepository auditRepository, ILogger<ComunaService> logger)
        {
            _repository = repository;
            _auditRepository = auditRepository;
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

        public async Task UpdateAsync(int idComuna, UpdateComunaRequest request, string auditUser, string auditIp)
        {
            ValidateComunaMetrics(request.Superficie, request.Poblacion, request.Densidad);
            var xml = BuildComunaXml(request.Superficie, request.Poblacion, request.Densidad);

            var entity = new Comuna
            {
                IdComuna = idComuna,
                IdRegion = request.IdRegion,
                Nombre = request.Nombre,
                InformacionAdicional = xml.ToString(SaveOptions.DisableFormatting) 
            };

            await _repository.UpdateAsync(entity);
            await LogAuditAsync(auditUser, auditIp, "UPDATE_COMUNA", $"ComunaId: {idComuna}, RegionId: {request.IdRegion}, Nombre: {request.Nombre}");
            _logger.LogInformation("Comuna {Id} actualizada exitosamente por {User}.", idComuna, auditUser);
        }

        public async Task<int> CreateAsync(CreateComunaRequest request, string auditUser, string auditIp)
        {
            ValidateComunaMetrics(request.Superficie, request.Poblacion, request.Densidad);
            var xml = BuildComunaXml(request.Superficie, request.Poblacion, request.Densidad);

            var entity = new Comuna
            {
                IdRegion = request.IdRegion,
                Nombre = request.Nombre,
                InformacionAdicional = xml.ToString(SaveOptions.DisableFormatting)
            };

            var newId = await _repository.CreateAsync(entity);
            await LogAuditAsync(auditUser, auditIp, "CREATE_COMUNA", $"NewId: {newId}, RegionId: {request.IdRegion}, Nombre: {request.Nombre}");
            _logger.LogInformation("Comuna {Id} creada exitosamente por {User}.", newId, auditUser);
            return newId;
        }

        private void ValidateComunaMetrics(decimal? superficie, int? poblacion, decimal? densidad)
        {
            if (superficie.HasValue && superficie.Value < 0) throw new ArgumentException("La superficie no puede ser negativa.");
            if (poblacion.HasValue && poblacion.Value < 0) throw new ArgumentException("La población no puede ser negativa.");
            if (densidad.HasValue && densidad.Value < 0) throw new ArgumentException("La densidad no puede ser negativa.");
        }

        private XElement BuildComunaXml(decimal? superficie, int? poblacion, decimal? densidad)
        {
            return new XElement("Info",
                new XElement("Superficie", superficie.GetValueOrDefault().ToString(CultureInfo.InvariantCulture)),
                new XElement("Poblacion", 
                    new XAttribute("Densidad", densidad.GetValueOrDefault().ToString(CultureInfo.InvariantCulture)),
                    poblacion.GetValueOrDefault()
                )
            );
        }
        
        private async Task LogAuditAsync(string user, string ip, string action, string detail)
        {
            await _auditRepository.AddAsync(new Auditoria { Usuario = user, IpAddress = ip, Accion = action, Detalle = detail });
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
        public async Task DeleteAsync(int id, string auditUser, string auditIp)
        {
            await _repository.DeleteAsync(id);
            await _auditRepository.AddAsync(new Auditoria
            {
                Usuario = auditUser,
                IpAddress = auditIp,
                Accion = "DELETE_COMUNA",
                Detalle = $"ComunaId: {id}"
            });
            _logger.LogInformation("Comuna {Id} eliminada exitosamente por {User}.", id, auditUser);
        }
    }
}

