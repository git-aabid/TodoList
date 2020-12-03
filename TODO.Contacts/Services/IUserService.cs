using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TODO.Models;

namespace TODO.Contacts.Services
{
    public interface IUserService
    {
        Task<UserModel> Login(UserModel user);
        Task<UserModel> GetUserByid(int userId);
    }
}
