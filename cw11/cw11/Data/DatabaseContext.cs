using cw10.Models;
using Microsoft.EntityFrameworkCore;

namespace cw10.Data;

public class DatabaseContext : DbContext
{
    protected DatabaseContext()
    {
    }

    public DatabaseContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<Doctor> Doctors { get; set; }
    public DbSet<Medicament> Medicaments { get; set; }
    public DbSet<Patient> Patients { get; set; }
    public DbSet<Prescription> Prescriptions { get; set; }
    public DbSet<PrescriptionMedicament> PrescriptionMedicaments { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Doctor>().HasData(new List<Doctor>
        {
            new Doctor
            {
                IdDoctor = 1,
                FirstName = "Gregory",
                LastName = "House",
                Email = "drhouse@hospital.com"
            },
            new Doctor
            {
                IdDoctor = 2,
                FirstName = "Zbigniew",
                LastName = "Religa",
                Email = "zbigniew@religa.pl"
            }
        });

        modelBuilder.Entity<Patient>().HasData(new List<Patient>
        {
            new Patient
            {
                IdPatient = 1,
                FirstName = "Jan",
                LastName = "Nowak",
                BirthDate = DateTime.Parse("1990-01-01")
            },
            new Patient
            {
                IdPatient = 2,
                FirstName = "Anna",
                LastName = "Kowalska",
                BirthDate = DateTime.Parse("1995-01-01")
            }
        });

        modelBuilder.Entity<Prescription>().HasData(new List<Prescription>
        {
            new Prescription
            {
                IdPrescription = 1,
                Date = DateTime.Parse("2021-01-01"),
                DueDate = DateTime.Parse("2021-01-15"),
                IdPatient = 1,
                IdDoctor = 1
            },
            new Prescription
            {
                IdPrescription = 2,
                Date = DateTime.Parse("2021-01-02"),
                DueDate = DateTime.Parse("2021-01-16"),
                IdPatient = 2,
                IdDoctor = 2
            }
        });

        modelBuilder.Entity<Medicament>().HasData(new List<Medicament>
        {
            new Medicament
            {
                IdMedicament = 1,
                Name = "Paracetamol",
                Description = "Painkiller"
            },
            new Medicament
            {
                IdMedicament = 2,
                Name = "Ibuprofen",
                Description = "Anti-inflammatory"
            }
        });

        modelBuilder.Entity<PrescriptionMedicament>().HasData(new List<PrescriptionMedicament>
        {
            new PrescriptionMedicament
            {
                IdPrescription = 1,
                IdMedicament = 1,
                Dose = 2,
                Details = "Twice a day"
            },
            new PrescriptionMedicament
            {
                IdPrescription = 2,
                IdMedicament = 2,
                Dose = 1,
                Details = "Once a day"
            }
        });
    }
}