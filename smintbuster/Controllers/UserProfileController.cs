using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using smintbuster.Modals;

namespace smintbuster.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserProfileController : ControllerBase
    {
        private readonly AuthenticationContext _context;
        private UserManager<ApplicationUser> _userManager;
        public UserProfileController(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        [HttpGet]
        [Authorize]
        //GET : /api/UserProfile
        public async Task<Object> GetUserProfile()
        {
            string userId = User.Claims.First(c => c.Type == "UserID").Value;
            var user = await _userManager.FindByIdAsync(userId);
            return new
            {
                user.Id,
                user.NickName,
                user.FirstName,
                user.LastName,
                user.Email,
                user.UserType
            };
        }

        // PUT: api/UserProfile/5
        [HttpPut("{id}")]
        [Authorize]
        public async Task<Object> PutCategoryModel([FromRoute] string id, [FromBody] ApplicationUser userModel)
        {

            if (userModel.Id == null || userModel.FirstName == null || userModel.LastName == null || userModel.NickName == null
                || userModel.Email == null || userModel.UserType == null)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var user = await _userManager.FindByIdAsync(id);
                user.UserType = userModel.UserType;
                user.NickName = userModel.NickName;
                user.FirstName = userModel.FirstName;
                user.LastName = userModel.LastName;
                user.Email = userModel.Email;
                user.UserName = userModel.NickName;
                await _userManager.UpdateAsync(user);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserModelExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return new
            {
                userModel.Id,
                userModel.NickName,
                userModel.FirstName,
                userModel.LastName,
                userModel.Email,
                userModel.UserType
            };
        }

        private bool UserModelExists(string id)
        {
            return _context.ApplicationUsers.Any(e => e.Id == id);
        }
    }
}