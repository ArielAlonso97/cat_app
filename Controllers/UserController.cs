using Azure.Core;
using cat_API.DB;
using cat_API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace cat_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private appDbContext _dbcontext;
        public UserController(appDbContext dbContext) {
       this._dbcontext = dbContext;
        }

        [HttpGet]
        [Authorize]
        [Route("GetUsers")]
        public async Task<IActionResult>  GetAllUsers()
        {
            var isAuthenticated = User.Identity!.IsAuthenticated;
           

            var users = await this._dbcontext.Users.ToListAsync();
            return Ok(users);
        }


        [HttpPost]
        [Route("CreateUser")]
        public async Task<NoContentResult>  CreateUser(UserModel user)
        {

            var passwordHasher = new PasswordHasher<UserModel>();

            string passwordHash = passwordHasher.HashPassword(user, user.Password);
            
            user.Password = passwordHash;
            _dbcontext.Users.Add(user);
            await _dbcontext.SaveChangesAsync();

            return NoContent();
                
        }


    }
}
