using System;
using System.ComponentModel.DataAnnotations;

public class MenuItem
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();

    [Required]
    public Guid TenantId { get; set; }

    [Required]
    public string Name { get; set; } = null!;

    public string? Category { get; set; }

    public bool IsGlutenFree { get; set; }

    public bool IsSugarFree { get; set; }

    public bool IsHeartHealthy { get; set; }

    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}
