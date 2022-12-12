using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyHostAPI.Models
{
    public class ReviewUpdateModel
    {
        public string Id { get; set; } = null!;
        public string Comment { get; set; } = null!;
        public int Rating { get; set; }
    }
}
