using CosmoVerse.Models.Domain;

namespace CosmoVerse.Services
{
    public interface IPlanetService
    {
        Task<IEnumerable<Planet>> GetAllPlanetsAsync(bool includeSatellites);
        Task<Planet?> GetPlanetByIdAsync(Guid id);
        Task<Planet> CreatePlanetAsync(Planet planet);
        Task<bool> UpdatePlanetAsync(Guid id, Planet updatedPlanet);
        Task<bool> DeletePlanetAsync(Guid id);
    }
}
