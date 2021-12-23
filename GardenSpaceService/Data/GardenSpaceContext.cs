using GardenSpaceService.Model;
using Microsoft.EntityFrameworkCore;

namespace GardenSpaceService.Data
{
    public class GardenSpaceContext : DbContext
    {
        public GardenSpaceContext(DbContextOptions<GardenSpaceContext> options) : base(options)
        {

        }

        public DbSet<GardenSpace> GardenSpace { get; set; }
        public DbSet<GardenSpaceUserMap> GardenSpaceUserMap { get; set; }
        public DbSet<GardenBranchType> GardenBranchType { get; set; }
        public DbSet<GardenRootType> GardenRootType { get; set; }
    }
}
