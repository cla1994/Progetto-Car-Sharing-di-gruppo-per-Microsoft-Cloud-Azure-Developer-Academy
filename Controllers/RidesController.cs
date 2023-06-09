using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Academy2023.Net.Models;
using System.Text.Json.Nodes;
using static Academy2023.Net.Models.APIResponses;
using Microsoft.Extensions.Azure;
using NuGet.Protocol;
using System.Data;
using System.Drawing;

namespace Academy2023.Net.Controllers
{
    public class RidesController : Controller
    {
        private readonly DataContext _context;
        private readonly RoutesAPIController _routes;
        
       

        public RidesController(DataContext context, RoutesAPIController foo)
        {
            _context = context;
            _routes = foo;

        }

        // GET: Rides
        public async Task<IActionResult> Index()
        {
            var dataContext = _context.Rides.Include(r => r.Car);
            return View(await dataContext.ToListAsync());
        }

        // GET: Rides/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            var co = await _routes.GetCoordinate();
            string[] k = co.ToString().Split(",".ToCharArray());
            ViewData["Partenza"] = $"{{ lat: {k[0]}.{k[1]} ,lng: {k[2]}.{k[3]} }}";

            ViewData["Center"] = $" {k[0]}.{k[1]} , {k[2]}.{k[3]} ";

            var cn = await _routes.GetCoordinate();
            string[] j = cn.ToString().Split(",".ToCharArray());
            ViewData["Arrivo"] = $"{{ lat: {j[0]}.{j[1]} ,lng: {j[2]}.{j[3]} }}";


            if (id == null || _context.Rides == null)
            {
                return NotFound();
            }

            var ride = await _context.Rides
                .Include(r => r.Car)
                .FirstOrDefaultAsync(m => m.RideId == id);
            if (ride == null)
            {
                return NotFound();
            }

            return View(ride);
        }

        // GET: Rides/Create
        public IActionResult Create()
        {
            ViewData["CarID"] = new SelectList(_context.cars, "CarID", "CarID");
            return View();
        }

        // POST: Rides/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("RideId,Departure,Arrival,DepartureTime,ArrivalTime,Duration,Km,AvailableSeat,CarID")] Ride ride)
        {
            ModelState.Clear();
            ride.Car = _context.cars.Find(ride.CarID);
            TryValidateModel(ride);

            var co = await _routes.CalcoloArrivo(ride.Departure, ride.Arrival);
            ride.Km = co.Distances;
            ride.Duration = co.Durations;
            TimeSpan DeltaT = TimeSpan.FromSeconds(ride.Duration);
            ride.ArrivalTime = DateTime.Now;

            if (ModelState.IsValid)
            {
                _context.Add(ride);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CarID"] = new SelectList(_context.cars, "CarID", "CarID", ride.CarID);
            return View(ride);
        }

        // GET: Rides/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Rides == null)
            {
                return NotFound();
            }

            var ride = await _context.Rides.FindAsync(id);
            if (ride == null)
            {
                return NotFound();
            }
            ViewData["CarID"] = new SelectList(_context.cars, "CarID", "CarID", ride.CarID);
            return View(ride);
        }

        // POST: Rides/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("RideId,Departure,Arrival,DepartureTime,ArrivalTime,Duration,Km,AvailableSeat,CarID")] Ride ride)
        {
            if (id != ride.RideId)
            {
                return NotFound();
            }

            ModelState.Clear();
            ride.Car = _context.cars.Find(ride.CarID);
            TryValidateModel(ride);

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(ride);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RideExists(ride.RideId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["CarID"] = new SelectList(_context.cars, "CarID", "CarID", ride.CarID);
            return View(ride);
        }

        // GET: Rides/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            var co = await _routes.GetCoordinate();
            string[] k = co.ToString().Split(",".ToCharArray());
            ViewData["Partenza"] = $"{{ lat: {k[0]}.{k[1]} ,lng: {k[2]}.{k[3]} }}";

            ViewData["Center"] = $" {k[0]}.{k[1]} , {k[2]}.{k[3]} ";

            var cn = await _routes.GetCoordinate();
            string[] j = cn.ToString().Split(",".ToCharArray());
            ViewData["Arrivo"] = $"{{ lat: {j[0]}.{j[1]} ,lng: {j[2]}.{j[3]} }}";

            if (id == null || _context.Rides == null)
            {
                return NotFound();
            }

            var ride = await _context.Rides
                .Include(r => r.Car)
                .FirstOrDefaultAsync(m => m.RideId == id);
            if (ride == null)
            {
                return NotFound();
            }

            return View(ride);
        }

        // POST: Rides/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Rides == null)
            {
                return Problem("Entity set 'DataContext.Rides'  is null.");
            }
            var ride = await _context.Rides.FindAsync(id);
            if (ride != null)
            {
                _context.Rides.Remove(ride);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RideExists(int id)
        {
          return (_context.Rides?.Any(e => e.RideId == id)).GetValueOrDefault();
        }

        public IActionResult Path()
        {
            return View();
        }
    }
}
