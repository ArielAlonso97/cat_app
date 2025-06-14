using cat_API.DB;
using cat_API.DTOs;
using cat_API.Models;
using cat_API.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace cat_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private IAuthService _authService;
       public AuthController(IAuthService authService) {
        this._authService = authService;
        }
        

        [HttpPost]
        public async Task<IActionResult> Login(AuthDTO user)
        {

            try
            {
               var token = await this._authService.Login(user);

                if (token == null)
                {
                    return Unauthorized();
                }
            return Ok(token);


            }
            catch (Exception err)
            {

                return BadRequest($"Error trying login: {err.Message}");
            }
         
        }
    }
}
