using Data.Abstract;
using Data.Concrete;
using Data.Core;
using Microsoft.Extensions.Configuration;
using Model;
using System;
using System.Collections.Generic;
using System.Text;
using Utilities;

namespace Data.Repos
{
    public class UserDataRepo : DapperSqlServerBaseData<User>, IUserDataRepo 
    {
        public UserDataRepo(IConfiguration config) : base(config)
        {
        }
    }
}
