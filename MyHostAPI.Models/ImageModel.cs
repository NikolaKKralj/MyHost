using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyHostAPI.Models
{
    public class ImageModel
    {
        public IFormFile? ImageFile { get; set; }
        public string? Path { get; set; } = null!;
        public int Order { get; set; }
    }
}
