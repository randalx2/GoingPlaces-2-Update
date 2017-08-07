using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using GoingPlaces_2.Models;
using System.Drawing;
using System.IO;
using FlickrNet;
using System.Collections.ObjectModel;

namespace GoingPlaces_2.Controllers
{
    [RoutePrefix("api/Pictures")]
    public class PicturesController : ApiController
    {
        private GoingPlaces_2Context db = new GoingPlaces_2Context();

        // GET: api/Pictures
        [Route("")]
        [HttpGet]
        public IQueryable<Picture> GetPictures()
        {
            return db.Pictures;
        }

        // GET: api/Pictures/5
        [ResponseType(typeof(Picture))]
        [Route("{id:int}")]
        [HttpGet]
        public async Task<IHttpActionResult> GetPicture(int id)
        {
            Picture picture = await db.Pictures.FindAsync(id);
            if (picture == null)
            {
                return NotFound();
            }

            return Ok(picture);
        }

        //Local service function to convert images to byte arrays before saving to our database
        //Pass in the large URL to set the image data
        public byte[] ImageToArray(string photoUrl)
        {
            WebClient web = new WebClient();

            //Download the image from its URL to the server
            byte[] arr = web.DownloadData(photoUrl);

            return arr;
        }

        //Use this to return a default image if no image data found on flickr
        public byte[] ImageToArrayDefault()
        {
            Image img = Image.FromFile(@"C:\Batman-Jim-Lee.jpg");
            byte[] arr;
            using (MemoryStream ms = new MemoryStream())
            {
                img.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                arr = ms.ToArray();
            }
            return arr;
        }

        // GET: api/Images/5
        [ResponseType(typeof(Picture))]
        [Route("{name}")]
        [HttpGet]
        public IEnumerable<Picture> GetImageByName(string name)
        {
            //Get the first contact in the contacts list with the specified id
            //Pass in the location name and check if the main image description contains it

            //Create the flickr objects and set it up on each call


            Flickr flickr = new Flickr();
            flickr.ApiKey = "dba11127902261afd54826b290ed3de6";
            flickr.ApiSecret = "1af450a5e13b378c";

            //Create the byte arrays to store the binary data of each object returned through the JSON

            //Set up search options object
            var options = new PhotoSearchOptions() { Tags = name, PerPage = 12, Page = 1, Extras = PhotoSearchExtras.LargeUrl | PhotoSearchExtras.Tags | PhotoSearchExtras.Small320Url };

            //This return all image objects including main description, landmark id and secondary images
            Picture[] ImageArray = db.Pictures.Where<Picture>(c => (c.Description.Contains(name))).ToArray();

            //If the location or any duplicate of it is not found in the ImageArray the array size count will be O
            //To make it easier convert this array to a list object for now
            List<Picture> myImageList = ImageArray.ToList<Picture>();

            //If we have 0 entries in the list corresponding to the search name then check flickr

            //Initialize a gallery of 12 pics
            Picture[] myImageObject = new Picture[8]
            {
                new Picture() { Id = -1, LocationId = -1, Description = "Image Not Found", FlickrImage = ImageToArrayDefault()},
                new Picture() { Id = -1, LocationId = -1, Description = "Image Not Found", FlickrImage = ImageToArrayDefault()},
                new Picture() { Id = -1, LocationId = -1, Description = "Image Not Found", FlickrImage = ImageToArrayDefault()},
                new Picture() { Id = -1, LocationId = -1, Description = "Image Not Found", FlickrImage = ImageToArrayDefault()},
                new Picture() { Id = -1, LocationId = -1, Description = "Image Not Found", FlickrImage = ImageToArrayDefault()},
                new Picture() { Id = -1, LocationId = -1, Description = "Image Not Found", FlickrImage = ImageToArrayDefault()},
                new Picture() { Id = -1, LocationId = -1, Description = "Image Not Found", FlickrImage = ImageToArrayDefault()},
                new Picture() { Id = -1, LocationId = -1, Description = "Image Not Found", FlickrImage = ImageToArrayDefault()},
                //new Picture() { Id = -1, LocationId = -1, Description = "Image Not Found", FlickrImage = ImageToArrayDefault()},
                //new Picture() { Id = -1, LocationId = -1, Description = "Image Not Found", FlickrImage = ImageToArrayDefault()},
                //new Picture() { Id = -1, LocationId = -1, Description = "Image Not Found", FlickrImage = ImageToArrayDefault()},
                //new Picture() { Id = -1, LocationId = -1, Description = "Image Not Found", FlickrImage = ImageToArrayDefault()},
            };


            try
            {
                if (myImageList.Count <= 0)
                {
                    //Search for photos using the search option
                    //Search tags are set for a max of 3 photos per page
                    PhotoCollection photos = flickr.PhotosSearch(options);

                    //We should at least return 3 photos based on the tag

                    //If we found some photos based on tags return at least 12
                    if (photos.Count > 0)
                    {
                        int counter = 0;

                        for (int j = 0; j < 8; j++)
                        {
                            if (counter < photos.Count)
                            {
                                myImageObject[j].Description = "Description: " + photos[counter].Title + "\n Date Uploaded: " + photos[counter].DateUploaded +
                                                           "\n Date Taken: " + photos[counter].DateTaken + "\n Place ID: " + photos[counter].PlaceId +
                                                           "\n Latitude: " + photos[counter].Latitude + "\n Longitude: " + photos[counter].Longitude;

                                myImageObject[j].DateTaken = photos[counter].DateTaken;
                                myImageObject[j].FlickrImage = ImageToArray(photos[counter].Small320Url);
                                ++counter;
                                if (counter >= photos.Count) break;
                            }
                            else
                            {
                                break;
                            }
                        }

                        counter = 0;

                        //Find the location that matches the name in the array
                        Location location = db.Locations.Where<Location>(c => c.Name.Contains(name)).FirstOrDefault<Location>();

                        //If location is not found then add a new one
                        //Also add the pictures associated with it
                        if (location == null)
                        {
                            Location flickrLocation = new Location()
                            {
                                Name = name,
                                Latitude = 0,
                                Longitude = 0,

                                Pictures = new Collection<Picture>()
                                {
                                    new Picture(){Description = myImageObject[0].Description, DateTaken = myImageObject[0].DateTaken, FlickrImage = myImageObject[0].FlickrImage},
                                    new Picture(){Description = myImageObject[1].Description, DateTaken = myImageObject[1].DateTaken, FlickrImage = myImageObject[1].FlickrImage},
                                    new Picture(){Description = myImageObject[2].Description, DateTaken = myImageObject[2].DateTaken, FlickrImage = myImageObject[2].FlickrImage},
                                    new Picture(){Description = myImageObject[3].Description, DateTaken = myImageObject[3].DateTaken, FlickrImage = myImageObject[3].FlickrImage},
                                    new Picture(){Description = myImageObject[4].Description, DateTaken = myImageObject[4].DateTaken, FlickrImage = myImageObject[4].FlickrImage},
                                    new Picture(){Description = myImageObject[5].Description, DateTaken = myImageObject[5].DateTaken, FlickrImage = myImageObject[5].FlickrImage},
                                    new Picture(){Description = myImageObject[6].Description, DateTaken = myImageObject[6].DateTaken, FlickrImage = myImageObject[6].FlickrImage},
                                    new Picture(){Description = myImageObject[7].Description, DateTaken = myImageObject[7].DateTaken, FlickrImage = myImageObject[7].FlickrImage}
                                }
                            };

                            db.Locations.Add(flickrLocation);
                            db.SaveChanges();                        
                        }
                        
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + Environment.NewLine + "Your Internet Connection may be down");
            }

            //Convert back to array
            ImageArray = myImageList.ToArray();

            //db.SaveChanges();
            return ImageArray;
        }

        // PUT: api/Pictures/5
        [ResponseType(typeof(void))]
        [HttpPut]
        public async Task<IHttpActionResult> PutPicture(int id, Picture picture)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != picture.Id)
            {
                return BadRequest();
            }

            db.Entry(picture).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PictureExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Pictures
        [ResponseType(typeof(Picture))]
        [HttpPost]
        public async Task<IHttpActionResult> PostPicture(Picture picture)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Pictures.Add(picture);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = picture.Id }, picture);
        }

        // DELETE: api/Pictures/5
        [ResponseType(typeof(Picture))]
        [HttpDelete]
        public async Task<IHttpActionResult> DeletePicture(int id)
        {
            Picture picture = await db.Pictures.FindAsync(id);
            if (picture == null)
            {
                return NotFound();
            }

            db.Pictures.Remove(picture);
            await db.SaveChangesAsync();

            return Ok(picture);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool PictureExists(int id)
        {
            return db.Pictures.Count(e => e.Id == id) > 0;
        }
    }
}