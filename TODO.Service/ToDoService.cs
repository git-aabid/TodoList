using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TODO.Contacts.Repository;
using TODO.Contacts.Services;
using TODO.Models;
using TODO.Repo.Models;
using TODO.Service.DataMapper;

namespace TODO.Service
{
    public class ToDoService : ITodoService
    {
        private readonly IRepo<ToDo> _todoRepo;

        public ToDoService(IRepo<ToDo> todoRepo)
        {
            _todoRepo = todoRepo;
        }
        public async Task<int> Add(string todoName, int userId)
        {
            var todoEntity = new ToDo
            {
                TaskName = todoName,
                CreatedDate = DateTime.Now,
                UserId = userId
            };
            var res = await _todoRepo.Insert(todoEntity);
            
            return todoEntity.TaskId;
        }

        public async Task<bool> ChangeStatus(bool status, int todoId, int userId)
        {
            var task = await _todoRepo.Get(c => c.TaskId == todoId && c.UserId == userId);
            if(task!= null)
            {
                task.IsComplete = status;
                task.UpdatedDate = DateTime.Now;
            }
            var res = await _todoRepo.Update(task);
            return res > 0;
        }

        public async Task<List<TodoModel>> GetAll(int userId)
        {
            var tasks = new List<TodoModel>();
            var res =  await _todoRepo.GetAll(c => c.UserId == userId);
            foreach(var todo in res)
            {
                tasks.Add(todo.MapToModel());
            }

            return tasks;
        }

        public async Task<bool> Remove(int todoId, int userId)
        {
            var res = await _todoRepo.Remove(c => c.UserId == userId && c.TaskId == todoId);
            return res > 0;
        }
    }
}
