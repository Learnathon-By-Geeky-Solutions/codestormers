using CosmoVerse.Application.DTOs;
using CosmoVerse.Application.Interfaces;
using CosmoVerse.Models;
using CosmoVerse.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CosmoVerse.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Policy = "AdminOnly")]
    public class AdminController : ControllerBase
    {
        private readonly IPlanetService _planetService;
        private readonly ISatelliteService _satelliteService;
        private readonly ICelestialService _celestialService;
        private readonly IAuthService _authService;

        public AdminController(IPlanetService planetService, ISatelliteService satelliteService, ICelestialService celestialService, IAuthService authService)
        {
            _planetService = planetService;
            _satelliteService = satelliteService;
            _celestialService = celestialService;
            _authService = authService;
        }

        [HttpPost("create-celestial-system")]
        public async Task<ActionResult> CreateCelestialSystem([FromBody] CelestialSystemDto request)
        {
            try
            {
                var result = await _celestialService.CreateCelestialSystemAsync(request);
                if (result)
                {
                    return Ok("Celestial system created successfully");
                }
                else
                {
                    return BadRequest("Failed to create celestial system");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("update-celestial-system")]
        public async Task<ActionResult> UpdateCelestialSystem(Guid Id, [FromBody] CelestialSystemDto request)
        {
            try
            {
                var result = await _celestialService.UpdateCelestialSystemAsync(Id, request);
                if (result)
                {
                    return Ok("Celestial system updated successfully");
                }
                else
                {
                    return BadRequest("Failed to update celestial system");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("delete-celestial-system/{id}")]
        public async Task<ActionResult> DeleteCelestialSystem([FromRoute]Guid id)
        {
            try
            {
                var result = await _celestialService.DeleteCelestialSystemAsync(id);
                if (result)
                {
                    return Ok("Celestial system deleted successfully");
                }
                else
                {
                    return BadRequest("Failed to delete celestial system");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("create-planet")]
        public async Task<ActionResult> CreatePlanet([FromBody] PlanetDto request)
        {
            try
            {
                var result = await _planetService.CreatePlanetAsync(request);
                if (result)
                {
                    return Ok("Planet created successfully");
                }
                else
                {
                    return BadRequest("Failed to create planet");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("update-planet/{id}")]
        public async Task<ActionResult> UpdatePlanet(Guid id, [FromBody] PlanetDto request)
        {
            try
            {
                var result = await _planetService.UpdatePlanetAsync(id, request);
                if (result)
                {
                    return Ok("Planet updated successfully");
                }
                else
                {
                    return BadRequest("Failed to update planet");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("delete-planet/{id}")]
        public async Task<ActionResult> DeletePlanet(Guid id)
        {
            try
            {
                var result = await _planetService.DeletePlanetAsync(id);
                if (result)
                {
                    return Ok("Planet deleted successfully");
                }
                else
                {
                    return BadRequest("Failed to delete planet");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("create-satellite")]
        public async Task<ActionResult> CreateSatellite([FromBody] SatelliteDto request)
        {
            try
            {
                var result = await _satelliteService.CreateSatelliteAsync(request);
                if (result)
                {
                    return Ok("Satellite created successfully");
                }
                else
                {
                    return BadRequest("Failed to create satellite");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("update-satellite/{id}")]
        public async Task<ActionResult> UpdateSatellite(Guid id, [FromBody] SatelliteDto request)
        {
            try
            {
                var result = await _satelliteService.UpdateSatelliteAsync(id, request);
                if (result)
                {
                    return Ok("Satellite updated successfully");
                }
                else
                {
                    return BadRequest("Failed to update satellite");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("delete-satellite/{id}")]
        public async Task<ActionResult> DeleteSatellite(Guid id)
        {
            try
            {
                var result = await _satelliteService.DeleteSatelliteAsync(id);
                if (result)
                {
                    return Ok("Satellite deleted successfully");
                }
                else
                {
                    return BadRequest("Failed to delete satellite");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("get-all-users")]
        public async Task<ActionResult<List<Object>>> GetAllUsers()
        {
            try
            {
                var users = await _authService.GetAllUsersAsync();
                return Ok(users);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("delete-user/{id}")]
        public async Task<ActionResult> DeleteUser(Guid id)
        {
            try
            {
                var result = await _authService.DeleteUserAsync(id);
                if (result)
                {
                    return Ok("User deleted successfully");
                }
                else
                {
                    return BadRequest("Failed to delete user");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
