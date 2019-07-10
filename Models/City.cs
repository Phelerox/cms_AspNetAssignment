using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace cms.Models
{
    public class City
    {
        [Key]
        public int CityId { get; set; }

        [Required]
        public string CityName { get; set; }

        [Required]
        public int CountryId { get; set; }

        [Required]
        public virtual Country Country {get; set;}

        public override string ToString() {
            return $"{CityName}, {Country.CountryName}";
        }
    }
}