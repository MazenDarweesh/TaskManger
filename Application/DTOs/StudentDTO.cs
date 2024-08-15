using System.ComponentModel.DataAnnotations;

namespace Application.DTOs
{
    public class StudentDTO
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public DateTime DateOfBirth { get; set; }
    }
}
