using CosmoVerse.Application.DTOs;
using CosmoVerse.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
