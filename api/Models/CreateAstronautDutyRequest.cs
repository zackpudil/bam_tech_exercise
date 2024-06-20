using System.ComponentModel.DataAnnotations;

namespace StargateAPI.Models
{
    public class CreateAstronautDutyRequest
    {
        [Required]
        public required string Name { get; set; }

        [Required]
        public required string Rank { get; set; }

        [Required]
        public required string DutyTitle { get; set; }

        [Required]
        public DateTime DutyStartDate { get; set; }
    }
}
