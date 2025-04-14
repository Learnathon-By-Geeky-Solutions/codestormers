using CosmoVerse.Application.DTOs;
using CosmoVerse.Application.Interfaces;
using CosmoVerse.Models;
using CosmoVerse.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CosmoVerse.Infrastructure.Services
{
    public class SatelliteService : ISatelliteService
    {
        private readonly IRepository<Satellite, Guid> _satelliteRepository;

        public SatelliteService(IRepository<Satellite, Guid> satelliteRepository)
        {
            _satelliteRepository = satelliteRepository;
        }

        public async Task<bool> CreateSatelliteAsync(SatelliteDto request)
        {
            var satelliteEntity = new Satellite
            {
                Id = Guid.NewGuid(),
                Name = request.Name,
                Size = request.Size,
                DistanceFromPlanet = request.DistanceFromPlanet,
                OrbitalPeriod = request.OrbitalPeriod,
                Description = request.Description,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                PlanetId = request.PlanetId,
            };
            try
            {
                await _satelliteRepository.AddAsync(satelliteEntity);
                return true;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Error creating satellite", ex);
            }
        }

        public async Task<bool> DeleteSatelliteAsync(Guid satelliteId)
        {
            try
            {
                await _satelliteRepository.DeleteAsync(satelliteId);
                return true;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Error while deleting satellite", ex);
            }
        }

        public async Task<List<object>> GetAllSatelliteAsync()
        {
            var satellites = await _satelliteRepository.FindWithProjectionAsync(
                predicate: _ => true,
                selector: satellite => new
                {
                    satellite.Id,
                    satellite.Name,
                }
                );
            return satellites.Cast<object>().ToList();
        }

        public async Task<SatelliteInfoDto> GetSatelliteByIdAsync(Guid satelliteId)
        {
            try
            {
                var satellites = await _satelliteRepository.FindByIdAsync(satelliteId);
                if (satellites == null)
                {
                    return null; // Satellite not found
                }
                var satellite = new SatelliteInfoDto
                {
                    Id = satellites.Id,
                    Name = satellites.Name,
                    Size = satellites.Size,
                    DistanceFromPlanet = satellites.DistanceFromPlanet,
                    OrbitalPeriod = satellites.OrbitalPeriod,
                    Description = satellites.Description,
                };
                return satellite;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Error while getting satellite", ex);
            }
        }

        public async Task<bool> UpdateSatelliteAsync(Guid Id, SatelliteDto request)
        {
            var satellite = await _satelliteRepository.FindByIdAsync(Id);
            if (satellite == null)
            {
                return false; // Planet not found
            }

            try
            {
                satellite.Name = request.Name;
                satellite.Size = request.Size;
                satellite.DistanceFromPlanet = request.DistanceFromPlanet;
                satellite.Description = request.Description;
                satellite.UpdatedAt = DateTime.UtcNow;

                if (request?.PlanetId != null)
                {
                    satellite.PlanetId = request.PlanetId;
                }
                await _satelliteRepository.UpdateAsync(satellite);
                return true;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Error while updating satellite", ex);

            }
        }
    }
}
