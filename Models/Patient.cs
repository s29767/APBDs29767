using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace APBD_EF_CodeFirst_Example.Models;

[Table("Patient")]
public class Patient
{
    [Key]
    public int IdPatient { get; set; }
    
    [MaxLength(100)]
    [Required]
    public string FirstName { get; set; } = null!;
    
    [MaxLength(100)]
    [Required]
    public string LastName { get; set; } = null!;
    
    [Required]
    public DateTime BirthDate { get; set; }
    
    public virtual ICollection<Prescription> Prescriptions { get; set; } = null!;
}