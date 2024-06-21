using System.ComponentModel.DataAnnotations;
using cw10.Models;

namespace cw10.DTOs;

public class NewPrescriptionDto
{
    [Required]
    public PatientDto Patient { get; set; }
    [Required]
    public List<MedicamentDto> Medicaments { get; set; }
    [Required]
    public DateTime Date { get; set; }
    [Required]
    public DateTime DueDate { get; set; }
    [Required]
    public int IdDoctor { get; set; }
}