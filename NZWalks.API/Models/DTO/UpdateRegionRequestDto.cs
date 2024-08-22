using System.ComponentModel.DataAnnotations;

namespace NZWalks.API.Models.DTO
{
    public class UpdateRegionRequestDto
    {
        [Required]
        [MinLength(3, ErrorMessage ="code has to be a min 3 characters")]
        [MaxLength(3, ErrorMessage ="code has to be a max 100 characters")]
        public string Code{ get; set; }

        [Required]
        [MaxLength(100, ErrorMessage ="has to be max 100 chars")]
        public string Name { get; set; }
        public string? RegionImageUrl { get; set; }
    }
}
