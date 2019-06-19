using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using smintbuster.Modals;

namespace smintbuster.Controllers
{
    [Route("api")]
    [ApiController]
    public class ApplicationUserController : ControllerBase
    {
        private readonly AuthenticationContext _context;
        private UserManager<ApplicationUser> _userManager;
        private SignInManager<ApplicationUser> _signInManager;
        private readonly ApplicationSettings _appSettings;

        public ApplicationUserController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IOptions<ApplicationSettings> appSettings)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _appSettings = appSettings.Value;
        }

        // GET: api/Category
        [HttpGet]
        [Authorize]
        [Route("users")]
        public IEnumerable<ApplicationUser> GetAllUsers()
        {
            var users = _userManager.Users;
            return users;
        }


        // DELETE: api//5
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteUserModel([FromRoute] string id)
        {
            var categoryModel = await _userManager.Users.Where(p => p.Id == id)
                .Select(p => p).FirstAsync();
            _userManager.DeleteAsync(categoryModel);
            if (categoryModel == null)
            {
                return NotFound();
            }

            return Ok(categoryModel);
        }

        [HttpPost]
        [Route("register")]
        //POST : /api/register
        public async Task<Object> PostApplicationUser(ApplicationUserModel model)
        {
            var applicationUser = new ApplicationUser()
            {
                NickName = model.NickName,
                Email = model.Email,
                FirstName = model.FirstName,
                LastName = model.LastName,
                UserType = model.UserType
            };
            applicationUser.UserName = model.NickName;

            try
            {
                var result = await _userManager.CreateAsync(applicationUser, model.Password);
                return Ok(result);
            } catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost]
        [Route("login")]
        //POST :  /api/login
        public async Task<IActionResult> Login(LoginModel model)
        {
            var user = await _userManager.FindByNameAsync(model.NickName);
            if(user != null && await _userManager.CheckPasswordAsync(user, model.Password))
            {
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new Claim[] {
                        new Claim("UserID", user.Id.ToString())
                    }),
                    Expires = DateTime.UtcNow.AddDays(1),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_appSettings.JWT_Secret)), SecurityAlgorithms.HmacSha256Signature)
                    };
                var tokenHandler = new JwtSecurityTokenHandler();
                var securityToken = tokenHandler.CreateToken(tokenDescriptor);
                var token = tokenHandler.WriteToken(securityToken);
                return Ok(new { token });
            }
             else
            {
                return BadRequest(new { message = "Username or password is not correct" });
            }
        }

    }
}