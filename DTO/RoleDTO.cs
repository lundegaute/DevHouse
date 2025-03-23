using System.ComponentModel.DataAnnotations;

namespace DevHouse.DTO {
    public class AddRoleDTO {
        [Required]
        [MinLength(2)]
        [MaxLength(50)]
        public string Name { get; set; } = string.Empty;
    }
    public class UpdateRoleDTO {
        [Required]
        [Range(1, int.MaxValue)]
        public int Id { get; set; }
        [Required]
        [MinLength(2)]
        [MaxLength(50)]
        public string Name { get; set; } = string.Empty;
    }
}
