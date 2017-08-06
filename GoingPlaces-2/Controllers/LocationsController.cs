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

namespace GoingPlaces_2.Controllers
{
    [RoutePrefix("api/Locations")]
    public class LocationsController : ApiController
    {
        private GoingPlaces_2Context db = new GoingPlaces_2Context();

        // GET: api/Locations
        [Route("")]
        [HttpGet]
        public IQueryable<Location> GetLocations()
        {
            return db.Locations;
        }

        // GET: api/Locations/5
        // GET: api/Locations/5
        [ResponseType(typeof(Location))]
        [Route("{id:int}")]
        [HttpGet]
        [ResponseType(typeof(Location))]
        public async Task<IHttpActionResult> GetLocation(int id)
        {
            Location location = await db.Locations.FindAsync(id);
            if (location == null)
            {
                return NotFound();
            }

            return Ok(location);
        }

        // GET: api/Locations/5
        [ResponseType(typeof(Location))]
        [Route("{name}")]
        [HttpGet]
        public IEnumerable<Location> GetLocationByName(string name)
        {
            //Get the first contact in the contacts list with the specified id
            Location[] locationArray = db.Locations.Where<Location>(c => c.Name.Contains(name)).ToArray();

            return locationArray;
        }

        // PUT: api/Locations/5
        [ResponseType(typeof(void))]
        [HttpPut]
        public async Task<IHttpActionResult> PutLocation(int id, Location location)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != location.Id)
            {
                return BadRequest();
            }

            db.Entry(location).State = EntityState.Modified;
            Location newlocation = db.Locations.FirstOrDefault<Location>(c => c.Id == id);

            if (location != null)
            {
                location.Name = newlocation.Name;
                location.Latitude = newlocation.Latitude;
                location.Longitude = newlocation.Longitude;
            }

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LocationExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            //return StatusCode(HttpStatusCode.NoContent);
            return Ok(location);
        }

        // POST: api/Locations
        [ResponseType(typeof(Location))]
        [HttpPost]
        public async Task<IHttpActionResult> PostLocation(Location location)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Locations.Add(location);
            await db.SaveChangesAsync();

            //return CreatedAtRoute("DefaultApi", new { id = location.Id }, location);
            return Ok(location);
        }

        // DELETE: api/Locations/5
        [ResponseType(typeof(Location))]
        [HttpDelete]
        public async Task<IHttpActionResult> DeleteLocation(int id)
        {
            Location location = await db.Locations.FindAsync(id);
            if (location == null)
            {
                return NotFound();
            }

            db.Locations.Remove(location);
            await db.SaveChangesAsync();

            return Ok(location);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool LocationExists(int id)
        {
            return db.Locations.Count(e => e.Id == id) > 0;
        }
    }
}