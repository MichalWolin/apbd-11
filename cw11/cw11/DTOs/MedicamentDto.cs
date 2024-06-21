using System.ComponentModel.DataAnnotations;

namespace cw10.DTOs;

public class MedicamentDto
{
    [Required]
    public int IdMedicament { get; set; }
    [Required]
    public int Dose { get; set; }
    [Required]
    [MaxLength(100)]
    public string Details { get; set; }
}