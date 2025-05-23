using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace APBD_EF_CodeFirst_Example.Models;

[Table("Medicament")]
public class Medicament
{
    [Key]
    public int IdMedicament { get; set; }
    
    [MaxLength(100)]
    [Required]
    public string Name { get; set; } = null!;
    
    [MaxLength(100)]
    [Required]
    public string Description { get; set; } = null!;
    
    [MaxLength(100)]
    [Required]
    public string Type { get; set; } = null!;
    
    public virtual ICollection<PrescriptionMedicament> PrescriptionMedicaments { get; set; } = null!;
}