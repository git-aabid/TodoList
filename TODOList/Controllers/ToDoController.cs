using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TODO.Contacts.Services;
using TODO.Models;
using Newtonsoft.Json;

namespace TODOList.Controllers
{
    [Authorize]
    public class ToDoController : Controller
    {
        private readonly ITodoService _todoService;
        private readonly UserContext _userContext;
        public ToDoController(ITodoService todoService, UserContext userContext)
        {
            _todoService = todoService;
            _userContext = userContext;
        }

        [HttpPost]
        public async Task<int> Create(TodoModel model)
        {
            return await _todoService.Add(model.TaskName, _userContext.UserId);
        }

        [HttpPost]
        public async Task<bool> ChangeStatus(StatusModel model)
        {
            return await _todoService.ChangeStatus(model.Status, model.TaskId, _userContext.UserId);
        }

        [HttpGet]
        public async Task<JsonResult> List()
        {
            return Json(await _todoService.GetAll(_userContext.UserId));
        }
        [HttpPost]
        public async Task<bool> Remove(int taskId)
        {
            return await _todoService.Remove(taskId, _userContext.UserId);
        }
    }
}
