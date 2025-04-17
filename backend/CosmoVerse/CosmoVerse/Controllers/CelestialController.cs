using CosmoVerse.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CosmoVerse.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
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
        /// <response code="400">If an error occurs while processing the request.</response>
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
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Retrieves a specific celestial body by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the celestial body.</param>
        /// <returns>
        /// The requested celestial body, or a not found error if the body is not found.
        /// </returns>
        /// <response code="200">Returns the requested celestial body.</response>
        /// <response code="400">If an error occurs while processing the request.</response>
        /// <response code="404">If the celestial body with the specified ID is not found.</response>
        [HttpGet("{id}")]
        public async Task<ActionResult> GetCelestialBody(Guid id)
        {
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
                return BadRequest(ex.Message);
            }
        }
    }
}
