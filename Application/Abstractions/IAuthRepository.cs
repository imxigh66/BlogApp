using Application.Authorization;
using Domain.Enumerations;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Abstractions
{
    public interface IAuthRepository
    {
        Task<AuthResult> RegisterAsync(string username, string email,string phoneNumber, string password, UserRole? role);
        Task<AuthResult> LoginAsync(string email, string password);
        Task<User> GetUserByIdAsync(int id);
        Task<bool> UpdateUserAsync(User user);
    }

}
