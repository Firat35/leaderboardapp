using Api.Models;

namespace Api.Repositories
{
    public interface IUserRepository
    {
        Task<List<User>> GetAllAsync();
        Task<bool> UpdateManyAsync(List<User> userList);
        Task<User> GetByIdAsync(string id);
        Task<User> AddAsync(User user);
        Task AddRangeAsync(List<User> userList);
    }
}
