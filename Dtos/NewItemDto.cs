using System.ComponentModel.DataAnnotations;

public class NewItemDto
{

    [Required]
    public string Name { get; set; } = null!;

    public string? Category { get; set; }

    public bool IsGlutenFree { get; set; }

    public bool IsSugarFree { get; set; }

    public bool IsHeartHealthy { get; set; }
}