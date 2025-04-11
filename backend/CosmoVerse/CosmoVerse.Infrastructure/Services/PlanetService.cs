using CosmoVerse.Application.DTOs;
using CosmoVerse.Application.Services;
using CosmoVerse.Data;
using CosmoVerse.Models;
using CosmoVerse.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CosmoVerse.Infrastructure.Services
{
    internal class PlanetService : IPlanetService
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
                Magnetosphere = request.Magnetosphere
            };

            // 2. Add the Planet entity to the database using the repository  
            await _planetRepository.AddAsync(planet);

            // 3. Return a Task<bool> instead of a plain bool  
            return await Task.FromResult(true);
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

        public async Task<List<Planet>> GetAllPlanetsAsync()
        {
            var planets = await _planetRepository.GetAllAsync();
            return planets.ToList();
        }

        public async Task<Planet> GetPlanetByIdAsync(Guid planetId)
        {
            var planet = await _planetRepository.FindAsync(
            p => p.Id == planetId,
            p => p.Satellites);

            return planet;
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

            await _planetRepository.UpdateAsync(planet);
            return true;
        }
    }
}
