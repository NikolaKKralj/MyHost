using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyHostAPI.Domain
{
    public class Tag : BaseEntity
    {
        public string Name { get; set; } = null!;
        public List<string> PremiseIds { get; set; } = new List<string>();
    }
}
