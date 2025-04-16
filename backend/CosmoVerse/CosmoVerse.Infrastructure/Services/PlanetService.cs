using Azure.Core;
using CosmoVerse.Application.DTOs;
using CosmoVerse.Application.Interfaces;
using CosmoVerse.Domain.Entities;

namespace CosmoVerse.Infrastructure.Services
{
    public class PlanetService : IPlanetService
    {
        private readonly IRepository<Planet, Guid> _planetRepository;
        public PlanetService(IRepository<Planet, Guid> planetRepository)
        {
            _planetRepository = planetRepository;
        }

        public async Task<bool> CreatePlanetAsync(PlanetDto request)
        {
            // 1. Map the PlanetDto to Planet entity  
            var planet = new Planet
            {
                Name = request.Name,
                Introduction = request.Introduction,
                Namesake = request.Namesake,
                PotentialForLife = request.PotentialForLife,
                SizeAndDistance = request.SizeAndDistance,
                OrbitAndRotation = request.OrbitAndRotation,
                Moons = request.Moons,
                Rings = request.Rings,
                Formation = request.Formation,
                Structure = request.Structure,
                Surface = request.Surface,
                Atmosphere = request.Atmosphere,
                Magnetosphere = request.Magnetosphere,
                CelestialSystemId = request.CelestialSystemId
            };

            // 2. Add the Planet entity to the database using the repository  
            await _planetRepository.AddAsync(planet);
 
            return true;
        }

        public async Task<bool> DeletePlanetAsync(Guid planetId)
        {
            var planet = await _planetRepository.FindByIdAsync(planetId);
            if (planet == null)
            {
                return false; 
            }

            await _planetRepository.DeleteAsync(planet);
            await _planetRepository.SaveChangesAsync();
            return true;
        } 

        public async Task<List<object>> GetAllPlanetsAsync()
        {
            var planets = await _planetRepository.FindWithProjectionAsync(
                predicate: _ => true,
                selector: planet => new
                {
                    planet.Id,
                    planet.Name
                }
                );
            return planets.Cast<object>().ToList();
        }

        public async Task<List<object>> GetPlanetByIdAsync(Guid planetId)
        {
            var planets = await _planetRepository.FindWithProjectionAsync(
                predicate: planet => planet.Id == planetId,
                selector: planet => new
                {
                    planet.Id,
                    planet.Name,
                    planet.Introduction,
                    planet.Namesake,
                    planet.PotentialForLife,
                    planet.SizeAndDistance,
                    planet.OrbitAndRotation,
                    planet.Moons,
                    planet.Rings,
                    planet.Formation,
                    planet.Structure,
                    planet.Surface,
                    planet.Atmosphere,
                    planet.Magnetosphere,

                    Satellites = planet.Satellites.Select(s => new { s.Id, s.Name }).ToList()
                }
            );
            return planets.Cast<object>().ToList();
        }

        public async Task<bool> UpdatePlanetAsync(Guid Id, PlanetDto planetDto)
        {
            var planet = await _planetRepository.FindByIdAsync(Id);
            if (planet == null)
            {
                return false; // Planet not found
            }

            // Update the planet's properties
            planet.Introduction = planetDto.Introduction;
            planet.Namesake = planetDto.Namesake;
            planet.PotentialForLife = planetDto.PotentialForLife;
            planet.SizeAndDistance = planetDto.SizeAndDistance;
            planet.OrbitAndRotation = planetDto.OrbitAndRotation;
            planet.Moons = planetDto.Moons;
            planet.Rings = planetDto.Rings;
            planet.Formation = planetDto.Formation;
            planet.Structure = planetDto.Structure;
            planet.Surface = planetDto.Surface;
            planet.Atmosphere = planetDto.Atmosphere;
            planet.Magnetosphere = planetDto.Magnetosphere;
            planet.CelestialSystemId = planetDto.CelestialSystemId;
            

            await _planetRepository.UpdateAsync(planet);
            return true;
        }
    }
}
