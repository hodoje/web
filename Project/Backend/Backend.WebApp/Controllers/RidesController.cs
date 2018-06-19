using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using Backend.DataAccess;
using DomainEntities.Models;

namespace Backend.Controllers
{
    public class RidesController : ApiController
    {
        private DatabaseContext db = new DatabaseContext();

        // GET: api/Rides
        public IQueryable<Ride> GetRides()
        {
            return db.Rides;
        }

        // GET: api/Rides/5
        [ResponseType(typeof(Ride))]
        public IHttpActionResult GetRide(int id)
        {
            Ride ride = db.Rides.Find(id);
            if (ride == null)
            {
                return NotFound();
            }

            return Ok(ride);
        }

        // PUT: api/Rides/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutRide(int id, Ride ride)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != ride.Id)
            {
                return BadRequest();
            }

            db.Entry(ride).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RideExists(id))
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

        // POST: api/Rides
        [ResponseType(typeof(Ride))]
        public IHttpActionResult PostRide(Ride ride)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Rides.Add(ride);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = ride.Id }, ride);
        }

        // DELETE: api/Rides/5
        [ResponseType(typeof(Ride))]
        public IHttpActionResult DeleteRide(int id)
        {
            Ride ride = db.Rides.Find(id);
            if (ride == null)
            {
                return NotFound();
            }

            db.Rides.Remove(ride);
            db.SaveChanges();

            return Ok(ride);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool RideExists(int id)
        {
            return db.Rides.Count(e => e.Id == id) > 0;
        }
    }
}