using CosmoVerse.Application.DTOs;
using CosmoVerse.Application.Interfaces;
using CosmoVerse.Domain.Entities;

namespace CosmoVerse.Infrastructure.Services
{
    public class CelestialService : ICelestialService
    {
        private readonly IRepository<CelestialSystem, Guid> _celestialSystemRepository;
        public CelestialService(IRepository<CelestialSystem, Guid> celestialSystemRepository)
        {
            _celestialSystemRepository = celestialSystemRepository;
        }
        public async Task<bool> CreateCelestialSystemAsync(CelestialSystemDto celestialSystem)
        {
            var celestialSystemEntity = new CelestialSystem
            {
                Id = Guid.NewGuid(),
                Name = celestialSystem.Name,
                Description = celestialSystem.Description,
                Type = celestialSystem.Type,
                Structure = celestialSystem.Structure,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
            };
            try
            {
                await _celestialSystemRepository.AddAsync(celestialSystemEntity);
                return true;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Error creating celestial system", ex);
            }
        }

        public async Task<bool> DeleteCelestialSystemAsync(Guid id)
        {
            try
            {
                await _celestialSystemRepository.DeleteAsync(id);
                return true;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Error while deleting celestial system", ex);
            }
        }

        public async Task<List<object>> GetAllCelestialSystemsAsync()
        {
            var celestials = await _celestialSystemRepository.FindWithProjectionAsync(
                predicate: _ => true,
                selector: celestial => new
                {
                    Id = celestial.Id,
                    Name = celestial.Name,
                    Description = celestial.Description,
                    Type = celestial.Type,
                    Structure = celestial.Structure,
                } 
                );

            return celestials.Cast<object>().ToList();
        }

        public async Task<object> GetCelestialSystemByIdAsync(Guid id)
        {
            var celestialSystem = await _celestialSystemRepository.FindWithProjectionAsync(
                predicate: celestial => celestial.Id == id,
                selector: celestial => new
                {
                    Id = celestial.Id,
                    Name = celestial.Name,
                    Description = celestial.Description,
                    Type = celestial.Type,
                    Structure = celestial.Structure,
                    Planets = celestial.Planets.Select(p => new { p.Id, p.Name }).ToList()
                }
            );

            if (celestialSystem == null || !celestialSystem.Any())
            {
                throw new InvalidOperationException("Celestial system not found");
            }

            return celestialSystem.Cast<object>().ToList();
        }

        public async Task<bool> UpdateCelestialSystemAsync(Guid id, CelestialSystemDto celestialSystem)
        {
            var celestialSystemEntity = await _celestialSystemRepository.FindByIdAsync(id);
            if (celestialSystemEntity == null)
            {
                throw new InvalidOperationException("Celestial system not found");
            }
            celestialSystemEntity.Name = celestialSystem.Name;
            celestialSystemEntity.Description = celestialSystem.Description;
            celestialSystemEntity.Type = celestialSystem.Type;
            celestialSystemEntity.Structure = celestialSystem.Structure;
            celestialSystemEntity.UpdatedAt = DateTime.UtcNow;

            try
            {
                await _celestialSystemRepository.UpdateAsync(celestialSystemEntity);
                return true;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Error while updating celestial system", ex);
            }
        }
    }
}
