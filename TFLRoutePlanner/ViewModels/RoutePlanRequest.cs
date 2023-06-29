using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TFLRoutePlanner.ViewModels
{
    public class RoutePlanRequest
    {
        public string? source { get; set; }
        public string? destination { get; set; }
        public string? excludeStation { get; set; }
        public string? viaStation { get; set; }

    }
}
