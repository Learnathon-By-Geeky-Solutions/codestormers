namespace CosmoVerse.Models
{
    public class PlanetSummary
    {
        public Guid Id { get; set; }
        public required string Name { get; set; }
        public double DistanceFromSun { get; set; }
        public double Diameter { get; set; }
        public double RotationPeriod { get; set; }
        public double OrbitalPeriod { get; set; }
        public required string MediaUrl { get; set; }
    }
}