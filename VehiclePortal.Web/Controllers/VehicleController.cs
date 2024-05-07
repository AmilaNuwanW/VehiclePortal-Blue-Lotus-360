using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using System.Drawing;
using VehiclePortal.Web.Data;
using VehiclePortal.Web.Models;
using VehiclePortal.Web.Models.Entities;

namespace VehiclePortal.Web.Controllers
{
    public class VehicleController : Controller
    {
        private readonly VehicleContext dbContext;

        public VehicleController(VehicleContext dbContext)
        {
            this.dbContext = dbContext;
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateVehicleViewModel viewModel)
        {
            try {

                if (await dbContext.Vehicles.AnyAsync(v => v.VIN == viewModel.VIN))
                {
                    TempData["ErrorMessage"] = "Vehicle with the same VIN already exists.";
                    return View();
                }

                if (await dbContext.Vehicles.AnyAsync(v => v.LicensePlate == viewModel.LicensePlate))
                {
                    TempData["ErrorMessage"] = "Vehicle with the same License Plate already exists.";
                    return View();
                }

                var vehicle = new Vehicle
            {
                Make = viewModel.Make,
                Model = viewModel.Model,
                Year = viewModel.Year,
                Color = viewModel.Color,
                VIN = viewModel.VIN,
                LicensePlate = viewModel.LicensePlate,
            };

            if (viewModel.ImageFile != null && viewModel.ImageFile.Length > 0)
            {
                using (var ms = new MemoryStream())
                {
                    viewModel.ImageFile.CopyTo(ms);
                    var fileBytes = ms.ToArray();
                    vehicle.ImageBase64 = Convert.ToBase64String(fileBytes);
                }
            }
            

            await dbContext.Vehicles.AddAsync(vehicle);
            await dbContext.SaveChangesAsync();

            TempData["Message"] = "Vehicle added successfully.";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Something Went Wrong";
            }

            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Details() 
        {
            var vehicles = await dbContext.Vehicles.ToListAsync();

            return View(vehicles);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            var vehicle = await dbContext.Vehicles.FindAsync(id);

            return View(vehicle);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Vehicle viewModel)
        {
            try {

                var vehicle = await dbContext.Vehicles.FindAsync(viewModel.Id);

                if(vehicle is not null)
                {

                    if (vehicle.VIN != viewModel.VIN)
                    {
                        if (await dbContext.Vehicles.AnyAsync(v => v.VIN == viewModel.VIN))
                        {
                            TempData["ErrorMessage"] = "Another vehicle with the same VIN already exists.";
                            return View(vehicle);
                        }
                    }
                    if (vehicle.LicensePlate != viewModel.LicensePlate)
                    {
                        if (await dbContext.Vehicles.AnyAsync(v => v.LicensePlate == viewModel.LicensePlate))
                        {
                            TempData["ErrorMessage"] = "Another vehicle with the same License Plate already exists.";
                            return View(vehicle);
                        }
                    }

                    vehicle.Make = viewModel.Make;
                    vehicle.Model = viewModel.Model;
                    vehicle.Year = viewModel.Year;
                    vehicle.Color = viewModel.Color;
                    vehicle.VIN = viewModel.VIN;
                    vehicle.LicensePlate = viewModel.LicensePlate;

                    if (viewModel.ImageFile != null && viewModel.ImageFile.Length > 0)
                    {
                        using (var ms = new MemoryStream())
                        {
                            viewModel.ImageFile.CopyTo(ms);
                            var fileBytes = ms.ToArray();
                            vehicle.ImageBase64 = Convert.ToBase64String(fileBytes);
                        }
                    }

                    await dbContext.SaveChangesAsync();
                    TempData["Message"] = "Updated Successfully";
                }
            }catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Something Went Wrong";
            }

            return RedirectToAction("Details","Vehicle");
        }

        [HttpPost]
        public async Task<IActionResult> Delete (Guid id)
        {
            try
            {

                var vehicle = await dbContext.Vehicles.FindAsync(id);

                //var vehicle = await dbContext.Vehicles.FindAsync(viewModel.Id);

                if (vehicle is not null)
                {
                    dbContext.Vehicles.Remove(vehicle);
                    await dbContext.SaveChangesAsync();
                    TempData["Message"] = "Vehicle deleted successfully.";
                }
            }catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Something Went Wrong";
            }


                return RedirectToAction("Details", "Vehicle");
        }
    }
}
