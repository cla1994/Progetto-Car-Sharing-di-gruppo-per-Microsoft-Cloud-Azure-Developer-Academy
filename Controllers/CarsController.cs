using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Academy2023.Net.Models;

namespace Academy2023.Net.Controllers
{
    public class CarsController : Controller
    {
        private readonly DataContext _context;

        public CarsController(DataContext context)
        {
            _context = context;
        }

        // GET: Cars
        public async Task<IActionResult> Index()
        {
            var dataContext = _context.cars.Include(c => c.CarCategory).Include(c => c.FuelType).Include(c => c.User);
            return View(await dataContext.ToListAsync());
        }

        // GET: Cars/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.cars == null)
            {
                return NotFound();
            }

            var car = await _context.cars
                .Include(c => c.CarCategory)
                .Include(c => c.FuelType)
                .Include(c => c.User)
                .FirstOrDefaultAsync(m => m.CarID == id);
            if (car == null)
            {
                return NotFound();
            }

            return View(car);
        }

        // GET: Cars/Create
        public IActionResult Create()
        {
            ViewData["CarCategoryID"] = new SelectList(_context.CarCategories, "CarCategoryID", "CarCategoryID");
            ViewData["FuelTypeID"] = new SelectList(_context.FuelTypes, "FuelTypeID", "FuelTypeID");
            ViewData["UserDataID"] = new SelectList(_context.usersData, "UserDataID", "UserDataID");
            return View();
        }

        // POST: Cars/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CarID,UserDataID,MaxSeat,RegNum,FuelTypeID,CarCategoryID,TrunkAvailable")] Car car)
        {
            if (ModelState.IsValid)
            {
                _context.Add(car);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CarCategoryID"] = new SelectList(_context.CarCategories, "CarCategoryID", "CarCategoryID", car.CarCategoryID);
            ViewData["FuelTypeID"] = new SelectList(_context.FuelTypes, "FuelTypeID", "FuelTypeID", car.FuelTypeID);
            ViewData["UserDataID"] = new SelectList(_context.usersData, "UserDataID", "UserDataID", car.UserDataID);
            return View(car);
        }

        // GET: Cars/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.cars == null)
            {
                return NotFound();
            }

            var car = await _context.cars.FindAsync(id);
            if (car == null)
            {
                return NotFound();
            }
            ViewData["CarCategoryID"] = new SelectList(_context.CarCategories, "CarCategoryID", "CarName", car.CarCategoryID);
            ViewData["FuelTypeID"] = new SelectList(_context.FuelTypes, "FuelTypeID", "FuelName", car.FuelTypeID);
            ViewData["UserDataID"] = new SelectList(_context.usersData, "UserDataID", "UserDataID", car.UserDataID);
            return View(car);
        }

        // POST: Cars/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CarID,UserDataID,MaxSeat,RegNum,FuelTypeID,CarCategoryID,TrunkAvailable")] Car car)
        {
            if (id != car.CarID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    car.FuelType = _context.FuelTypes.Find(car.FuelTypeID);
                    car.CarCategory = _context.CarCategories.Find(car.CarCategoryID);
                    car.User = _context.usersData.Find(car.UserDataID);
                    _context.Update(car);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CarExists(car.CarID))
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
            ViewData["CarCategoryID"] = new SelectList(_context.CarCategories, "CarCategoryID", "CarName", car.CarCategoryID);
            ViewData["FuelTypeID"] = new SelectList(_context.FuelTypes, "FuelTypeID", "FuelName", car.FuelTypeID);
            ViewData["UserDataID"] = new SelectList(_context.usersData, "UserDataID", "UserDataID", car.UserDataID);
            return View(car);
        }

        // GET: Cars/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.cars == null)
            {
                return NotFound();
            }

            var car = await _context.cars
                .Include(c => c.CarCategory)
                .Include(c => c.FuelType)
                .Include(c => c.User)
                .FirstOrDefaultAsync(m => m.CarID == id);
            if (car == null)
            {
                return NotFound();
            }

            return View(car);
        }

        // POST: Cars/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.cars == null)
            {
                return Problem("Entity set 'DataContext.cars'  is null.");
            }
            var car = await _context.cars.FindAsync(id);
            if (car != null)
            {
                _context.cars.Remove(car);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CarExists(int id)
        {
          return (_context.cars?.Any(e => e.CarID == id)).GetValueOrDefault();
        }
    }
}
