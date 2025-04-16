using CosmoVerse.Application.DTOs;

namespace CosmoVerse.Application.Interfaces
{
    public interface ISatelliteService
    {
        Task<bool> CreateSatelliteAsync(SatelliteDto request);
        Task<bool> UpdateSatelliteAsync(Guid Id, SatelliteDto request);
        Task<bool> DeleteSatelliteAsync(Guid satelliteId);
        Task<SatelliteInfoDto> GetSatelliteByIdAsync(Guid satelliteId);
        Task<List<object>> GetAllSatelliteAsync();
    }
}
