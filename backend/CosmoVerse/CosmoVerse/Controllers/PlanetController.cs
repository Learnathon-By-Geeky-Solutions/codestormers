using Microsoft.AspNetCore.Mvc;
using CosmoVerse.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;

namespace CosmoVerse.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    //[Authorize]
    public class PlanetController : ControllerBase
    {
        private readonly IPlanetService _planetService;

        public PlanetController(IPlanetService planetService)
        {
            _planetService = planetService;
        }

        /// <summary>
        /// Retrieves a list of all planets in the system.
        /// </summary>
        /// <returns>
        /// An HTTP response containing a list of planets if successful, or an error message if the request fails.
        /// </returns>
        /// <response code="200">Returns a list of planets.</response>
        /// <response code="500">An unexpected error occurred while processing the request.</response>
        [HttpGet("get-all-planets")]
        public async Task<IActionResult> GetAllPlanets()
        {
            try
            {
                var planets = await _planetService.GetAllPlanetsAsync();
                return Ok(planets);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An unexpected error occurred. Please try again later.");
            }
        }

        /// <summary>
        /// Retrieves the details of a specific planet by its unique identifier.
        /// </summary>
        /// <param name="planetId">The unique identifier of the planet to retrieve.</param>
        /// <returns>
        /// An HTTP response containing the planet details if found, or an appropriate error response if not.
        /// </returns>
        /// <response code="200">The requested planet details are successfully retrieved.</response>
        /// <response code="400">The request is invalid, typically due to an empty or improperly formatted planet ID.</response>
        /// <response code="404">No planet is found with the specified ID.</response>
        /// <response code="500">An unexpected server error occurred while processing the request.</response>
        [HttpGet("{planetId}")]
        public async Task<IActionResult> GetPlanetById(Guid planetId)
        {
            if (planetId == Guid.Empty)
            {
                return BadRequest("Invalid planet ID");
            }

            try
            {
                var planet = await _planetService.GetPlanetByIdAsync(planetId);
                if (planet == null)
                {
                    return NotFound("Planet not found");
                }
                return Ok(planet);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An unexpected error occurred. Please try again later.");
            }
        }
    }
}