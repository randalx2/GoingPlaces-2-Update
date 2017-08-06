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
    public class PicturesController : ApiController
    {
        private GoingPlaces_2Context db = new GoingPlaces_2Context();

        // GET: api/Pictures
        public IQueryable<Picture> GetPictures()
        {
            return db.Pictures;
        }

        // GET: api/Pictures/5
        [ResponseType(typeof(Picture))]
        public async Task<IHttpActionResult> GetPicture(int id)
        {
            Picture picture = await db.Pictures.FindAsync(id);
            if (picture == null)
            {
                return NotFound();
            }

            return Ok(picture);
        }

        // PUT: api/Pictures/5
        [ResponseType(typeof(void))]
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