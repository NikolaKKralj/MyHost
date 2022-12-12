namespace MyHostAPI.Domain
{
    public class User : BaseEntity
    {
        public string Name { get; set; } = null!;
        public string Surname { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;
        public string? ProfileImage { get; set; }
        public Identity Identity { get; set; } = null!;
        public Address? Address { get; set; }
        public List<string> FavoritePremises { get; set; } = new List<string>();
    }
}