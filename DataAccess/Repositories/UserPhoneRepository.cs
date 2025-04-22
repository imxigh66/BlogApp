using Application.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repositories
{
    public class UserPhoneRepository : IUserPhoneRepository
    {
        private readonly BlogDbContext _context;

        public UserPhoneRepository(BlogDbContext context)
        {
            _context = context;
        }

        public async Task<string> GetUserPhoneAsync(int userId)
        {
            var user = await _context.Users.FindAsync(userId);
            return user?.PhoneNumber;
        }
    }
}
