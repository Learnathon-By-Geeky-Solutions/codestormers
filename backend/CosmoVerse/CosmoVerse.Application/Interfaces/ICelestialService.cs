using CosmoVerse.Application.DTOs;

namespace CosmoVerse.Application.Interfaces
{
    public interface ICelestialService
    {
        Task<List<object>> GetAllCelestialSystemsAsync();
        Task<object> GetCelestialSystemByIdAsync(Guid id);
        Task<bool> CreateCelestialSystemAsync(CelestialSystemDto celestialSystem);
        Task<bool> UpdateCelestialSystemAsync(Guid id, CelestialSystemDto celestialSystem);
        Task<bool> DeleteCelestialSystemAsync(Guid id);
    }
}
