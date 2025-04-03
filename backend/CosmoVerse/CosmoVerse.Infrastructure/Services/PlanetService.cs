using CosmoVerse.Models.Domain;
using CosmoVerse.Repositories;

namespace CosmoVerse.Services
{
    public class PlanetService : IPlanetService
    {
        private readonly IRepository<Planet, Guid> _planetRepository;

        public PlanetService(IRepository<Planet, Guid> planetRepository)
        {
            _planetRepository = planetRepository ?? throw new ArgumentNullException(nameof(planetRepository));
        }

        public async Task<IEnumerable<Planet>> GetAllPlanetsAsync(bool includeSatellites = false)
        {
            return includeSatellites
                ? await _planetRepository.FindAllAsync(p => true, p => p.Satellites)
                : await _planetRepository.FindAllAsync(p => true);
        }

        public async Task<Planet?> GetPlanetByIdAsync(Guid id)
        {
            var planet = await _planetRepository.FindAsync(p => p.Id == id, p => p.Satellites);
            if (planet == null)
            {
                Console.WriteLine($"[Warning] Planet with ID {id} not found.");
            }
            return planet;
        }

        public async Task<Planet> CreatePlanetAsync(Planet planet)
        {
            if (planet == null)
                throw new ArgumentNullException(nameof(planet));

            await _planetRepository.AddAsync(planet);
            return planet;
        }

        public async Task<bool> UpdatePlanetAsync(Guid id, Planet updatedPlanet)
        {
            if (updatedPlanet == null || id != updatedPlanet.Id)
                return false;

            var existingPlanet = await _planetRepository.FindByIdAsync(id);
            if (existingPlanet == null)
                return false;

            existingPlanet.Name = updatedPlanet.Name;
            existingPlanet.Mass = updatedPlanet.Mass;
            existingPlanet.Radius = updatedPlanet.Radius;
            existingPlanet.OrbitalPeriod = updatedPlanet.OrbitalPeriod;
            existingPlanet.UpdatedAt = DateTime.UtcNow;

            await _planetRepository.UpdateAsync(existingPlanet);
            return true;
        }


        public async Task<bool> DeletePlanetAsync(Guid id)
        {
            var planet = await _planetRepository.FindByIdAsync(id);
            if (planet == null) 
            { 
                Console.WriteLine($"[Warning] Attempted to delete non-existent planet with ID {id}.");
                return false;
            }
            await _planetRepository.DeleteAsync(id);
            return true;
        }
    }
}
