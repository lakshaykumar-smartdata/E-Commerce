

using Microsoft.EntityFrameworkCore;

namespace OrderService.Data
{
    public class OrderServiceDbContext : DbContext
    {
        public OrderServiceDbContext(DbContextOptions<OrderServiceDbContext> options) : base(options) { }
    }
}
