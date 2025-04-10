using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CosmoVerse.Models;

namespace CosmoVerse.Services
{
    public interface IPlanetService
    {
        Task<List<PlanetSummary>> GetPlanetSummariesAsync();
        Task<Planet?> GetPlanetByIdAsync(Guid id, bool includeSatellites = false);
        Task<Planet> CreatePlanetAsync(Planet planet);
        Task<bool> UpdatePlanetAsync(Planet planet);
        Task<bool> DeletePlanetAsync(Guid id);
    }
}