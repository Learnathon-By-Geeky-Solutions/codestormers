using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CosmoVerse.Data;
using CosmoVerse.Models;
using Microsoft.EntityFrameworkCore;
using CosmoVerse.Models.Domain;
using CosmoVerse.Repositories;

namespace CosmoVerse.Services
{
    public class PlanetService : IPlanetService
    {
        private readonly ApplicationDbContext _context;

        public PlanetService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<PlanetSummary>> GetPlanetSummariesAsync()
        {
            return await _context.Planets
                .Select(p => new PlanetSummary
                {
                    Id = p.Id,
                    Name = p.Name,
                    DistanceFromSun = p.DistanceFromSun,
                    Diameter = p.Diameter,
                    RotationPeriod = p.RotationPeriod,
                    OrbitalPeriod = p.OrbitalPeriod,
                    MediaUrl = p.MediaUrl
                })
                .ToListAsync();
        }

        public async Task<Planet?> GetPlanetByIdAsync(Guid id, bool includeSatellites = false)
        {
            var query = _context.Planets.AsQueryable()
                .Where(p => p.Id == id);

            if (includeSatellites)
            {
                query = query.Include(p => p.Satellites);
            }

            return await query.FirstOrDefaultAsync();
        }

        public async Task<Planet> CreatePlanetAsync(Planet planet)
        {
            if (planet == null) throw new ArgumentNullException(nameof(planet));
            if (string.IsNullOrEmpty(planet.Name)) throw new ArgumentException("Planet name is required", nameof(planet.Name));
            if (planet.Name.Length > 100) throw new ArgumentException("Planet name cannot exceed 100 characters", nameof(planet.Name));

            planet.Id = Guid.NewGuid();
            planet.CreatedAt = DateTime.UtcNow;
            planet.UpdatedAt = DateTime.UtcNow;

            _context.Planets.Add(planet);
            await _context.SaveChangesAsync();
            return planet;
        }

        public async Task<bool> UpdatePlanetAsync(Planet planet)
        {
            if (planet == null || string.IsNullOrEmpty(planet.Name) || planet.Name.Length > 100) return false;

            var existingPlanet = await _context.Planets.FindAsync(planet.Id);
            if (existingPlanet == null) return false;

            existingPlanet.Name = planet.Name;
            existingPlanet.DistanceFromSun = planet.DistanceFromSun;
            existingPlanet.Diameter = planet.Diameter;
            existingPlanet.RotationPeriod = planet.RotationPeriod;
            existingPlanet.OrbitalPeriod = planet.OrbitalPeriod;
            existingPlanet.Description = planet.Description;
            existingPlanet.MediaUrl = planet.MediaUrl;
            existingPlanet.UpdatedAt = DateTime.UtcNow;

            _context.Planets.Update(existingPlanet);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeletePlanetAsync(Guid id)
        {
            var planet = await _context.Planets.FindAsync(id);
            if (planet == null) return false;

            _context.Planets.Remove(planet);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}