﻿using CosmoVerse.Application.DTOs;
using CosmoVerse.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CosmoVerse.Application.Services
{
    public interface IPlanetService
    {
        Task<bool> CreatePlanetAsync(PlanetDto request);
        Task<bool> UpdatePlanetAsync(Guid Id, PlanetDto planetDto);
        Task<bool> DeletePlanetAsync(Guid planetId);
        Task<Planet> GetPlanetByIdAsync(Guid planetId);
        Task<List<Planet>> GetAllPlanetsAsync();
    }
}
