﻿using Domain.Entities;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
namespace Domain.Models;

public class TaskDomain
{
    public Ulid Id { get; set; }
    public required string Title { get; set; }
    public required string Description { get; set; }
    public bool IsCompleted { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public DateTime TerminationDate { get; set; } // New property

    // Foreign key to the Student entity
    public Ulid StudentId { get; set; }
    // Navigation property allows you to navigate from a task to the related student
    public Student Student { get; set; }
}

