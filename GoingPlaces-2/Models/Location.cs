using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GoingPlaces_2.Models
{
    public class Location
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }

        //1 to many relationship associated with images related to it
        public ICollection<Picture> Pictures { get; set; }
    }
}