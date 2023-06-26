namespace PublicModule.Models
{
    public class VMPublicImage
    {
        public int Id { get; set; }

        public string Content { get; set; } = null!;

        public virtual ICollection<VMPublicVideo> Videos { get; set; } = new List<VMPublicVideo>();
    }
}
