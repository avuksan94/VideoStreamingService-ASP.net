namespace PublicModule.Models
{
    public class VMPublicCountry
    {
        public int Id { get; set; }
        public string Code { get; set; } = null!;
        public string Name { get; set; } = null!;
        public virtual ICollection<VMPublicUser> Users { get; set; } = new List<VMPublicUser>();
    }
}
