namespace CosmoVerse.Application.DTOs
{
    /// <summary>
    /// Image details Data Transfer Object.
    /// </summary>
    public class ImageDto
    {
        /// <summary>
        /// Image Url
        ///</summary>
        public string ImageUrl { get; set; } = string.Empty;

        /// <summary>
        /// Public Id of the image 
        /// </summary>
        public string PublicId { get; set; } = string.Empty;

        /// <summary>
        /// Created at date of the image
        /// </summary>
        public DateTime CreatedAt { get; set; }
    }
}
