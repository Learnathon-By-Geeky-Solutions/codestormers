using CosmoVerse.Models.Domain;
using CosmoVerse.Services;
using Microsoft.AspNetCore.Mvc;

namespace CosmoVerse.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlanetController : ControllerBase
    {
        private readonly IPlanetService _planetService;

        public PlanetController(IPlanetService planetService)
        {
            _planetService = planetService ?? throw new ArgumentNullException(nameof(planetService));
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Planet>>> GetPlanets([FromQuery] bool includeSatellites = false)
        {
            try
            {
                var planets = await _planetService.GetAllPlanetsAsync(includeSatellites);
                return Ok(planets);
            }
            catch (Exception)
            {
                return StatusCode(500, "An error occurred while retrieving planets.");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Planet>> GetPlanet(Guid id)
        {
            try
            {
                var planet = await _planetService.GetPlanetByIdAsync(id);
                if (planet == null)
                    return NotFound("Planet not found");

                return Ok(planet);
            }
            catch (Exception)
            {
                return StatusCode(500, "An error occurred while retrieving the planet.");
            }
        }

        [HttpPost]
        public async Task<ActionResult<Planet>> CreatePlanet([FromBody] Planet planet)
        {
            if (planet == null || !ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var createdPlanet = await _planetService.CreatePlanetAsync(planet);
                return CreatedAtAction(nameof(GetPlanet), new { id = createdPlanet.Id }, createdPlanet);
            }
            catch (Exception)
            {
                return StatusCode(500, "An error occurred while creating the planet.");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePlanet(Guid id, [FromBody] Planet updatedPlanet)
        {
            if (updatedPlanet == null || id != updatedPlanet.Id || !ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var success = await _planetService.UpdatePlanetAsync(id, updatedPlanet);
                if (!success)
                    return NotFound("Planet not found");

                return NoContent();
            }
            catch (Exception)
            {
                return StatusCode(500, "An error occurred while updating the planet.");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePlanet(Guid id)
        {
            try
            {
                var success = await _planetService.DeletePlanetAsync(id);
                if (!success)
                    return NotFound("Planet not found");

                return NoContent();
            }
            catch (Exception)
            {
                return StatusCode(500, "An error occurred while deleting the planet.");
            }
        }
    }
}
