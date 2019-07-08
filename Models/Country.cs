using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace cms.Models
{
    public class Country
    {
        [Key]
        public int CountryId { get; set; }

        [Required]
        public string CountryName { get; set; }

        public List<City> Cities { get; set; }
    }
}