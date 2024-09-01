using Domain.Models;

namespace Domain.Entities
{
    public class Student : BaseEntity
    {
        public Ulid Id { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }
        public DateTime DateOfBirth { get; set; }

        // Navigation property for the related tasks
        public ICollection<TaskDomain> Tasks { get; set; } = new List<TaskDomain>();
    }
}
