using System.ComponentModel.DataAnnotations;

namespace Application.DTOs
{
    public class TaskDomainDTO
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public bool IsCompleted { get; set; }
        public string StudentId { get; set; }
        //public DateTime TerminationDate { get; set; } 
    }
}
