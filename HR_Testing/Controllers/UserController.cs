using Hr_Testing.Data;
using Hr_Testing.Entities;
using Hr_Testing.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Hr_Testing.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController(
        Hr_TestingDbContext context,
        UserManager<IdentityUser> userManager) : ControllerBase
    {

        [HttpGet("User")]
        [EndpointSummary("GetUser")]
        public async Task<IActionResult> GetAsync()
        {
            var userData = await context.UserInfos.ToListAsync();
            if (userData == null || userData.Count == 0)
            {
                return NotFound(new DefaultResponseModel()
                {
                    Success = true,
                    Statuscode = StatusCodes.Status404NotFound,
                    Message = "User Data Not Found!",
                    Data = null
                });
            }

            return Ok(new DefaultResponseModel()
            {
                Success = true,
                Statuscode = StatusCodes.Status200OK,
                Message = "User Data retrieved successfully!",
                Data = userData
            });
        }

        [HttpPost]
        [EndpointSummary("Create user account")]
        public async Task<IActionResult> CreateAsync(UserInfoModel model)
        {
            IdentityUser user = new()
            {
                UserName = model.UserName,
                Email = model.Email,
                EmailConfirmed = !string.IsNullOrEmpty(model.Email),
                PhoneNumber = model.PhoneNumber,
                PhoneNumberConfirmed = !string.IsNullOrEmpty(model.PhoneNumber),
                SecurityStamp = Guid.NewGuid().ToString(),
            };

            IdentityResult result = await userManager.CreateAsync(user, model.Password);

            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            IdentityResult setRole = await userManager.AddToRoleAsync(user, model.RoleName);
            if (setRole.Succeeded)
            {

                UserInfo userInfo = new()
                {
                    UserId = user.Id,
                    FullName = model.FullName,
                    JoinDate = DateOnly.FromDateTime(DateTime.Now),
                    Status = true,
                    StateId = model.StateId,
                    TownshipId = model.TownshipId,
                    StreetId = model.StreetId,
                };
                context.UserInfos.Add(userInfo);
            }
            else
            {
                return BadRequest(result.Errors);
            }

            return await context.SaveChangesAsync() > 0
                ? Ok(new DefaultResponseModel()
                {
                    Success = true,
                    Statuscode = StatusCodes.Status200OK,
                    Message = "success",
                    Data = model
                })
                : BadRequest(new DefaultResponseModel()
                {
                    Success = false,
                    Statuscode = StatusCodes.Status400BadRequest,
                    Message = "error",
                    Data = null
                });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAsync(string id, UserInfoModel model)
        {
            if (id != model.UserId)
                return BadRequest("Route ID and Body UserId do not match");

            var userInfo = await context.UserInfos
                .FirstOrDefaultAsync(u => u.UserId == id);

            if (userInfo == null)
                return NotFound("UserInfo not found");

           
            var identityUser = await userManager.FindByIdAsync(id);
            if (identityUser == null)
                return NotFound("IdentityUser not found");

          
            identityUser.UserName = model.UserName;
            identityUser.Email = model.Email;
            identityUser.PhoneNumber = model.PhoneNumber;

            var updateResult = await userManager.UpdateAsync(identityUser);
            if (!updateResult.Succeeded)
                return BadRequest(updateResult.Errors);


            if (!string.IsNullOrWhiteSpace(model.Password))
            {
                var token = await userManager.GeneratePasswordResetTokenAsync(identityUser);
                var passwordResult = await userManager.ResetPasswordAsync(
                    identityUser, token, model.Password);

                if (!passwordResult.Succeeded)
                    return BadRequest(passwordResult.Errors);
            }

 
            var currentRoles = await userManager.GetRolesAsync(identityUser);
            await userManager.RemoveFromRolesAsync(identityUser, currentRoles);
            await userManager.AddToRoleAsync(identityUser, model.RoleName);

            userInfo.FullName = model.FullName;
            userInfo.Status = model.Status;
            userInfo.JoinDate = model.JoinDate;
            userInfo.StateId = model.StateId;
            userInfo.TownshipId = model.TownshipId;
            userInfo.StreetId = model.StreetId;
            userInfo.ProfileImage = model.ProfileImage;

            await context.SaveChangesAsync();

            return Ok(new DefaultResponseModel
            {
                Success = true,
                Statuscode = StatusCodes.Status200OK,
                Message = "User updated successfully",
                Data = userInfo
            });
        }


        [HttpDelete("{UserName}")]
        [EndpointSummary("Delete User acc")]
        public async Task<IActionResult> DeleteAsync(int UserName)
        {
            UserInfo? UserData = await context.UserInfos.FindAsync(UserName);

            if(UserData == null)
            {
                return NotFound(new DefaultResponseModel()
                {
                    Success = false,
                    Statuscode = StatusCodes.Status404NotFound,
                    Message = "error",
                    Data = null
                });
            }

            context.UserInfos.Remove(UserData);
            return await context.SaveChangesAsync() > 0
                ? Ok(new DefaultResponseModel()
                {
                    Success = true,
                    Statuscode = StatusCodes.Status200OK,
                    Message = "success",
                    Data = null
                })
                : BadRequest(new DefaultResponseModel()
                {
                    Success = false,
                    Statuscode = StatusCodes.Status400BadRequest,
                    Message = "error",
                    Data = null
                });
        }

    }
}
