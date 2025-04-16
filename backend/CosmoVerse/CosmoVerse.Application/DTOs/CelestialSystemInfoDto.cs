namespace CosmoVerse.Application.DTOs
{
    public class CelestialSystemInfoDto
    {
        public Guid Id { get; set; } = Guid.Empty;
        public string Name { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string structure { get; set; } = string.Empty;
    }
}
