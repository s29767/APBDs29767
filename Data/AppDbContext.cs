﻿using APBD_EF_CodeFirst_Example.Models;
using Microsoft.EntityFrameworkCore;

namespace APBD_EF_CodeFirst_Example.Data;

public class AppDbContext : DbContext
{
    // Any class representing a table should be added here as a DbSet to be visible for migrations system
    public DbSet<Student> Students { get; set; }
    public DbSet<Group> Groups { get; set; }
    public DbSet<GroupAssignment> GroupAssignments { get; set; }
    
    public DbSet<Patient> Patients { get; set; }
    public DbSet<Doctor> Doctors { get; set; }
    public DbSet<Medicament> Medicaments { get; set; }
    public DbSet<Prescription> Prescriptions { get; set; }
    public DbSet<PrescriptionMedicament> PrescriptionMedicaments { get; set; }
    
    
    public AppDbContext(DbContextOptions options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        var students = new List<Student>
        {
            new()
            {
                Id = 1,
                FirstName = "John",
                LastName = "Doe",
                Age = 25,
                EntranceExamScore = 70.2f,
            },
            new()
            {
                Id = 2,
                FirstName = "Jane",
                LastName = "Doe",
                Age = 20,
            }
        };

        var groups = new List<Group>
        {
            new ()
            {
                Id = 1,
                Name = "16c"
            }
        };

        var groupAssignments = new List<GroupAssignment>
        {
            new()
            {
                GroupId = 1,
                StudentId = 1,
            }
        };
        
        modelBuilder.Entity<Student>().HasData(students);
        modelBuilder.Entity<Group>().HasData(groups);
        modelBuilder.Entity<GroupAssignment>().HasData(groupAssignments);
        
        modelBuilder.Entity<Doctor>().HasData(
            new Doctor
            {
                IdDoctor = 1,
                FirstName = "Jan",
                LastName = "Kowalski",
                Email = "jan.kowalski@example.com"
            }
        );
        
        modelBuilder.Entity<Medicament>().HasData(
            new Medicament
            {
                IdMedicament = 1,
                Name = "Apap",
                Description = "Lek przeciwbólowy",
                Type = "Przeciwbólowy"
            }
        );

        
        
    }
}