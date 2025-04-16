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
