namespace CosmoVerse.Application.DTOs
{
    /// <summary>
    /// Satellite Data Transfer Object.
    /// </summary>
    public class SatelliteDto
    {
        public required string Name { get; set; }

        public double Size { get; set; }
        public double DistanceFromPlanet { get; set; }
        public double OrbitalPeriod { get; set; }
        public string Description { get; set; } = string.Empty;
        public Guid PlanetId { get; set; } = Guid.Empty;
    }
}
