using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace GoingPlaces_2.Models
{
    public class GoingPlaces_2Context : DbContext
    {
        // You can add custom code to this file. Changes will not be overwritten.
        // 
        // If you want Entity Framework to drop and regenerate your database
        // automatically whenever you change your model schema, please use data migrations.
        // For more information refer to the documentation:
        // http://msdn.microsoft.com/en-us/data/jj591621.aspx
    
        public GoingPlaces_2Context() : base("name=GoingPlaces_2Context")
        {
        }

        public System.Data.Entity.DbSet<GoingPlaces_2.Models.Location> Locations { get; set; }

        public System.Data.Entity.DbSet<GoingPlaces_2.Models.Picture> Pictures { get; set; }
    }
}
