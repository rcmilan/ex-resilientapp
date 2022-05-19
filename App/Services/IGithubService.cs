using App.Models;

namespace App.Services
{
    public interface IGithubService
    {
        Task<User> GetUser(string username);
        Task<List<User>> GetUsers(string orgName);
    }
}
