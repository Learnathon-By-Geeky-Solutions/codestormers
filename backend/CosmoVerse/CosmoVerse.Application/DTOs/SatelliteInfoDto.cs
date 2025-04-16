namespace CosmoVerse.Application.DTOs
{
    public class SatelliteInfoDto
    {
        public Guid Id { get; set; } = Guid.Empty;
        public required string Name { get; set; }

        public double Size { get; set; }
        public double DistanceFromPlanet { get; set; }
        public double OrbitalPeriod { get; set; }
        public string Description { get; set; } = string.Empty;
    }
}
