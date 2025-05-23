namespace APBD_EF_CodeFirst_Example.DTOs;

public class PatientDetailsDto
{
    public int IdPatient { get; set; }
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public DateTime BirthDate { get; set; }
    public ICollection<PrescriptionDetailsDto> Prescriptions { get; set; } = null!;
}