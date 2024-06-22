using cw10.DTOs;

namespace cw10.Services;

public interface IDbService
{
    Task<bool> DoesPatientExist(int id);
    Task<int> AddNewPatient(PatientDto patientDto);
    Task<bool> DoesMedicationExist(int id);
    Task<int> AddNewPrescription(DateTime date, DateTime dueDate, int idPatient, int idDoctor);
    Task AddNewPrescriptionMedicament(int idMedicament, int idPrescription, int dose, string details);
    Task<PatientInfoDto> GetPatientInfo(int id);
    Task<bool> DoesUserExist(string login);
    Task AddNewUser(string login, string password);
}