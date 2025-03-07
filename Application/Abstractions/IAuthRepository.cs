using Domain.Enumerations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Abstractions
{
    public interface IAuthRepository
    {
        Task<string> RegisterAsync(string username, string email, string password, UserRole role);
        Task<string> LoginAsync(string email, string password);
    }

}
