using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyHostAPI.Models
{
    public class OperatingCityModel
    {
        public string? Id { get; set; }
        public string Name { get; set; } = null!;
        public double Lat { get; set; }
        public double Lng { get; set; }
    }
}
