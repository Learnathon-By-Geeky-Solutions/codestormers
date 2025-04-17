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

        /// <summary>
        /// Retrieves a list of all satellites.
        /// </summary>
        /// <returns>
        /// A list of all satellites, or an error message if the request fails.
        /// </returns>
        /// <response code="200">Returns a list of satellites.</response>
        /// <response code="400">If an error occurs while processing the request.</response>
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

        /// <summary>
        /// Retrieves a specific satellite by its unique identifier.
        /// </summary>
        /// <param name="satelliteId">The unique identifier of the satellite.</param>
        /// <returns>
        /// The requested satellite, or a not found error if the satellite is not found.
        /// </returns>
        /// <response code="200">Returns the requested satellite.</response>
        /// <response code="400">If an error occurs while processing the request.</response>
        /// <response code="404">If the satellite with the specified ID is not found.</response>
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
