using Data.Abstract;
using Data.Core;
using Data.Repos;
using Microsoft.AspNetCore.Mvc;
using Model;
using System.Collections.Generic;
using System.Linq;

namespace DotNetCoreWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        IUserDataRepo _userDataRepo;

        public UserController(IUserDataRepo userDataRepo)
        {
            _userDataRepo = userDataRepo;
        }


        [HttpGet]
        [Route("GetUsers")]
        public ActionResult<List<User>> GetUsers()
        {
            return _userDataRepo.GetAll();
        }

        
        [HttpPost]
        [Route("SaveUser")]
        public ActionResult<DataResult<User>> SaveUser([FromBody] User user)
        {
            return _userDataRepo.Insert(user);
        }

        [HttpPost]
        [Route("UpdateUser")]
        public ActionResult<DataResult<User>> UpdateUser([FromBody] User user)
        {
            return _userDataRepo.Update(user);

        }
    }
}
