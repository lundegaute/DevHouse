namespace DevHouse.Models {
    public class Team {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<Project>? Projects { get; set; } // Adding ? to make it nullable
        public ICollection<Developer>? Developers { get; set; }
    }
}