using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CosmoVerse.Models;
using CosmoVerse.Services;
using CosmoVerse.Application.Services;
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

        
    }
}