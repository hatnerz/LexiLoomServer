using LexiLoom.Database;

namespace LexiLoom.Services
{
    public class ServiceBase
    {
        protected readonly LexiLoomDbContext _context;

        public ServiceBase(LexiLoomDbContext context)
        {
            _context = context;
        }
    }
}
