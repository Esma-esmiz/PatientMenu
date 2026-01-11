using System.ComponentModel.DataAnnotations;
public class NewPatientDto
{

    [Required]
    public string Name { get; set; } = null!;

    public string? DietaryRestrictionCode { get; set; }
}