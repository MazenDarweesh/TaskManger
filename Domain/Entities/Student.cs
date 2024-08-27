using Domain.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Student
    {
        public Ulid Id { get; set; }
        
        [Required]
        [StringLength(100)]
        public string Name { get; set; }
       
        [Required]
        [StringLength(100)]
        [EmailAddress]
        public string Email { get; set; }
        public DateTime DateOfBirth { get; set; }

        // Navigation property for the related tasks
        public ICollection<TaskDomain> Tasks { get; set; } = new List<TaskDomain>();
    }
}
