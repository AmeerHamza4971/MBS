using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MBS.Models;

public class TodoTask
{
    [Key]
    public long Id { get; set; }

    [Required]
    public string Name { get; set; }

    public string Description { get; set; }

    [Required]
    public string Type { get; set; }
    [Required]
    public string Status { get; set; } = "Pending"; 

    public string Tags { get; set; } 

    public DateTime? Deadline { get; set; }

    public DateTime CreatedDate { get; set; }
    public DateTime? UpdatedDate { get; set; }

    public bool IsDeleted { get; set; } = false;

    [NotMapped]
    public List<string> TagList
    {
        get => Tags?.Split(',', StringSplitOptions.RemoveEmptyEntries).ToList() ?? new List<string>();
        set => Tags = value != null ? string.Join(",", value) : null;
    }
}
