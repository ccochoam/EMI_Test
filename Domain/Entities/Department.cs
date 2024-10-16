using System.Text.Json.Serialization;

namespace Domain.Entities
{
    public class Department
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<Employee> Employees { get; set; }
        [JsonIgnore]
        public ICollection<Project> Projects { get; set; } = new List<Project>();
    }
}
