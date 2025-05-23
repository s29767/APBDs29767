using APBD_EF_CodeFirst_Example.Data;
using APBD_EF_CodeFirst_Example.DTOs;
using APBD_EF_CodeFirst_Example.Exceptions;
using APBD_EF_CodeFirst_Example.Models;
using Microsoft.EntityFrameworkCore;

namespace APBD_EF_CodeFirst_Example.Services;

public interface IDbService
{
    public Task<ICollection<StudentGetDto>> GetStudentsDetailsAsync();
    public Task<StudentGetDto> GetStudentDetailsByIdAsync(int studentId);
    public Task<StudentGetDto> CreateStudentAsync(StudentCreateDto studentData);
    public Task RemoveStudentAsync(int studentId);
    public Task UpdateStudentAsync(int studentId, StudentUpdateDto studentData);
    
    Task<PatientDetailsDto> GetPatientDetailsAsync(int idPatient);
    Task<Prescription> AddPrescriptionAsync(PrescriptionCreateDto prescriptionData);
    
}

public class DbService(AppDbContext data) : IDbService
{
    public async Task<ICollection<StudentGetDto>> GetStudentsDetailsAsync()
    {
        // Get all students from the DB and map them to a DTO
        return await data.Students.Select(st => new StudentGetDto
        {
            Id = st.Id,
            FirstName = st.FirstName,
            LastName = st.LastName,
            Age = st.Age,
            EntranceExamScore = st.EntranceExamScore,
            Groups = st.GroupAssignments.Select(ga => new StudentGetDtoGroup
            {
                Id = ga.GroupId,
                Name = ga.Group.Name,
            }).ToList()
        }).ToListAsync();
    }

    public async Task<StudentGetDto> GetStudentDetailsByIdAsync(int studentId)
    {
        // Try to get and map a student to a DTO
        var result = await data.Students.Select(st => new StudentGetDto
        {
            Id = st.Id,
            FirstName = st.FirstName,
            LastName = st.LastName,
            Age = st.Age,
            EntranceExamScore = st.EntranceExamScore,
            Groups = st.GroupAssignments.Select(ga => new StudentGetDtoGroup
            {
                Id = ga.GroupId,
                Name = ga.Group.Name,
            }).ToList()
        }).FirstOrDefaultAsync(e => e.Id == studentId);

        // If the student does not exist, we have to send a notification about it to the controller.
        return result ?? throw new NotFoundException($"Student with id: {studentId} not found");
    }

    public async Task<StudentGetDto> CreateStudentAsync(StudentCreateDto studentData)
    {
        List<Group> groups = [];
        
        // At first, we have to check if the given groups exist in the DB
        if (studentData.Groups is not null && studentData.Groups.Count != 0)
        {
            foreach (var groupId in studentData.Groups)
            {
                var group = await data.Groups.FirstOrDefaultAsync(g => g.Id == groupId);
                if (group is null)
                {
                    throw new NotFoundException($"Group with id: {groupId} not found");
                }
                groups.Add(group);
            }
        }
        
        // *Approach 1* - add data to each table manually using multiple requests (transaction required)
        // var transaction = await data.Database.BeginTransactionAsync(); // Begin transaction
        // try
        // {
        //     var student = new Student
        //     {
        //         FirstName = studentData.FirstName,
        //         LastName = studentData.LastName,
        //         Age = studentData.Age,
        //         EntranceExamScore = studentData.EntranceExamScore
        //     };
        //     await data.Students.AddAsync(student);
        //     await data.SaveChangesAsync(); // Request 1
        //
        //     var groupAssignments = (studentData.Groups ?? []).Select(groupId => new GroupAssignment
        //     {
        //         GroupId = groupId,
        //         StudentId = student.Id,
        //     });
        //     await data.GroupAssignments.AddRangeAsync(groupAssignments);
        //     await data.SaveChangesAsync(); // Request 2
        //     
        //     await transaction.CommitAsync(); // Commit transaction
        // }
        // catch (Exception)
        // {
        //     await transaction.RollbackAsync(); // Rollback transaction if error occurs
        //     throw;
        // }
        
        // *Approach 2* - add all data via single context access
        // Map a DTO data to the student model.
        var student = new Student
        {
            FirstName = studentData.FirstName,
            LastName = studentData.LastName,
            Age = studentData.Age,
            EntranceExamScore = studentData.EntranceExamScore,
            GroupAssignments = (studentData.Groups ?? []).Select(groupId => new GroupAssignment
            {
                GroupId = groupId,
            }).ToList()
        };

        // Add new student to the db context, and save all changes.
        await data.Students.AddAsync(student);
        await data.SaveChangesAsync();

        
        // Return created records to the controller.
        return new StudentGetDto
        {
            Id = student.Id,
            FirstName = studentData.FirstName,
            LastName = studentData.LastName,
            Age = studentData.Age,
            EntranceExamScore = studentData.EntranceExamScore,
            Groups = groups.Select(group => new StudentGetDtoGroup
            {
                Id = group.Id,
                Name = group.Name,
            }).ToList()
        };
    }

    public async Task RemoveStudentAsync(int studentId)
    {
        // *Approach 1*
        // var student = await data.Students.FirstOrDefaultAsync(st => st.Id == studentId);
        // if (student is null)
        // {
        //     throw new NotFoundException($"Student with id {studentId} not found");
        // }
        //
        // data.Students.Remove(student);
        // await data.SaveChangesAsync();
        
        // *Approach 2* (more efficient)
        // Try to delete a record from a table of students by the specified identifier
        var affectedRows = await data.Students.Where(s => s.Id == studentId).ExecuteDeleteAsync();

        // If there are no changes, it means that, there is no record with this id.
        if (affectedRows == 0)
        {
            throw new NotFoundException($"Student with id: {studentId} not found");
        }
    }

    public async Task UpdateStudentAsync(int studentId, StudentUpdateDto studentData)
    {
        // *Approach 1*
        // var student = await data.Students.FirstOrDefaultAsync(s => s.Id == studentId);
        // if (student is null)
        // {
        //     throw new NotFoundException($"Student with id {studentId} not found");
        // }
        //
        // student.FirstName = studentData.FirstName;
        // student.LastName = studentData.LastName;
        // student.Age = studentData.Age;
        // student.EntranceExamScore = studentData.EntranceExamScore;
        //
        // await data.SaveChangesAsync();

        // *Approach 2*  (more efficient)
        var affectedRows = await data.Students.Where(e => e.Id == studentId).ExecuteUpdateAsync(
            setters => setters
                .SetProperty(e => e.FirstName, studentData.FirstName)
                .SetProperty(e => e.LastName, studentData.LastName)
                .SetProperty(e => e.Age, studentData.Age)
                .SetProperty(e => e.EntranceExamScore, studentData.EntranceExamScore)
        );
        
        // If there are no changes, it means that, there is no record with this id.
        if (affectedRows == 0)
        {
            throw new NotFoundException($"Student with id: {studentId} not found");
        }
    }
    public async Task<PatientDetailsDto> GetPatientDetailsAsync(int idPatient)
    {
        var patient = await data.Patients
            .Include(p => p.Prescriptions)
            .ThenInclude(pr => pr.Doctor)
            .Include(p => p.Prescriptions)
            .ThenInclude(pr => pr.PrescriptionMedicaments)
            .ThenInclude(pm => pm.Medicament)
            .FirstOrDefaultAsync(p => p.IdPatient == idPatient);

        if (patient == null)
        {
            throw new NotFoundException($"Patient with id: {idPatient} not found");
        }

        return new PatientDetailsDto
        {
            IdPatient = patient.IdPatient,
            FirstName = patient.FirstName,
            LastName = patient.LastName,
            BirthDate = patient.BirthDate,
            Prescriptions = patient.Prescriptions
                .OrderBy(p => p.DueDate)
                .Select(p => new PrescriptionDetailsDto
                {
                    IdPrescription = p.IdPrescription,
                    Date = p.Date,
                    DueDate = p.DueDate,
                    Doctor = new DoctorDetailsDto
                    {
                        IdDoctor = p.Doctor.IdDoctor,
                        FirstName = p.Doctor.FirstName,
                        LastName = p.Doctor.LastName
                    },
                    Medicaments = p.PrescriptionMedicaments.Select(pm => new MedicamentDetailsDto
                    {
                        IdMedicament = pm.Medicament.IdMedicament,
                        Name = pm.Medicament.Name,
                        Dose = pm.Dose,
                        Description = pm.Details
                    }).ToList()
                }).ToList()
        };
    }
    public async Task<Prescription> AddPrescriptionAsync(PrescriptionCreateDto prescriptionData)
{
    if (prescriptionData.Medicaments.Count > 10)
    {
        throw new NotFoundException("Prescription can include max 10 medicaments");
    }

    if (prescriptionData.DueDate < prescriptionData.Date)
    {
        throw new NotFoundException("DueDate must be greater or equal to Date");
    }

    // Sprawdzanie czy wszystkie leki istnieją
    var medicamentIds = prescriptionData.Medicaments.Select(m => m.IdMedicament).ToList();
    var existingMedicamentsCount = await data.Medicaments
        .Where(m => medicamentIds.Contains(m.IdMedicament))
        .CountAsync();

    if (existingMedicamentsCount != medicamentIds.Count)
    {
        throw new NotFoundException("One or more medicaments not found");
    }

    // Sprawdzanie czy lekarz istnieje
    var doctorExists = await data.Doctors.AnyAsync(d => d.IdDoctor == prescriptionData.IdDoctor);
    if (!doctorExists)
    {
        throw new NotFoundException($"Doctor with id: {prescriptionData.IdDoctor} not found");
    }

    // Obsługa pacjenta
    Patient patient;
    if (prescriptionData.Patient.IdPatient.HasValue)
    {
        patient = await data.Patients.FirstOrDefaultAsync(p => p.IdPatient == prescriptionData.Patient.IdPatient);
        if (patient == null)
        {
            throw new NotFoundException($"Patient with id: {prescriptionData.Patient.IdPatient} not found");
        }
    }
    else
    {
        patient = new Patient
        {
            FirstName = prescriptionData.Patient.FirstName,
            LastName = prescriptionData.Patient.LastName,
            BirthDate = prescriptionData.Patient.BirthDate
        };
        await data.Patients.AddAsync(patient);
        await data.SaveChangesAsync();
    }

    // Tworzenie recepty
    var prescription = new Prescription
    {
        Date = prescriptionData.Date,
        DueDate = prescriptionData.DueDate,
        IdPatient = patient.IdPatient,
        IdDoctor = prescriptionData.IdDoctor,
        PrescriptionMedicaments = prescriptionData.Medicaments.Select(m => new PrescriptionMedicament
        {
            IdMedicament = m.IdMedicament,
            Dose = m.Dose,
            Details = m.Description
        }).ToList()
    };

    await data.Prescriptions.AddAsync(prescription);
    await data.SaveChangesAsync();

    return prescription;
}
    
}