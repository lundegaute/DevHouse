namespace DevHouse.Models {
    public class Role {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<Developer>? Developers { get; set; } // Adding ? to make it nullable
    }
}
