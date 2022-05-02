using BlogLab.Models.Account;
using BlogLab.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace BlogLab.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly ITokenService _tokenService;
        private readonly UserManager<ApplicationUserIdentity> _userManager;
        private readonly SignInManager<ApplicationUserIdentity> _signInManager;

        public AccountController(
            ITokenService tokenService,
            UserManager<ApplicationUserIdentity> userManager,
            SignInManager<ApplicationUserIdentity> signInManager)
        {
            _tokenService = tokenService;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [HttpPost("register")]
        public async Task<ActionResult<ApplicationUser>> Register(ApplicationUserCreate applicationUserCreate)
        {
            var applicationUserIdentity = new ApplicationUserIdentity
            {
                Username = applicationUserCreate.UserName,
                Email = applicationUserCreate.Email,
                Fullname = applicationUserCreate.Fullname
            };

            var results = await _userManager.CreateAsync(applicationUserIdentity, applicationUserCreate.Password);

            if (results.Succeeded)
            {
                ApplicationUser userInfo = new ApplicationUser()
                {
                    ApplicationUserId = applicationUserIdentity.ApplicationUserId,
                    Username = applicationUserIdentity.Username,
                    Email = applicationUserIdentity.Email,
                    Fullname = applicationUserIdentity.Fullname,
                    Token = _tokenService.CreateToken(applicationUserIdentity)
                };
                return Ok(userInfo);
            }
            return BadRequest(results.Errors);
        }

        [HttpPost("login")]
        public async Task<ActionResult<ApplicationUser>> Login(ApplicationUserLogin applicationUserLogin)
        {
            var applicationUserIdentity = await _userManager.FindByNameAsync(applicationUserLogin.UserName);
            if (applicationUserIdentity != null)
            {
                var results = await _signInManager.CheckPasswordSignInAsync(applicationUserIdentity, applicationUserLogin.Password, false);
                if (results.Succeeded)
                {
                    ApplicationUser userInfo = new ApplicationUser()
                    {
                        ApplicationUserId = applicationUserIdentity.ApplicationUserId,
                        Username = applicationUserIdentity.Username,
                        Email = applicationUserIdentity.Email,
                        Fullname = applicationUserIdentity.Fullname,
                        Token = _tokenService.CreateToken(applicationUserIdentity)
                    };
                    return Ok(userInfo);
                }
            }
            return BadRequest("Invalid login attempt");
        }
    }
}
