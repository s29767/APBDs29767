using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace APBD_EF_CodeFirst_Example.Models;

[Table("Prescription")]
public class Prescription
{
    [Key]
    public int IdPrescription { get; set; }
    
    [Required]
    public DateTime Date { get; set; }
    
    [Required]
    public DateTime DueDate { get; set; }
    
    [Required]
    public int IdPatient { get; set; }
    
    [Required]
    public int IdDoctor { get; set; }
    
    [ForeignKey(nameof(IdPatient))]
    public virtual Patient Patient { get; set; } = null!;
    
    [ForeignKey(nameof(IdDoctor))]
    public virtual Doctor Doctor { get; set; } = null!;
    
    public virtual ICollection<PrescriptionMedicament> PrescriptionMedicaments { get; set; } = null!;
}