using eCommerceApp.Application.DTOs.Identity;
using eCommerceApp.Application.Services.Interfaces.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Web;

namespace eCommerceApp.Host.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController(IAuthenticationService authenticationService) : ControllerBase
    {
        [HttpPost("create")]
        public async Task<IActionResult> CreateUser(CreateUser user)
        {
            var result = await authenticationService.CreateUser(user);
            return result.Success ? Ok(result) : BadRequest(result);  
        }
        
        [HttpPost("login")]
        public async Task<IActionResult> LoginUser(LoginUser user)
        {
            var result = await authenticationService.LoginUser(user);
            return result.Success ? Ok(result) : BadRequest(result);  
        }
        
        [HttpGet("refreshToken/{refreshToken}")]
        public async Task<IActionResult> ReviveUser(string refreshToken)
        {
            var result = await authenticationService.ReviveUser(HttpUtility.UrlDecode(refreshToken));
            return result.Success ? Ok(result) : BadRequest(result);  
        }
    }
}
