using CosmoVerse.Application.DTOs;

namespace CosmoVerse.Application.Interfaces
{
    public interface IPlanetService
    {
        Task<bool> CreatePlanetAsync(PlanetDto request);
        Task<bool> UpdatePlanetAsync(Guid Id, PlanetDto planetDto);
        Task<bool> DeletePlanetAsync(Guid planetId);
        Task<List<object>> GetPlanetByIdAsync(Guid planetId);
        Task<List<object>> GetAllPlanetsAsync();
    }
}
