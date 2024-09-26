using LoginApp.Data;
using LoginApp.Models;
using LoginApp.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static System.Net.Mime.MediaTypeNames;

namespace LoginApp.Controllers
{
    [Authorize]
    public class CarController : Controller
    {
        private readonly AppDBContext _dbContext;
        public CarController(AppDBContext appDBContext)
        {
            _dbContext = appDBContext;
        }

        [HttpGet]
        public async Task<IActionResult> GetCars()
        {
            List<Car> cars = await _dbContext.Cars.ToListAsync();
            return View(cars);
        }

        [HttpGet]
        public async Task<IActionResult> GetCarById(int id)
        {
            var car = await _dbContext.Cars.FirstOrDefaultAsync(e => e.IdCar == id);
            return StatusCode(StatusCodes.Status200OK, car);
        }

        [HttpGet]
        public IActionResult AddCar()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddCar(CarViewModel carViewModel)
        {
            if (carViewModel.Year.ToString().Length != 4)
            {
                ModelState.AddModelError("Year", "The year must have exactly 4 digits.");
            }

            if (!ModelState.IsValid)
            {
                Console.WriteLine($"Model state is invalid: {string.Join(", ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage))}");
                return View(carViewModel);
            }

            Car car = new Car()
            {
                Brand = carViewModel.Brand,
                Model = carViewModel.Model,
                Year = carViewModel.Year,
                Km = carViewModel.Km,
                Color = carViewModel.Color,
                Used = carViewModel.Used,
                Images = new List<byte[]>()
            };

            if (carViewModel.Images != null && carViewModel.Images.Count > 0)
            {
                foreach (var image in carViewModel.Images)
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        await image.CopyToAsync(memoryStream);
                        car.Images.Add(memoryStream.ToArray());
                        Console.WriteLine($"Image loaded: {image.FileName}, Size: {image.Length} bytes");
                    }
                }
            }
            await _dbContext.Cars.AddAsync(car);
            await _dbContext.SaveChangesAsync();

            return RedirectToAction("GetCars");
        }

        [HttpGet]
        public async Task<IActionResult> EditCar(int id)
        {
            var car = await _dbContext.Cars.FindAsync(id);
            if (car == null)
            {
                return NotFound();
            }

            var carViewModel = new CarViewModel
            {
                IdCar = car.IdCar,
                Brand = car.Brand,
                Model = car.Model,
                Year = car.Year,
                Km = car.Km,
                Color = car.Color,
                Used = car.Used,
                ExistingImages = car.Images.Select(i => Convert.ToBase64String(i)).ToList()
            };

            return View("AddCar", carViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> EditCar(CarViewModel carViewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(carViewModel);
            }

            var car = await _dbContext.Cars.FindAsync(carViewModel.IdCar);
            if (car == null)
            {
                return NotFound();
            }

            car.Brand = carViewModel.Brand;
            car.Model = carViewModel.Model;
            car.Year = carViewModel.Year;
            car.Km = carViewModel.Km;
            car.Color = carViewModel.Color;
            car.Used = carViewModel.Used;


            _dbContext.Cars.Update(car);
            await _dbContext.SaveChangesAsync();

            return RedirectToAction("GetCars");
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteEmployeeById(int id)
        {
            var car = await _dbContext.Cars.FirstOrDefaultAsync(e => e.IdCar == id);
            _dbContext.Cars.Remove(car);
            await _dbContext.SaveChangesAsync();
            return StatusCode(StatusCodes.Status200OK, new { message = "ok" });
        }
    }
}
