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