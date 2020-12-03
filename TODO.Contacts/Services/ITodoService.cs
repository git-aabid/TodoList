using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TODO.Models;

namespace TODO.Contacts.Services
{
    public interface ITodoService
    {
        Task<int> Add(string todoName, int userId);
        Task<bool> Remove(int todoId, int userId);
        Task<List<TodoModel>> GetAll(int userId);
        Task<bool> ChangeStatus(bool status, int todoId, int userId);
    }
}
