using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Academy2023.Net.Models;
using SQLitePCL;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authorization;

namespace Academy2023.Net.Controllers
{
    [Authorize]
    public class UserDatasController : Controller
    {
        private readonly DataContext _context;

        public UserDatasController(DataContext context)
        {
            _context = context;
            if (_context.Genders.Count() == 0)
            {
                _context.Genders.Add(new Gender()
                {
                    GenderName = "Male"
                });
                _context.Genders.Add(new Gender()
                {
                    GenderName = "Female"
                });
                _context.Genders.Add(new Gender()
                {
                    GenderName = "Not Specified"
                });
            }
            if (_context.FuelTypes.Count() == 0)
            {
                _context.FuelTypes.Add(new FuelType()
                {
                    FuelName = "Gasoline"
                });
                _context.FuelTypes.Add(new FuelType()
                {
                    FuelName = "Unleaded"
                });
                _context.FuelTypes.Add(new FuelType()
                {
                    FuelName = "GPL"
                });
                _context.FuelTypes.Add(new FuelType()
                {
                    FuelName = "Hybrid"
                });
                _context.FuelTypes.Add(new FuelType()
                {
                    FuelName = "Full Electric"
                });
                _context.FuelTypes.Add(new FuelType()
                {
                    FuelName = "Methane"
                });
            }
            if (_context.CarCategories.Count() == 0)
            {
                _context.CarCategories.Add(new CarCategory()
                {
                    CarName = "Microcar"
                });
                _context.CarCategories.Add(new CarCategory()
                {
                    CarName = "City Car"
                });
                _context.CarCategories.Add(new CarCategory()
                {
                    CarName = "Supermini"
                });
                _context.CarCategories.Add(new CarCategory()
                {
                    CarName = "Small family"
                });
                _context.CarCategories.Add(new CarCategory()
                {
                    CarName = "Large family"
                });
                _context.CarCategories.Add(new CarCategory()
                {
                    CarName = "Executive"
                });
                _context.CarCategories.Add(new CarCategory()
                {
                    CarName = "Minivans"
                });
                _context.CarCategories.Add(new CarCategory()
                {
                    CarName = "SUV"
                });
                _context.CarCategories.Add(new CarCategory()
                {
                    CarName = "Muscle Car"
                });
            }
            _context.SaveChanges();
        }

        public IActionResult GetPicture(int Id)
        {
            Picture picture = _context.Pictures.Find(Id);
            if (picture == null)
            {
                return NotFound();
            }
            var rawData = picture.RawData;
            MemoryStream ms1 = new MemoryStream(rawData);
            return File(ms1.ToArray(), "image/png");
        }

        [HttpGet]
        public IActionResult DeletePicture(int PictureId,int UserId)
        {
            UserData user = _context.usersData
                .Include(x=>x.Gender)
                .Include(x=>x.Picture)
                .FirstOrDefault(x=>x.UserDataID==UserId);

            var file = System.IO.File.ReadAllBytes(@".\wwwroot\img\unknown-users.png");

            Picture picture = new Picture()
            {
                PictureName = "default",
                RawData = file.ToArray()
            };
            Picture _default = _context.Pictures
                .Where(x => x.RawData == picture.RawData)
                .FirstOrDefault();

            Picture pic = _context.Pictures.Find(PictureId);
            if (pic != _default)
            {
                _context.Pictures.Remove(pic);
            }
            //ViewData["message"] = "Cannot delete default picture";

            if (_default != null)
            {
                user.Picture = _default;
            } else
            {
                user.Picture= picture;
                _context.Pictures.Add(picture);
            }
            _context.Update(user);
            _context.SaveChanges();
            return RedirectToAction(nameof(Edit), new { Id = UserId });
        }

        // GET: UserDatas
        public async Task<IActionResult> Index()
        {
            var dataContext = _context.usersData.Include(u => u.Gender);
            return View(await dataContext.ToListAsync());
        }

        // GET: UserDatas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.usersData == null)
            {
                return NotFound();
            }

            var userData = await _context.usersData
                .Include(u => u.Gender)
                .Include(u => u.UserCar)
                .ThenInclude(c=>c.CarCategory)
                .Include(u=>u.UserCar)
                .ThenInclude(c=>c.FuelType)
                .Include(p => p.Picture)
                .FirstOrDefaultAsync(m => m.UserDataID == id);

            if (userData == null)
            {
                return NotFound();
            }

            return View(userData);
        }

        // GET: UserDatas/Create
        public IActionResult Create()
        {
            ViewData["GenderID"] = new SelectList(_context.Genders, "GenderID", "GenderName");
            return View();
        }

        // POST: UserDatas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("UserDataID,FirstName,LastName,GenderID,BirthDate,CF,License,AuthID,Description,HasCar")] UserData userData)
        {
            userData.Gender = _context.Genders
                                .Where(x => x.GenderID == userData.GenderID)
                                .FirstOrDefault();
            if (ModelState.IsValid)
            {
                if (Request.Form.Files.Count == 1)
                {
                    var file = Request.Form.Files[0];
                    var fileName = file.FileName;
                    using (MemoryStream ms = new MemoryStream())
                    {
                        file.CopyTo(ms);
                        Picture picture = new Picture()
                        {
                            PictureName = fileName,
                            RawData = ms.ToArray()
                        };
                        _context.Pictures.Add(picture);
                        if (userData.Picture != null)
                        {
                            _context.Pictures.Remove(userData.Picture);
                        }
                        userData.Picture = picture;
                    }
                }
                else
                {
                    var file = System.IO.File.ReadAllBytes(@".\wwwroot\img\unknown-users.png");

                    Picture picture = new Picture()
                    {
                        PictureName = "default",
                        RawData = file.ToArray()
                    };
                    Picture _default = _context.Pictures
                        .Where(x => x.RawData == picture.RawData)
                        .FirstOrDefault();
                    if (_default != null)
                    {
                        userData.Picture = _default;
                    }
                    else
                    {
                        _context.Pictures.Add(picture);
                        userData.Picture = picture;
                    }
                }
                _context.Add(userData);
                await _context.SaveChangesAsync();
                
                return RedirectToAction(nameof(Details), new {Id = userData.UserDataID});
                
                
            }
            ViewData["GenderID"] = new SelectList(_context.Genders, "GenderID", "GenderName", userData.GenderID);
            return View(userData);
        }

        public IActionResult CreateCars(int id)
        {
            ViewData["CarCategoryID"] = new SelectList(_context.CarCategories, "CarCategoryID", "CarName");
            ViewData["FuelTypeID"] = new SelectList(_context.FuelTypes, "FuelTypeID", "FuelName");
            ViewData["UserDataID"] = id;
            return View();
        }

        // POST: Cars/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateCars([Bind("CarID,UserDataID,MaxSeat,RegNum,FuelTypeID,CarCategoryID,TrunkAvailable")] Car car)
        {
            car.User = _context.usersData
                        .Where(x => x.UserDataID == car.UserDataID)
                        .FirstOrDefault();
            car.FuelType = _context.FuelTypes
                        .Where(x => x.FuelTypeID == car.FuelTypeID)
                        .FirstOrDefault();
            car.CarCategory = _context.CarCategories
                        .Where(x => x.CarCategoryID == car.CarCategoryID)
                        .FirstOrDefault();
            UserData user = _context.usersData
                        .Where(x => x.UserDataID == car.UserDataID)
                        .FirstOrDefault();
            if (ModelState.IsValid)
            {
                _context.cars.Add(car);
                if (!user.HasCar)
                {
                    user.HasCar = true;
                }
                _context.usersData.Update(user);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Details), new { Id = car.UserDataID });
            }
            ViewData["CarCategoryID"] = new SelectList(_context.CarCategories, "CarCategoryID", "CarName", car.CarCategoryID);
            ViewData["FuelTypeID"] = new SelectList(_context.FuelTypes, "FuelTypeID", "FuelName", car.FuelTypeID);
            ViewData["UserDataID"] = car.UserDataID;
            return View(car);
        }

        // GET: UserDatas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.usersData == null)
            {
                return NotFound();
            }

            var userData = await _context.usersData
                .Include(g => g.Gender)
                .Include(p => p.Picture)
                .FirstOrDefaultAsync(m => m.UserDataID == id);
            if (userData == null)
            {
                return NotFound();
            }
            ViewData["GenderID"] = new SelectList(_context.Genders, "GenderID", "GenderName", userData.GenderID);
            return View(userData);
        }

        // POST: UserDatas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("UserDataID,FirstName,LastName,GenderID,BirthDate,CF,License,AuthID,Description,HasCar")] UserData userData)
        {
            if (id != userData.UserDataID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                if (Request.Form.Files.Count == 1)
                {
                    var file = Request.Form.Files[0];
                    var fileName = file.FileName;
                    using (MemoryStream ms = new MemoryStream())
                    {
                        file.CopyTo(ms);
                        Picture picture = new Picture()
                        {
                            PictureName = fileName,
                            RawData = ms.ToArray()
                        };
                        _context.Pictures.Add(picture);
                        if (userData.Picture != null)
                        {
                            _context.Pictures.Remove(userData.Picture);
                        }
                        userData.Picture = picture;
                    }
                }
                try
                {
                    userData.Gender = _context.Genders.Find(userData.GenderID);
                    _context.Update(userData);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserDataExists(userData.UserDataID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Details), new { Id = userData.UserDataID });
                
            }
            ViewData["GenderID"] = new SelectList(_context.Genders, "GenderID", "GenderName", userData.GenderID);
            return View(userData);
        }

        // GET: Cars/Edit/5
        public async Task<IActionResult> EditCar(int? id)
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
            ViewData["UserDataID"] = car.UserDataID;
            return View(car);
        }

        // POST: Cars/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditCar(int id, [Bind("CarID,UserDataID,MaxSeat,RegNum,FuelTypeID,CarCategoryID,TrunkAvailable")] Car car)
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
                return RedirectToAction(nameof(Details), new { Id = car.UserDataID });
            }
            ViewData["CarCategoryID"] = new SelectList(_context.CarCategories, "CarCategoryID", "CarName", car.CarCategoryID);
            ViewData["FuelTypeID"] = new SelectList(_context.FuelTypes, "FuelTypeID", "FuelName", car.FuelTypeID);
            ViewData["UserDataID"] = car.UserDataID;
            return View(car);
        }

        // GET: UserDatas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.usersData == null)
            {
                return NotFound();
            }

            var userData = await _context.usersData
                .Include(u => u.Gender)
                .FirstOrDefaultAsync(m => m.UserDataID == id);
            if (userData == null)
            {
                return NotFound();
            }

            return View(userData);
        }

        // POST: UserDatas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.usersData == null)
            {
                return Problem("Entity set 'DataContext.usersData'  is null.");
            }
            var userData = await _context.usersData.FindAsync(id);
            if (userData != null)
            {
                _context.usersData.Remove(userData);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> DeleteCar(int? id)
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
        [HttpPost, ActionName("DeleteCar")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteCarConfirmed(int id, int userID)
        {
            UserData user = _context.usersData.Find(userID);
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
            if (_context.cars.IsNullOrEmpty())
            {
                user.HasCar = false;
                _context.usersData.Update(user);
            }
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Details), new { Id = userID });
        }


        private bool UserDataExists(int id)
        {
          return (_context.usersData?.Any(e => e.UserDataID == id)).GetValueOrDefault();
        }

        private bool CarExists(int id)
        {
            return (_context.cars?.Any(e => e.CarID == id)).GetValueOrDefault();
        }
    }
}
