using System.ComponentModel.DataAnnotations;


    public class TenantDto
    {
        [Required]
        public string Name { get; set; } = null!;
    }
