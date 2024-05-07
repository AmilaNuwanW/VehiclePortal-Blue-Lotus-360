using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VehiclePortal.Web.Models.Entities
{
    public class Vehicle
    {
        public Guid Id { get; set; }

        [Required]
        public string Make { get; set; }

        [Required]
        public string Model { get; set; }


        [Required]
        [RegularExpression(@"^\d{4}$", ErrorMessage = "Year must be a 4-digit number.")]
        public string Year { get; set; }

        [Required]
        public string Color { get; set; }

        [Required] 
        public string VIN { get; set; }

        [Required]
        [RegularExpression(@"^[A-Z]{3}\d{4}$", ErrorMessage = "License plate must be in the format ABC1234.")]
        public string LicensePlate { get; set; }

        [NotMapped]
        public IFormFile ImageFile { get; set; }

        public string ImageBase64 { get; set; }
    }
}
