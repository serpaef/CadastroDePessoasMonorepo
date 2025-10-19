using backend.Domain.DTO;
using backend.Domain.Interfaces;
using backend.Domain.ModelViews;
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
        public IActionResult Login(LoginDTO loginDto)
        {
            _logger.LogInformation("/POST Login recebido" +  loginDto);

            var user = _userServices.GetUser(loginDto);

            if (user == null)
            {
                return Unauthorized();
            }

            var userView = _userServices.GetUserView(user);

            return Ok(userView);
        }
    }

    
}
