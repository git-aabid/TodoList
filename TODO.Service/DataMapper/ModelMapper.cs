using System;
using System.Collections.Generic;
using System.Text;
using TODO.Models;
using TODO.Repo.Models;

namespace TODO.Service.DataMapper
{
 //TODO:move to common
    public static class ModelMapper
    {
        public static UserModel MapToModel(this User user)
        {
            return new UserModel
            {
                UserName = user.UserName,
                UserId = user.UserId
            };
        }

        public static TodoModel MapToModel(this ToDo todo)
        {
            return new TodoModel
            {
                UserId = todo.UserId,
                CreatedDate = todo.CreatedDate,
                IsComplete=todo.IsComplete,
                TaskId = todo.TaskId,
                UpdatedDate = todo.UpdatedDate,
                TaskName = todo.TaskName
            };
        }
    }
}
