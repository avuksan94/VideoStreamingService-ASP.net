using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DAL.Models;

public partial class Video
{
    public int Id { get; set; }

    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
    public DateTime CreatedAt { get; set; }

    [Required(ErrorMessage = "Name is required")]
    public string? Name { get; set; } = null!;

    [Required(ErrorMessage = "Description is required")]
    public string? Description { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "GenreId must be a positive number(integer)")]
    public int GenreId { get; set; }

    [Range(1, 2145600, ErrorMessage = "Length must be between 1 and 2145600 seconds")]
    public int TotalSeconds { get; set; }

    [Url(ErrorMessage = "Invalid URL format")]
    public string? StreamingUrl { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "ImageId must be a positive number(integer)")]
    public int ImageId { get; set; }

    public virtual Genre Genre { get; set; } = null!;
    public virtual Image? Image { get; set; }

    public virtual ICollection<VideoTag> VideoTags { get; set; } = new List<VideoTag>();
}
