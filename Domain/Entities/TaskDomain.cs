﻿using Domain.Entities;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
namespace Domain.Models;

public class TaskDomain
{
    //prevent a property from being serialized into JSON.
    [JsonIgnore]
    public Ulid Id { get; set; }

    [Required]
    [StringLength(100)]
    public required string Title { get; set; }

    [StringLength(500)]
    public required string Description { get; set; }
    public bool IsCompleted { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    // Foreign key to the Student entity
    public Ulid StudentId { get; set; }

    // Navigation property to the related student
    public Student Student { get; set; }
}

