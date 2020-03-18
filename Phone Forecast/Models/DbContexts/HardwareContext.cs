using Microsoft.EntityFrameworkCore;

namespace Phone_Forecast.Models.DbContexts
{
    public class HardwareContext : DbContext
    {
        public HardwareContext(DbContextOptions<HardwareContext> options) : base(options) { }

        public DbSet<Hardware> HardwareConfigurations { get; set; }
    }
}
