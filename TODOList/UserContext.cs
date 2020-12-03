using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TODOList
{
    public class UserContext
    {

        private readonly IHttpContextAccessor _contextAccessor;

        public UserContext(IHttpContextAccessor contextAccessor)
        {
            _contextAccessor = contextAccessor;
        }
        public int UserId
        {
            get
            {
                return int.Parse(_contextAccessor.HttpContext.Items["userId"].ToString());
            }
            set
            {
                _contextAccessor.HttpContext.Items.Add("userId", value);
            }
        }
    }
}
