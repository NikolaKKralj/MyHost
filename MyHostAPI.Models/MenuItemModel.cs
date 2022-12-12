using Microsoft.AspNetCore.Http;

namespace MyHostAPI.Models
{
    public class MenuItemModel
    {
        public IFormFile ImageFile { get; set; } = null!;
        public string? Image { get; set; }
        public int Order { get; set; }
    }
}
