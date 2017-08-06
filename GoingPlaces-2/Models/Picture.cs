using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GoingPlaces_2.Models
{
    public class Picture
    {
        public int Id { get; set; }

        //Metadata foreach picture from flickr
        public string Description { get; set; }

        //Use question mark to make it nullable
        public DateTime? DateTaken { get; set; }

        //Each image will be stored as a byte array in the db
        public byte[] FlickrImage { get; set; }

        //Foreign Key
        public int LocationId { get; set; }

        //FK Navigation Property
        public Location Location { get; set; }
    }
}