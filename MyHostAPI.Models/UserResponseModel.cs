using MyHostAPI.Domain;

namespace MyHostAPI.Models
{
    public class UserResponseModel
    {
        public string Id { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string Surname { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;
        public string? ProfileImage { get; set; }
        public string Email { get; set; } = null!;
        public Role Role { get; set; }
        public AddressModel? Address { get; set; }
        public List<string> FavoritePremises { get; set; } = new List<string>();
    }
}
