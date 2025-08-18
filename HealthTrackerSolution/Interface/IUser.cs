using System.Collections.Generic;
using System.Threading.Tasks;
using HealthTrackerSolution.Model;

namespace HealthTrackerSolution.Interface
{
    public interface IUser
    {
        Task<IEnumerable<User>> GetAllAsync();
        Task<User?> GetByIdAsync(int id);
        Task AddAsync(User user);
        Task UpdateAsync(User user);    
        Task DeleteAsync(int id);
        Task<bool> UserExistsAsync(long phoneNumber);

        Task<bool> ComparePasswordAsync(long phoneNumber, string password); // Added method
    }
}
