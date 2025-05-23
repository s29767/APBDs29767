namespace APBD_EF_CodeFirst_Example.DTOs;

public class DoctorDetailsDto
{
    public int IdDoctor { get; set; }
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
}