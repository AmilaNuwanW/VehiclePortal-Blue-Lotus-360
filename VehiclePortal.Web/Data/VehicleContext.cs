using Microsoft.EntityFrameworkCore;
using VehiclePortal.Web.Models.Entities;

namespace VehiclePortal.Web.Data
{
    public class VehicleContext : DbContext
    {
        public VehicleContext(DbContextOptions<VehicleContext> options) :base(options)
        { 
        }

        public DbSet<Vehicle> Vehicles { get; set; }
    }
}
