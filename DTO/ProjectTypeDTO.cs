using System.ComponentModel.DataAnnotations;

namespace DevHouse.DTO {
    public class AddProjectTypeDTO {
        [Required]
        [MinLength(2)]
        [MaxLength(50)]
        public string Name { get; set; }
    }

    public class UpdateProjectTypeDTO {
        [Required]
        [Range(1, int.MaxValue)]
        public int Id { get; set; }

        [Required]
        [MinLength(2)]
        [MaxLength(50)]
        public string Name { get; set; }
    }
}