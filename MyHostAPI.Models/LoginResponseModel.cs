namespace MyHostAPI.Models
{
    public class LoginResponseModel
    {
        public UserResponseModel User { get; set; } = null!;
        public string Token { get; set; } = null!;
    }
}
