using cw10.DTOs;
using cw11.Models;
using Microsoft.AspNetCore.Identity.Data;
using RegisterRequest = cw11.Models.RegisterRequest;

namespace cw10.Services;

public interface IDbService
{
    Task<bool> DoesPatientExist(int id);
    Task<int> AddNewPatient(PatientDto patientDto);
    Task<bool> DoesMedicationExist(int id);
    Task<int> AddNewPrescription(DateTime date, DateTime dueDate, int idPatient, int idDoctor);
    Task AddNewPrescriptionMedicament(int idMedicament, int idPrescription, int dose, string details);
    Task<PatientInfoDto> GetPatientInfo(int id);
    Task AddNewUser(RegisterRequest model);
    Task<User> GetUser(string login);
    Task<User> GetUser(RefreshTokenRequest refreshToken);
    Task RefreshToken(string login);
}