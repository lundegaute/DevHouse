using System.ComponentModel.DataAnnotations;

namespace DevHouse.DTO {
    public class AddProjectDTO {
        [Required]
        [MinLength(2)]
        [MaxLength(50)]
        public string Name { get; set; }
        [Required]
        [Range(1, int.MaxValue)]
        public int TeamId { get; set; }
        [Required]
        [Range(1, int.MaxValue)]
        public int ProjectTypeId { get; set; }
    }
    public class UpdateProjectDTO {

        [Required]
        [Range(1, int.MaxValue)]
        public int Id { get; set; }
        [Required]
        [MinLength(2)]
        [MaxLength(50)]
        public string Name { get; set; }
        [Required]
        [Range(1, int.MaxValue)]
        public int TeamId { get; set; }
        [Required]
        [Range(1, int.MaxValue)]
        public int ProjectTypeId { get; set; }
    }
}