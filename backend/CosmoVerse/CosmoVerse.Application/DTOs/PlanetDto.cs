namespace CosmoVerse.Application.DTOs
{
    /// <summary>
    /// Data Transfer Object for Planet information.
    /// </summary>
    public class PlanetDto
    {
        /// <summary>
        /// Name of the planet.
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Introductory information about the planet.
        ///</summary>
        public string Introduction { get; set; } = string.Empty;

        /// <summary>
        /// Namesake of the planet, if any.
        /// </summary>
        public string Namesake { get; set; } = string.Empty;

        /// <summary>
        /// Description of the planet's history about life.
        /// </summary>
        public string PotentialForLife { get; set; } = string.Empty;

        /// <summary>
        /// Size and distance information of the planet.
        /// </summary>
        public string SizeAndDistance { get; set; } = string.Empty;

        /// <summary>
        /// Information about the planet's orbit and rotation.
        /// </summary>
        public string OrbitAndRotation { get; set; } = string.Empty;

        /// <summary>
        /// Information about satellites and moons of the planet.
        /// </summary>
        public string Moons { get; set; } = string.Empty;

        /// <summary>
        /// Information about the planet's rings, if any.
        /// </summary>
        public string Rings { get; set; } = string.Empty;

        /// <summary>
        /// information about the planet's history how it was formed.
        /// </summary>
        public string Formation { get; set; } = string.Empty;

        /// <summary>
        /// Internal structure of the planet.
        /// </summary>
        public string Structure { get; set; } = string.Empty;

        /// <summary>
        /// Details about the planet's surface.
        /// </summary>
        public string Surface { get; set; } = string.Empty;

        /// <summary>
        /// Atmospheric details of the planet.
        /// </summary>
        public string Atmosphere { get; set; } = string.Empty;

        /// <summary>
        /// Magnetic field details of the planet.
        /// </summary>
        public string Magnetosphere { get; set; } = string.Empty;

        /// <summary>
        /// Foreign key of the celestial system to which the planet belongs.
        /// </summary>
        public Guid CelestialSystemId { get; set; } = Guid.Empty;
    }
}
