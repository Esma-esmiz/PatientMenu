using System;
using System.ComponentModel.DataAnnotations;

public class Patient
{
    [Key]
    public Guid Id { get; set; }

    [Required]
    public Guid TenantId { get; set; }

    
    [Required]
    public string Name { get; set; } = null!;

    public string? DietaryRestrictionCode { get; set; }

    public DateTime CreatedAt { get; set; }  // automatically set
    public DateTime UpdatedAt { get; set; }  // automatically updated
}
