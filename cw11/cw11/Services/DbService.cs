using cw10.Data;
using cw10.DTOs;
using cw10.Models;
using Microsoft.EntityFrameworkCore;

namespace cw10.Services;

public class DbService : IDbService
{
    private readonly DatabaseContext _context;

    public DbService(DatabaseContext context)
    {
        _context = context;
    }

    public async Task<bool> DoesPatientExist(int id)
    {
        var result = await _context.Patients.AnyAsync(p => p.IdPatient.Equals(id));

        return result;
    }

    public async Task<int> AddNewPatient(PatientDto patientDto)
    {
        var patient = new Patient
        {
            FirstName = patientDto.FirstName,
            LastName = patientDto.LastName,
            BirthDate = patientDto.BirthDate
        };

        await _context.Patients.AddAsync(patient);

        await _context.SaveChangesAsync();

        return patient.IdPatient;
    }

    public async Task<bool> DoesMedicationExist(int id)
    {
        var result = await _context.Medicaments.AnyAsync(m => m.IdMedicament.Equals(id));

        return result;
    }

    public async Task<int> AddNewPrescription(DateTime date, DateTime dueDate, int idPatient, int idDoctor)
    {
        var prescription = new Prescription
        {
            Date = date,
            DueDate = dueDate,
            IdPatient = idPatient,
            IdDoctor = idDoctor
        };

        await _context.Prescriptions.AddAsync(prescription);

        await _context.SaveChangesAsync();

        return prescription.IdPrescription;
    }

    public async Task AddNewPrescriptionMedicament(int idMedicament, int idPrescription, int dose, string details)
    {
        await _context.PrescriptionMedicaments.AddAsync(new PrescriptionMedicament
        {
            IdMedicament = idMedicament,
            IdPrescription = idPrescription,
            Dose = dose,
            Details = details
        });

        await _context.SaveChangesAsync();
    }

    public async Task<PatientInfoDto> GetPatientInfo(int id)
    {
        var patient = await _context.Patients
            .Include(p => p.Prescriptions)
            .ThenInclude(p => p.PrescriptionMedicaments)
            .ThenInclude(pm => pm.Medicament)
            .FirstOrDefaultAsync(p => p.IdPatient.Equals(id));

        var patientInfoDto = new PatientInfoDto
        {
            IdPatient = patient.IdPatient,
            FirstName = patient.FirstName,
            LastName = patient.LastName,
            BirthDate = patient.BirthDate,
            Prescriptions = patient.Prescriptions.Select(p => new PrescriptionInfoDto
            {
                IdPrescription = p.IdPrescription,
                Date = p.Date,
                DueDate = p.DueDate,
                Medicaments = p.PrescriptionMedicaments.Select(pm => new MedicamentInfoDto
                {
                    IdMedicament = pm.IdMedicament,
                    Name = pm.Medicament.Name,
                    Dose = pm.Dose,
                    Description = pm.Medicament.Description
                }).ToList(),
                Doctor = _context.Doctors.Where(d => d.IdDoctor.Equals(p.IdDoctor))
                    .Select(d => new DoctorInfoDto
                    {
                        IdDoctor = d.IdDoctor,
                        FirstName = d.FirstName,
                        LastName = d.LastName,
                        Email = d.Email
                    }).FirstOrDefaultAsync()
            }).ToList()
        };

        return patientInfoDto;
    }
}