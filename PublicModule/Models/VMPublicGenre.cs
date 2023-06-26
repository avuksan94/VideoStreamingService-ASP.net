using BLL.Models;
using System.ComponentModel.DataAnnotations;

namespace PublicModule.Models
{
    public class VMPublicGenre
    {
        public int Id { get; set; }
        [Required]
        public string? Name { get; set; }
        [Required]
        public string? Description { get; set; }

        public virtual ICollection<VMPublicVideo> Videos { get; set; } = new List<VMPublicVideo>();

        public override string ToString() => $"{Name} {Description}";
    }
}
