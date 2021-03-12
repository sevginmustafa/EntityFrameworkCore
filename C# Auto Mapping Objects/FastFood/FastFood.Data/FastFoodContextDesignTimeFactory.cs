using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace FastFood.Data
{
    public class FastFoodContextDesignTimeFactory : IDesignTimeDbContextFactory<FastFoodContext>
    {
        FastFoodContext IDesignTimeDbContextFactory<FastFoodContext>.CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<FastFoodContext>();
            builder.UseSqlServer(@"Server =.\SQLEXPRESS; Database = FastFood; Trusted_Connection = True; MultipleActiveResultSets = true");

            return new FastFoodContext(builder.Options);
        }
    }
}
