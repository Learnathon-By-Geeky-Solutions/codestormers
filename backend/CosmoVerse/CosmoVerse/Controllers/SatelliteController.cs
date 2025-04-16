using CosmoVerse.Application.DTOs;
using CosmoVerse.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CosmoVerse.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class SatelliteController : ControllerBase
    {
        private readonly ISatelliteService _satelliteService;

        public SatelliteController(ISatelliteService satelliteService)
        {
            _satelliteService = satelliteService;
        }

        [HttpGet("get-all-satellite")]
        public async Task<IActionResult> GetAllSatellite()
        {
            try
            {
                var satellites = await _satelliteService.GetAllSatelliteAsync();
                return Ok(satellites);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{satelliteId}")]
        public async Task<ActionResult<SatelliteInfoDto>> GetSatelliteById(Guid satelliteId)
        {
            try
            {
                var satellite = await _satelliteService.GetSatelliteByIdAsync(satelliteId);
                if (satellite == null)
                {
                    return NotFound("Satellite not found");
                }
                return Ok(satellite);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
