using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DAL.Models;

public partial class Tag
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Name is required")]
    public string Name { get; set; } = null!;

    public virtual ICollection<VideoTag> VideoTags { get; set; } = new List<VideoTag>();

    public override string ToString() => $"{Name}";
}
