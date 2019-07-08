using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace cms.Models
{
  public class Person
  {
    [Key]
    public int PersonId { get; set; }

    //Website/Identity User ID
    // [Required]
    public string CreatorId { get; set; }

    [DataType(DataType.Text),Required,MaxLength(10),MinLength(5)]
    public string Name { get; set; }
    [DataType(DataType.MultilineText),MaxLength(250),MinLength(6)]
    public string Description { get; set; }

    [Required]
    public int CityId { get; set; }
    public City City { get; set; }

    [DataType(DataType.PhoneNumber)]
    public string Phone { get; set; }
    [DataType(DataType.ImageUrl)]
    public string Portrait { get; set; }
    [DataType(DataType.EmailAddress)]
    [Required]
    public string Email { get; set; }
    [Required]
    public PersonStatus Status { get; set; } = PersonStatus.Visible;
  }

  public enum PersonStatus
  {
    VIP,
    Visible,
    Hidden
  }
}