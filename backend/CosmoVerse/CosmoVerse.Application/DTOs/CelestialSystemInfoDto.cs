namespace CosmoVerse.Application.DTOs
{
    /// <summary>
    /// Class representing a Celestial System Info Data Transfer Object.
    /// </summary>
    public class CelestialSystemInfoDto
    {
        /// <summary>
        /// Unique identifier for the celestial system.
        /// </summary>
        public Guid Id { get; set; } = Guid.Empty;

        /// <summary>
        /// Name of the celestial system.
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Type of the celestial system.
        /// </summary>
        public string Type { get; set; } = string.Empty;

        /// <summary>
        /// Description of the celestial system.
        /// </summary>
        public string Description { get; set; } = string.Empty;

        /// <summary>
        /// Structure in how the celestial system is organized.
        /// </summary>
        public string structure { get; set; } = string.Empty;
    }
}
