using Microsoft.AspNetCore.Mvc;
using CosmoVerse.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;

namespace CosmoVerse.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
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
        /// A list of planets or an error message if the request fails.
        /// </returns>
        /// <response code="200">Returns a list of planets.</response>
        /// <response code="400">If an error occurs while processing the request.</response>
        [HttpGet("get-all-planets")]
        public async Task<IActionResult> GetAllPlanets()
        {
            try
            {
                var palnets = await _planetService.GetAllPlanetsAsync();
                return Ok(palnets);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Retrieves a specific planet by its unique identifier.
        /// </summary>
        /// <param name="planetId">The unique identifier of the planet.</param>
        /// <returns>
        /// The requested planet, or a not found error if the planet is not found.
        /// </returns>
        /// <response code="200">Returns the requested planet.</response>
        /// <response code="400">If an error occurs while processing the request.</response>
        /// <response code="404">If the planet with the specified ID is not found.</response>
        [HttpGet("{planetId}")]
        public async Task<IActionResult> GetPlanetById(Guid planetId)
        {
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
                return BadRequest(ex.Message);
            }
        }
    }
}