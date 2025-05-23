using System.ComponentModel.DataAnnotations;

namespace APBD_EF_CodeFirst_Example.DTOs;

public class PatientDto
{
    public int? IdPatient { get; set; }
    
    [MaxLength(100)]
    [Required]
    public string FirstName { get; set; } = null!;
    
    [MaxLength(100)]
    [Required]
    public string LastName { get; set; } = null!;
    
    [Required]
    public DateTime BirthDate { get; set; }
}