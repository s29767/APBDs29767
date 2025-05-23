namespace APBD_EF_CodeFirst_Example.DTOs;

public class PrescriptionDetailsDto
{
    public int IdPrescription { get; set; }
    public DateTime Date { get; set; }
    public DateTime DueDate { get; set; }
    public ICollection<MedicamentDetailsDto> Medicaments { get; set; } = null!;
    public DoctorDetailsDto Doctor { get; set; } = null!;
}