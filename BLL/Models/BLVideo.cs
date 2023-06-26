using DAL.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Models
{
    public class BLVideo
    {
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public string? Name { get; set; } = null!;
        public string? Description { get; set; }
        public int GenreId { get; set; }
        [Range(1, 2145600, ErrorMessage = "Length must be between 1 and 2145600 seconds")]
        public int TotalSeconds { get; set; }
        [Url(ErrorMessage = "Invalid URL format")]
        public string? StreamingUrl { get; set; }
        public int ImageId { get; set; }
        public virtual BLGenre Genre { get; set; } = null!;
        public virtual BLImage? Image { get; set; }

        public virtual ICollection<BLVideoTag>? VideoTags { get; set; } = new List<BLVideoTag>();
    }
}
