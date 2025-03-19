
using System.Text.Json.Serialization;

namespace DevHouse.Models {
    public class Developer {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public Team Team { get; set; }
        public Role Role { get; set; }
    }
} 