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
                //Console.WriteLine(ex);
                //Console.ReadLine();
            }

            return View();
            //return RedirectToAction("Details", "Vehicle");
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
