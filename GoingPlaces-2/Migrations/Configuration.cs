namespace GoingPlaces_2.Migrations
{
    using GoingPlaces_2.Models;
    using System;
    using System.Collections.ObjectModel;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Drawing;
    using System.IO;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<GoingPlaces_2.Models.GoingPlaces_2Context>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        //Method for seeding image Batman-Jim-Lee.jpg
        public byte[] ImageToArray()
        {
            //NB THIS IS THE LOCATION OF THE TEST IMAGE ON THE SERVER
            Image img = Image.FromFile(@"C:\Batman-Jim-Lee.jpg");
            byte[] arr;
            using (MemoryStream ms = new MemoryStream())
            {
                img.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                arr = ms.ToArray();
            }
            return arr;
        }

        protected override void Seed(GoingPlaces_2.Models.GoingPlaces_2Context context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //

            context.Locations.AddOrUpdate(a => a.Name,
                    new Location()
                    {
                        Name = "Gotham City", Latitude = 0, Longitude = 0,
                        Pictures = new Collection<Picture>()
                                     {
                                         new Picture(){Description = "Gotham City",
                                         DateTaken = DateTime.Now,
                                         FlickrImage = ImageToArray()}
                                     }
                    });
        }
    }
}
