using System.ComponentModel.DataAnnotations;

namespace StargateAPI.Models
{
    public class CreatePersonRequest
    {
        [Required]
        public required string Name { get; set; }
    }
}
