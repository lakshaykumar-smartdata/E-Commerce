using UserService.Data;

namespace UserService.Services
{
    public class UserService
    {
        private readonly UserServiceDbContext _dbContext;

        public UserService(UserServiceDbContext dbContext)
        {
            _dbContext = dbContext;
        }
    }
}