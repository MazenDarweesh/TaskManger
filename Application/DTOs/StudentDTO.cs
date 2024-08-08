using System.ComponentModel.DataAnnotations;

namespace Application.DTOs
{
    public class StudentDTO
    {
        public string Id { get; set; }
        
        [Required(ErrorMessage = "Name is required.")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "Name must be between 3 and 100 characters.")]
        public string Name { get; set; }
        
        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        public string Email { get; set; }
        public DateTime DateOfBirth { get; set; }
    }
}
