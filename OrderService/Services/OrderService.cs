using OrderService.Data;

namespace OrderService.Services
{
    public class OrderService : IOrderService
    {
        private readonly OrderServiceDbContext _dbContext;

        public OrderService(OrderServiceDbContext dbContext)
        {
            _dbContext = dbContext;
        }
    }
}
