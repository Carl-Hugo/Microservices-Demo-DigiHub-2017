using Microservices.Users.Api.Contracts;
using System.Threading.Tasks;

namespace Microservices.Users.Write.Api
{
    public interface IUserService
    {
        Task<User> InsertAsync(User user);
        Task<User> UpdateAsync(User user);
        Task<User> DeleteAsync(string userId);
    }
}