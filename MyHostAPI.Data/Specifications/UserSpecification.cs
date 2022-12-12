using MyHostAPI.Domain;

namespace MyHostAPI.Data.Specifications
{
    public class ActiveUsers : BaseSpecification<User>
    {
        public ActiveUsers() : base(b => b.IsDeleted == false)
        {

        }
    } 

    public class UserByUsername : BaseSpecification<User>
    {
        public UserByUsername(string username) : base(b => b.Identity.Email == username)
        {

        }
    }

    public class UsersByRole : BaseSpecification<User>
    {
        public UsersByRole(Role role) : base(b => b.Identity.Role == role)
        {

        }
    }

    public class UserById : BaseSpecification<User>
    {
        public UserById(string id) : base(b => b.Id == id)
        {

        }
    }

    public class UserByEmail : BaseSpecification<User>
    {
        public UserByEmail(string email) : base(b => b.Identity.Email == email)
        {

        }
    }
}

