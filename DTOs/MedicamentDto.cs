using System.ComponentModel.DataAnnotations;

namespace APBD_EF_CodeFirst_Example.DTOs;

public class MedicamentDto
{
    [Required]
    public int IdMedicament { get; set; }
    
    public int? Dose { get; set; }
    
    [MaxLength(100)]
    [Required]
    public string Description { get; set; } = null!;
}