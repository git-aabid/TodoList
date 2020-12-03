using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;
using TODO.Contacts.Connection;

namespace TODO.Repo
{
    public class DatabaseConnection : IDatabaseConnection
    {
        private readonly IConfiguration _config;

        public DatabaseConnection(IConfiguration config)
        {
            _config = config;
        }

        public string GetConnection()
        {
            return _config.GetSection("AppSettings")["ConnectionString"];
        }
    }
}
