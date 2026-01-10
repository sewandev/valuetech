using ValueTech.Data.Interfaces;
using ValueTech.Api.Contracts.Responses;

namespace ValueTech.Api.Services
{
    public class RegionService : IRegionService
    {
        private readonly IRegionRepository _repository;

        public RegionService(IRegionRepository repository)
        {
            _repository = repository;
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
    }
}
