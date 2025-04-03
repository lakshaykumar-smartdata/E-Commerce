

using Microsoft.EntityFrameworkCore;
using OrderService.Models;

namespace OrderService.Data
{
    public class OrderServiceDbContext : DbContext
    {
        public OrderServiceDbContext(DbContextOptions<OrderServiceDbContext> options) : base(options) { }
        public DbSet<Order> Orders { get; set; }
    }
}
