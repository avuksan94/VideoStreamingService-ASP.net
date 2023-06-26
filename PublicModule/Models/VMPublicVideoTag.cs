namespace PublicModule.Models
{
    public class VMPublicVideoTag
    {
        public int Id { get; set; }
        public int VideoId { get; set; }
        public int TagId { get; set; }
        public virtual VMPublicTag Tag { get; set; } = null!;
        public virtual VMPublicVideo Video { get; set; } = null!;
    }
}
