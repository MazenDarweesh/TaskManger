using System.ComponentModel.DataAnnotations;

namespace Application.DTOs
{
    public class TaskDomainDTO
    {

        public string Id { get; set; }
        [Required(ErrorMessage = "Title is required.")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "Title must be between 3 and 100 characters.")]
        public string Title { get; set; }

        [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters.")]
        public string Description { get; set; }

        public bool IsCompleted { get; set; }

        [Required(ErrorMessage = "Student name is required.")]
        public string StudentName { get; set; }
    }
}
