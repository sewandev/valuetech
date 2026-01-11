using ValueTech.Data.Interfaces;
using ValueTech.Api.Contracts.Responses;
using Microsoft.Extensions.Logging;


namespace ValueTech.Api.Services
{
    public class RegionService : IRegionService
    {
        private readonly IRegionRepository _repository;
        private readonly IAuditoriaRepository _auditRepository;
        private readonly ILogger<RegionService> _logger;

        public RegionService(IRegionRepository repository, IAuditoriaRepository auditRepository, ILogger<RegionService> logger)
        {
            _repository = repository;
            _auditRepository = auditRepository;
            _logger = logger;
        }

        public async Task<IEnumerable<RegionResponse>> GetAllAsync()
        {
            var regions = await _repository.GetAllAsync();
            return regions.Select(r => new RegionResponse 
            { 
                IdRegion = r.IdRegion, 
                Nombre = r.Nombre 
            });
        }

        public async Task<RegionResponse?> GetByIdAsync(int id)
        {
            var region = await _repository.GetByIdAsync(id);
            if (region == null) return null;
            
            return new RegionResponse
            {
                IdRegion = region.IdRegion,
                Nombre = region.Nombre
            };
        }

        public async Task DeleteAsync(int id, string auditUser, string auditIp)
        {
            await _repository.DeleteAsync(id);
            await _auditRepository.AddAsync(new Data.Models.Auditoria
            {
                Usuario = auditUser,
                IpAddress = auditIp,
                Accion = "DELETE_REGION",
                Detalle = $"RegionId: {id}"
            });
            _logger.LogInformation("Region {Id} eliminada por {User}.", id, auditUser);
        }
    }
}
