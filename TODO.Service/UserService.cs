using System;
using System.Threading.Tasks;
using TODO.Contacts.Repository;
using TODO.Contacts.Services;
using TODO.Models;
using TODO.Repo.Models;
using TODO.Service.DataMapper;

namespace TODO.Service
{
    public class UserService : IUserService
    {
        private readonly IRepo<User> _userRepo;

        public UserService(IRepo<User> userRepo)
        {
            _userRepo = userRepo;
        }

        public async Task<UserModel> GetUserByid(int userId)
        {
            var user =  await _userRepo.Get(c=>c.UserId == userId);

            return user?.MapToModel();
        }

        public async Task<UserModel> Login(UserModel user)
        {
            var res = await _userRepo.Get(c => c.UserName == user.UserName && c.Password == user.Password);
            return res?.MapToModel();
        }

     
    }
}
