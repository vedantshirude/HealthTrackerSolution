using HealthTrackerSolution.DataContext;
using HealthTrackerSolution.Interface;
using HealthTrackerSolution.Model;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HealthTrackerSolution.Repository
{
    public class UserRepository : IUser
    {
        private readonly dataContext _context;

        public UserRepository(dataContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<User>> GetAllAsync()
        {
            return await _context.Users.ToListAsync();
        }

        public async Task<User?> GetByIdAsync(int id)
        {
            return await _context.Users.FindAsync(id);
        }

        public async Task AddAsync(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(User user)
        {
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user != null)
            {
                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> UserExistsAsync(long phoneNumber)
        {
            return await _context.Users.AnyAsync(u => u.PhoneNumber == phoneNumber);
        }

        public async Task<bool> ComparePasswordAsync(long phoneNumber, string password)
        {
            var user = await _context.Users.SingleOrDefaultAsync(u => u.PhoneNumber == phoneNumber);
            if (user == null) return false;
            return user.Password == password; // Assuming password is stored in plain text, which it's not recommended to do
        }
    }
}