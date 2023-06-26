using System.ComponentModel.DataAnnotations;

namespace AdminModule.Models
{
    public class VMGenre
    {
        public int Id { get; set; }
        [Required]
        public string? Name { get; set; }
        [Required]
        public string? Description { get; set; }

        public virtual ICollection<VMVideo> Videos { get; set; } = new List<VMVideo>();

        public override string ToString() => $"{Name} {Description}";
    }
}
