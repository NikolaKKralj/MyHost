
using MyHostAPI.Domain;

namespace MyHostAPI.Common.Models
{
    public class UserContext
    {
        public readonly string UserId;
        public readonly string Email;
        public readonly Role Role;

        public UserContext(string userId, string email, Role role)
        {
            UserId = userId;
            Email = email;
            Role = role;
        }
    }
}

