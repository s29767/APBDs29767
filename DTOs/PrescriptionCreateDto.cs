using System.ComponentModel.DataAnnotations;

namespace APBD_EF_CodeFirst_Example.DTOs;

public class PrescriptionCreateDto
{
    [Required]
    public PatientDto Patient { get; set; } = null!;
    
    [Required]
    [MaxLength(10)]
    public ICollection<MedicamentDto> Medicaments { get; set; } = null!;
    
    [Required]
    public DateTime Date { get; set; }
    
    [Required]
    public DateTime DueDate { get; set; }
    
    [Required]
    public int IdDoctor { get; set; }
}