using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Sunday.Repository.Api.Data
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        public UserRepository(DbContext dbDbContext) : base(dbDbContext)
        {
        }

        public override Task<User> FirstOrDefaultAsync()
        {
            return default;
        }
    }

    public interface IUserRepository : IRepository<User>
    {
    }
}