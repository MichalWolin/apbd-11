using cw10.Models;

namespace cw10.DTOs;

public class PatientInfoDto
{
    public int IdPatient { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public DateTime BirthDate { get; set; }
    public List<PrescriptionInfoDto> Prescriptions { get; set; }
}