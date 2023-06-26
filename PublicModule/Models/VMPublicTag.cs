namespace PublicModule.Models
{
    public class VMPublicTag
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public virtual ICollection<VMPublicVideoTag> VideoTags { get; set; } = new List<VMPublicVideoTag>();
    }
}
