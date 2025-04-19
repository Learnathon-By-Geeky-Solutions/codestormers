namespace CosmoVerse.Application.DTOs
{
    /// <summary>
    /// Data Transfer Object for Celestial System.
    /// </summary>
    public class CelestialSystemDto
    {
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
        public string Structure { get; set; } = string.Empty;
    }
}
