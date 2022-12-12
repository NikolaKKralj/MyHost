namespace MyHostAPI.Domain
{
    public class Identity
    {
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
        public bool EmailConfirmed { get; set; }
        public Role Role { get; set; }
    }
}