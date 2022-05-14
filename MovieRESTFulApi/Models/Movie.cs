using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieRESTFulApi.Models
{
    public class Movie
    {
        public int MovieId { get; set; }
        public string MovieCode { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string URL { get; set; }
        public string Type { get; set; }
    }
}
