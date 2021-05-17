using Microsoft.EntityFrameworkCore;

namespace DB_s2_1_1.EntityModels
{
    public class ManualRawSqlContext : DbContext
    {
        public ManualRawSqlContext(DbContextOptions<ManualRawSqlContext> options) : base(options)
        {
        }
    }
}
