using CosmoVerse.Data;
using CosmoVerse.Models.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CosmoVerse.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class PlanetController : ControllerBase
	{
		private readonly ApplicationDbContext _context;

		public PlanetController(ApplicationDbContext context)
		{
			_context = context;
		}

		//  Get all planets
		[HttpGet]
		public async Task<ActionResult<IEnumerable<Planet>>> GetPlanets()
		{
			var planets = await _context.Planets.Include(p => p.Star).Include(p => p.Satellites).ToListAsync();
			return Ok(planets);
		}

		//  Get a specific planet by ID
		[HttpGet("{id}")]
		public async Task<ActionResult<Planet>> GetPlanet(Guid id)
		{
			var planet = await _context.Planets
				.Include(p => p.Star)
				.Include(p => p.Satellites)
				.FirstOrDefaultAsync(p => p.Id == id);

			if (planet == null)
				return NotFound("Planet not found");

			return Ok(planet);
		}

		//  Create a new planet
		[HttpPost]
		public async Task<ActionResult<Planet>> CreatePlanet([FromBody] Planet planet)
		{
			_context.Planets.Add(planet);
			await _context.SaveChangesAsync();
			return CreatedAtAction(nameof(GetPlanet), new { id = planet.Id }, planet);
		}

		//  Update an existing planet
		[HttpPut("{id}")]
		public async Task<IActionResult> UpdatePlanet(Guid id, [FromBody] Planet updatedPlanet)
		{
			var planet = await _context.Planets.FindAsync(id);
			if (planet == null)
				return NotFound("Planet not found");

			planet.Name = updatedPlanet.Name;
			planet.Mass = updatedPlanet.Mass;
			planet.Radius = updatedPlanet.Radius;
			planet.OrbitalPeriod = updatedPlanet.OrbitalPeriod;
			planet.StarId = updatedPlanet.StarId;
			planet.UpdatedAt = DateTime.UtcNow;

			await _context.SaveChangesAsync();
			return NoContent();
		}

		//  Delete a planet
		[HttpDelete("{id}")]
		public async Task<IActionResult> DeletePlanet(Guid id)
		{
			var planet = await _context.Planets.FindAsync(id);
			if (planet == null)
				return NotFound("Planet not found");

			_context.Planets.Remove(planet);
			await _context.SaveChangesAsync();
			return NoContent();
		}
	}
}
