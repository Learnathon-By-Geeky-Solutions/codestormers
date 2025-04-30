using CosmoVerse.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CosmoVerse.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class CelestialController : ControllerBase
    {
        private readonly ICelestialService _celestialService;
        public CelestialController(ICelestialService celestialService)
        {
            _celestialService = celestialService;
        }

        /// <summary>
        /// Retrieves a list of all celestial bodies from the system.
        /// </summary>
        /// <returns>
        /// A list of celestial bodies or an error message if the request fails.
        /// </returns>
        /// <response code="200">Returns a list of celestial bodies.</response>
        /// <response code="500">If an error occurs while processing the request.</response>
        [HttpGet("get-all-celestials")]
        public async Task<ActionResult<List<object>>> GetCelestialBodies()
        {
            try
            {
                var result = await _celestialService.GetAllCelestialSystemsAsync();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An unexpected error occurred. Please try again later.");
            }
        }

        /// <summary>
        /// Retrieves a specific celestial body by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the celestial body to retrieve.</param>
        /// <returns>
        /// An HTTP response containing the requested celestial body details if found, or an appropriate error response if not.
        /// </returns>
        /// <response code="200">Successfully retrieves the requested celestial body.</response>
        /// <response code="400">The provided celestial body ID is invalid.</response>
        /// <response code="404">No celestial body is found with the specified ID.</response>
        /// <response code="500">An unexpected server error occurred while processing the request.</response>
        [HttpGet("{id}")]
        public async Task<ActionResult> GetCelestialBody(Guid id)
        {
            if (id == Guid.Empty)
            {
                return BadRequest("Invalid celestial body ID");
            }
            try
            {
                var result = await _celestialService.GetCelestialSystemByIdAsync(id);
                if (result == null)
                {
                    return NotFound("Celestial body not found");
                }
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An unexpected error occurred. Please try again later.");
            }
        }
    }
}
