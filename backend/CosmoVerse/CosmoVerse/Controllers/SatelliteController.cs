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
        /// Retrieves a list of all satellites in the system.
        /// </summary>
        /// <returns>
        /// An HTTP response containing a list of satellites if successful, or an error message if the request fails.
        /// </returns>
        /// <response code="200">Successfully retrieves the list of satellites.</response>
        /// <response code="500">An unexpected server error occurred while processing the request.</response>
        [HttpGet("get-all-satellites")]
        public async Task<IActionResult> GetAllSatellites()
        {
            try
            {
                var satellites = await _satelliteService.GetAllSatelliteAsync();
                return Ok(satellites);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An unexpected error occurred. Please try again later.");
            }
        }

        /// <summary>
        /// Retrieves a specific satellite by its unique identifier.
        /// </summary>
        /// <param name="satelliteId">The unique identifier of the satellite to retrieve.</param>
        /// <returns>
        /// An HTTP response containing the requested satellite details if found, or an appropriate error response if not.
        /// </returns>
        /// <response code="200">Successfully retrieves the requested satellite.</response>
        /// <response code="400">The provided satellite ID is invalid.</response>
        /// <response code="404">No satellite is found with the specified ID.</response>
        /// <response code="500">An unexpected server error occurred while processing the request.</response>
        [HttpGet("{satelliteId}")]
        public async Task<ActionResult<SatelliteInfoDto>> GetSatelliteById(Guid satelliteId)
        {
            if (satelliteId == Guid.Empty)
            {
                return BadRequest("Invalid satellite ID");
            }
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
                return StatusCode(500, "An unexpected error occurred. Please try again later.");
            }
        }
    }
}
