using System.ComponentModel.DataAnnotations;
namespace DevHouse.DTO {
    public class AddTeamDTO {
        [Required]
        public string Name { get; set; }
    }
    public class UpdateTeamDTO {
        [Range(1, int.MaxValue)]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
    }
}