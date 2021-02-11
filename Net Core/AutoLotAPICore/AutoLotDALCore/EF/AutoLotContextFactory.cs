using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.Diagnostics;
namespace AutoLotDALCore.EF
{
    class AutoLotContextFactory : IDesignTimeDbContextFactory<AutoLotContext>
    {
        public AutoLotContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<AutoLotContext>();
            optionsBuilder.UseSqlServer(ConnectionStrings.ConnectionStr,
                options => options.EnableRetryOnFailure())
                .ConfigureWarnings(warn => warn.Throw(RelationalEventId
                .QueryPossibleUnintendedUseOfEqualsWarning));
            // EventId only for fun because it's about log this event

            return new AutoLotContext(optionsBuilder.Options);
        }
    }
}
