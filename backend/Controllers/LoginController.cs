using backend.Domain.DTO;
using backend.Domain.Exceptions;
using backend.Domain.Interfaces;
using backend.Domain.ModelViews;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LoginController : ControllerBase
    {
        private readonly ILogger<LoginController> _logger;
        private readonly IUserServices _userServices;
        public LoginController(ILogger<LoginController> logger, IUserServices userServices) 
        {
            _logger = logger;
            _userServices = userServices;
        }


        [HttpPost]
        [AllowAnonymous]
        public IActionResult Login(LoginDTO loginDto)
        {
            var user = _userServices.GetUser(loginDto);

            var userView = _userServices.GetUserView(user);

            var token = _userServices.GenerateTokenJwt(userView);

            return Ok(new { token });
        }
    }

    
}
