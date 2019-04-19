using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NG_Core_Auth.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace NG_Core_Auth.Controllers
{
    [Route("api/[controller]")]
    public class AccountController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;

        public AccountController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager )
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [HttpPost("action")]
        public async Task<IActionResult> Register([FromBody] RegisterViewModel formData) 
        {
            //Will hold all the erros
            List<string> errorList = new List<string>();

            var user = new IdentityUser()
            {
                Email = formData.Email,
                UserName = formData.Username,
                SecurityStamp = Guid.NewGuid().ToString()
            };

            var result = await _userManager.CreateAsync(user, formData.Password);

            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, "Customer");

                //Sending Confirmation Email


                return Ok(new { username = user.UserName, email = user.Email, status = 1, message = "Registration was Successful" });

            }
            else
            {
               foreach (var er in result.Errors)
                {
                    ModelState.AddModelError("", er.Description);
                    errorList.Add(er.Description);
                }
                return BadRequest(new JsonResult(errorList));
            }

        }
    }
}
