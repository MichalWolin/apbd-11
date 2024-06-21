using cw10.Models;

namespace cw10.DTOs;

public class PrescriptionInfoDto
{
    public int IdPrescription { get; set; }
    public DateTime Date { get; set; }
    public DateTime DueDate { get; set; }
    public List<MedicamentInfoDto> Medicaments { get; set; }
    public Task<DoctorInfoDto?> Doctor { get; set; }
}