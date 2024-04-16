using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Runtime.InteropServices;
using System.Security.Claims;
using System.Text;
using WebAPI.DTO;
using WebAPI.Models;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> UserManager;
        private readonly IConfiguration configuration;

        public AccountController(UserManager<ApplicationUser>UserManager, IConfiguration configuration) 
        {
            this.UserManager = UserManager;
            this.configuration = configuration;
        }
        [HttpPost("Register")]
        public async Task<IActionResult> Registeration(RegisterUserDTO userDTO) 
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser {  UserName = userDTO.UserName, Email = userDTO.Email };
                IdentityResult result= await UserManager.CreateAsync(user, userDTO.Password);
                if (result.Succeeded) { return Ok("Account Added"); }
                else { return BadRequest(result.Errors.FirstOrDefault()); }
            
            }
            return BadRequest(ModelState);
        
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginUserDTO loginUserDTO)
        {
            if (ModelState.IsValid)
            {
                var ApplicationUser = await UserManager.FindByNameAsync(loginUserDTO.Username);
                if (ApplicationUser != null)
                {
                    bool found = await UserManager.CheckPasswordAsync(ApplicationUser, loginUserDTO.Password);
                    if (found)
                    {
                        // claims Token
                        var claims = new List<Claim>();
                        claims.Add(new Claim(ClaimTypes.Name, ApplicationUser.UserName));
                        claims.Add(new Claim(ClaimTypes.NameIdentifier, ApplicationUser.Id));
                        claims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));
                        // get role
                        var roles = await UserManager.GetRolesAsync(ApplicationUser);
                        foreach (var itemRole in roles)
                        {
                            claims.Add(new Claim(ClaimTypes.Role, itemRole));
                        }
                        SecurityKey securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Secret"]));
                        SigningCredentials signing = new SigningCredentials
                            (
                           securityKey, SecurityAlgorithms.HmacSha256
                            );
                        JwtSecurityToken token = new JwtSecurityToken
                            (
                            issuer: configuration["JWT:ValidIssuer"],//url provider web api
                            audience: configuration["JWT:ValidAudience"],// url consumer angular
                            claims: claims,
                            expires: DateTime.Now.AddHours(1),
                            signingCredentials: signing

                            );
                        return Ok(new
                        {
                            token = new JwtSecurityTokenHandler().WriteToken(token),
                            expiration = token.ValidTo
                        });
                    }
                    else { return Unauthorized(); }

                }
                return Ok();

            }
            else { return Unauthorized(); }

        }

    }
}
