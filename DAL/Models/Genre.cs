using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DAL.Models;

public partial class Genre
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Name is required")]
    public string Name { get; set; } = null!;

    [Required(ErrorMessage = "Description is required")]
    public string Description { get; set; }

    public virtual ICollection<Video> Videos { get; set; } = new List<Video>();

    public override string ToString() => $"{Name} {Description}";
}
