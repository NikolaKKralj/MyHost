using Microsoft.AspNetCore.Http;

namespace MyHostAPI.Models
{
    public class UserUpdateModel
    {
        public string Id { get; set; } = null!;
        public string? Name { get; set; }
        public string? Surname { get; set; }
        public string PhoneNumber { get; set; } = null!;
        public IFormFile? Image { get; set; }
        public string? ProfileImage { get; set; }
        public AddressModel? Address { get; set; }
    }
}
