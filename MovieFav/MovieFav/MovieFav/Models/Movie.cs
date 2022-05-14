using System;
using System.Collections.Generic;
using System.Text;

namespace MovieFav.Models
{
    public class Movie
    {
        public int MovieId { get; set; }
        public string MovieCode { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string URL { get; set; }
        public string Type { get; set; }
        public string ImageURL { get; set; }
    }
}
