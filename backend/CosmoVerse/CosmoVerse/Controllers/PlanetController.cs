using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CosmoVerse.Models;
using CosmoVerse.Services;

namespace CosmoVerse.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PlanetController : ControllerBase
    {
        private readonly IPlanetService _planetService;

        public PlanetController(IPlanetService planetService)
        {
            _planetService = planetService;
        }

        [HttpGet("summaries")]
        public async Task<IActionResult> GetPlanetSummaries()
        {
            var summaries = await _planetService.GetPlanetSummariesAsync();
            return Ok(summaries);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Planet>> GetPlanet(Guid id, [FromQuery] bool includeSatellites = false)
        {
            var planet = await _planetService.GetPlanetByIdAsync(id, includeSatellites);
            if (planet == null) return NotFound();
            return Ok(planet);
        }
    }
}