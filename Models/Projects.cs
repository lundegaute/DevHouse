namespace DevHouse.Models {
    public class Project {
        public int Id { get; set; }
        public string Name { get; set; }
        public Team Team { get; set; }
        public ProjectType ProjectType { get; set; }
    }
}