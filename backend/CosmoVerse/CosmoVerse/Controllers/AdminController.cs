using CosmoVerse.Application.DTOs;
using CosmoVerse.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
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

        /// <summary>
        /// Creates a new celestial system.
        /// </summary>
        /// <param name="request">The details of the celestial system to create.</param>
        /// <returns>Action result indicating the success or failure of the operation.</returns>
        /// <response code="200">Celestial system created successfully.</response>
        /// <response code="400">Failed to create celestial system due to invalid data.</response>
        /// <response code="500">An unexpected error occurred while creating the celestial system.</response>
        /// <remarks>
        /// This endpoint creates a new celestial system based on the provided details in the request.
        /// </remarks>
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
                return StatusCode(500, "An unexpected error occurred. Please try again later.");
            }
        }

        /// <summary>
        /// Updates an existing celestial system.
        /// </summary>
        /// <param name="Id">The unique identifier of the celestial system to update.</param>
        /// <param name="request">The updated details of the celestial system.</param>
        /// <returns>Action result indicating the success or failure of the operation.</returns>
        /// <response code="200">Celestial system updated successfully.</response>
        /// <response code="400">Invalid celestial system ID or failed to update celestial system due to invalid data.</response>
        /// <response code="500">An unexpected error occurred while updating the celestial system.</response>
        /// <remarks>
        /// This endpoint allows updating the details of an existing celestial system identified by its unique ID.
        /// </remarks>
        [HttpPut("update-celestial-system")]
        public async Task<ActionResult> UpdateCelestialSystem(Guid Id, [FromBody] CelestialSystemDto request)
        {
            if (Id == Guid.Empty)
            {
                return BadRequest("Invalid celestial system ID.");
            }

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
                return StatusCode(500, "An unexpected error occurred. Please try again later.");
            }
        }

        /// <summary>
        /// Deletes an existing celestial system.
        /// </summary>
        /// <param name="id">The unique identifier of the celestial system to delete.</param>
        /// <returns>Action result indicating the success or failure of the operation.</returns>
        /// <response code="200">Celestial system deleted successfully.</response>
        /// <response code="400">Invalid celestial system ID or failed to delete the celestial system.</response>
        /// <response code="404">Celestial system with the given ID not found.</response>
        /// <response code="500">An unexpected error occurred while deleting the celestial system.</response>
        /// <remarks>
        /// This endpoint allows the deletion of an existing celestial system identified by its unique ID.
        /// </remarks>
        [HttpDelete("delete-celestial-system/{id}")]
        public async Task<ActionResult> DeleteCelestialSystem([FromRoute]Guid id)
        {
            if (id == Guid.Empty)
            {
                return BadRequest("Invalid celestial system ID.");
            }

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
                return StatusCode(500, "An unexpected error occurred. Please try again later.");
            }
        }

        /// <summary>
        /// Creates a new planet.
        /// </summary>
        /// <param name="request">The details of the planet to create.</param>
        /// <returns>Action result indicating the success or failure of the operation.</returns>
        /// <response code="200">Planet created successfully.</response>
        /// <response code="400">Failed to create planet due to invalid data or other issues.</response>
        /// <response code="500">An unexpected error occurred while creating the planet.</response>
        /// <remarks>
        /// This endpoint creates a new planet based on the details provided in the request body.
        /// If the planet creation is successful, a 200 OK response is returned. 
        /// If the creation fails due to invalid input or other reasons, a 400 BadRequest is returned.
        /// Any internal server errors during the creation process will result in a 500 InternalServerError.
        /// </remarks>
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
                return StatusCode(500, "An unexpected error occurred. Please try again later.");
            }
        }

        /// <summary>
        /// Updates an existing planet.
        /// </summary>
        /// <param name="id">The unique identifier of the planet to update.</param>
        /// <param name="request">The updated details of the planet.</param>
        /// <returns>Action result indicating the success or failure of the operation.</returns>
        /// <response code="200">Planet updated successfully.</response>
        /// <response code="400">Failed to update planet due to invalid input or other issues.</response>
        /// <response code="500">An unexpected error occurred while updating the planet.</response>
        /// <remarks>
        /// This endpoint updates the details of an existing planet identified by the provided ID.
        /// If the update is successful, a 200 OK response is returned.
        /// If the planet ID is invalid or the update fails for any reason, a 400 BadRequest is returned.
        /// If the specified planet is not found, a 404 NotFound is returned.
        /// Any internal errors during the update process will result in a 500 InternalServerError.
        /// </remarks>
        [HttpPut("update-planet/{id}")]
        public async Task<ActionResult> UpdatePlanet(Guid id, [FromBody] PlanetDto request)
        {
            if (id == Guid.Empty)
            {
                return BadRequest("Invalid planet ID.");
            }

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
                return StatusCode(500, "An unexpected error occurred. Please try again later.");
            }
        }

        /// <summary>
        /// Deletes an existing planet.
        /// </summary>
        /// <param name="id">The unique identifier of the planet to delete.</param>
        /// <returns>Action result indicating the success or failure of the operation.</returns>
        /// <response code="200">Planet deleted successfully.</response>
        /// <response code="400">Failed to delete planet due to invalid input or other issues.</response>
        /// <response code="500">An unexpected error occurred while deleting the planet.</response>
        /// <remarks>
        /// This endpoint deletes an existing planet identified by the provided ID.
        /// If the deletion is successful, a 200 OK response is returned.
        /// If the planet ID is invalid or the deletion fails for any reason, a 400 BadRequest is returned.
        /// If the specified planet is not found, a 404 NotFound is returned.
        /// Any internal errors during the deletion process will result in a 500 InternalServerError.
        /// </remarks>
        [HttpDelete("delete-planet/{id}")]
        public async Task<ActionResult> DeletePlanet(Guid id)
        {
            if (id == Guid.Empty)
            {
                return BadRequest("Invalid planet ID.");
            }

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
                return StatusCode(500, "An unexpected error occurred. Please try again later.");
            }
        }

        /// <summary>
        /// Creates a new satellite.
        /// </summary>
        /// <param name="request">The details of the satellite to create.</param>
        /// <returns>Action result indicating the success or failure of the operation.</returns>
        /// <response code="200">Satellite created successfully.</response>
        /// <response code="400">Failed to create satellite due to invalid input or other issues.</response>
        /// <response code="500">An unexpected error occurred while creating the satellite.</response>
        /// <remarks>
        /// This endpoint creates a new satellite using the provided details in the <paramref name="request"/>.
        /// If the satellite is created successfully, a 200 OK response is returned.
        /// If the satellite creation fails due to invalid input or other issues, a 400 BadRequest is returned.
        /// Any internal errors that occur during the creation process will result in a 500 InternalServerError.
        /// </remarks>
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
                return StatusCode(500, "An unexpected error occurred. Please try again later.");
            }
        }

        /// <summary>
        /// Updates an existing satellite.
        /// </summary>
        /// <param name="id">The unique identifier of the satellite to update.</param>
        /// <param name="request">The updated details of the satellite.</param>
        /// <returns>Action result indicating the success or failure of the operation.</returns>
        /// <response code="200">Satellite updated successfully.</response>
        /// <response code="400">Failed to update satellite due to invalid input or other issues.</response>
        /// <response code="500">An unexpected error occurred while updating the satellite.</response>
        /// <remarks>
        /// This endpoint updates an existing satellite using the provided unique identifier <paramref name="id"/> and the updated details in <paramref name="request"/>.
        /// If the satellite is updated successfully, a 200 OK response is returned.
        /// If the update fails due to invalid input or other issues, a 400 BadRequest is returned.
        /// Any internal errors that occur during the update process will result in a 500 InternalServerError.
        /// </remarks>
        [HttpPut("update-satellite/{id}")]
        public async Task<ActionResult> UpdateSatellite(Guid id, [FromBody] SatelliteDto request)
        {
            if (id == Guid.Empty)
            {
                return BadRequest("Invalid satellite ID.");
            }

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
                return StatusCode(500, "An unexpected error occurred. Please try again later.");
            }
        }

        /// <summary>
        /// Deletes an existing satellite.
        /// </summary>
        /// <param name="id">The unique identifier of the satellite to delete.</param>
        /// <returns>Action result indicating the success or failure of the operation.</returns>
        /// <response code="200">Satellite deleted successfully.</response>
        /// <response code="400">Failed to delete satellite due to invalid input or other issues.</response>
        /// <response code="500">An unexpected error occurred while deleting the satellite.</response>
        /// <remarks>
        /// This endpoint deletes a satellite identified by the provided unique identifier <paramref name="id"/>.
        /// If the satellite is successfully deleted, a 200 OK response is returned.
        /// If the deletion fails due to invalid input or other issues, a 400 BadRequest is returned.
        /// Any internal errors that occur during the deletion process will result in a 500 InternalServerError.
        /// </remarks>
        [HttpDelete("delete-satellite/{id}")]
        public async Task<ActionResult> DeleteSatellite(Guid id)
        {
            if (id == Guid.Empty)
            {
                return BadRequest("Invalid satellite ID.");
            }

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
                return StatusCode(500, "An unexpected error occurred. Please try again later.");
            }
        }

        /// <summary>
        /// Retrieves all users in the system.
        /// </summary>
        /// <returns>A list of users in the system.</returns>
        /// <response code="200">Successfully retrieved the list of users.</response>
        /// <response code="500">An unexpected error occurred while retrieving the users.</response>
        /// <remarks>
        /// This endpoint retrieves all users from the system. If the retrieval is successful, a 200 OK response is returned with a list of users.
        /// If an unexpected error occurs during the retrieval process, a 500 InternalServerError response is returned.
        /// </remarks>
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
                return StatusCode(500, "An unexpected error occurred. Please try again later.");
            }
        }

        /// <summary>
        /// Deletes an existing user from the system.
        /// </summary>
        /// <param name="id">The unique identifier of the user to delete.</param>
        /// <returns>Action result indicating the success or failure of the operation.</returns>
        /// <response code="200">Successfully deleted the user.</response>
        /// <response code="400">Invalid user ID provided.</response>
        /// <response code="404">User not found in the system.</response>
        /// <response code="500">An unexpected error occurred while deleting the user.</response>
        /// <remarks>
        /// This endpoint deletes the user from the system using their unique ID. 
        /// If the ID is invalid or the user is not found, an appropriate error response is returned. 
        /// If the deletion is successful, a 200 OK response is returned.
        /// </remarks>
        [HttpDelete("delete-user/{id}")]
        public async Task<ActionResult> DeleteUser(Guid id)
        {
            if (id == Guid.Empty)
            {
                return BadRequest("Invalid user ID.");
            }

            try
            {
                var result = await _authService.DeleteUserAsync(id);
                if (result)
                {
                    return Ok("User deleted successfully");
                }
                else
                {
                    return NotFound("User not found");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An unexpected error occurred. Please try again later.");
            }
        }
    }
}
