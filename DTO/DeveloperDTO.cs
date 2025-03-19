using System.ComponentModel.DataAnnotations;

namespace DevHouse.DTO {
    public class AddDeveloperDTO {
        [Required]
        [MinLength(2)]
        [MaxLength(50)]
        public string FirstName { get; set; }

        [Required]
        [MinLength(2)]
        [MaxLength(50)]
        public string LastName { get; set; }
        
        [Required]
        [Range(1, int.MaxValue)]
        public int TeamId { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        public int RoleId { get; set; }
    }
    public class UpdateDeveloperDTO {
        [Required]
        [Range(1, int.MaxValue)]
        public int Id { get; set; }

        [Required]
        [MinLength(2)]
        [MaxLength(50)]
        public string FirstName { get; set; }

        [Required]
        [MinLength(2)]
        [MaxLength(50)]
        public string LastName { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        public int RoleId { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        public int TeamId { get; set; }
    }
}