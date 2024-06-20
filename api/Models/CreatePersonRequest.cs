using System.ComponentModel.DataAnnotations;

namespace StargateAPI.Models
{
    public class CreatePersonRequest
    {
        [Required]
        [MinLength(1)]
        public string Name { get; set; }
    }
}
