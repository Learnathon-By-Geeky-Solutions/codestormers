﻿namespace CosmoVerse.Application.DTOs
{
    /// <summary>
    /// Data Transfer Object for Satellite information.
    /// </summary>
    public class SatelliteInfoDto
    {
        /// <summary>
        /// Unique identifier for the satellite.
        /// </summary>
        public Guid Id { get; set; } = Guid.Empty;

        /// <summary>
        /// Name of the satellite.
        /// </summary>
        public required string Name { get; set; }

        /// <summary>
        /// Size of the satellite.
        /// </summary>
        public double Size { get; set; }

        /// <summary>
        /// Distance from the planet it orbits.
        /// </summary>
        public double DistanceFromPlanet { get; set; }

        /// <summary>
        /// Orbital period of the satellite around the planet.
        /// </summary>
        public double OrbitalPeriod { get; set; }

        /// <summary>
        /// Description of the satellite.
        /// </summary>
        public string Description { get; set; } = string.Empty;
    }
}
