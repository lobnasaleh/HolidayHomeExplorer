using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Security.Claims;
using Villa_Web.Models;
using Villa_Web.Models.DTO;
using Villa_Web.Services.IServices;
using VillaUtility;

namespace Villa_Web.Controllers
{
    public class AuthController : Controller
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService _authService)
        {
            this._authService = _authService;
        }
        [HttpGet]
        public IActionResult Login()
        {
            LoginRequestDTO loginRequestDTO = new LoginRequestDTO();
         return View(loginRequestDTO);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task< IActionResult> Login( LoginRequestDTO obj)
        {
          APIResponse response=  await _authService.LoginAsync<APIResponse>(obj);
            if (response!=null && response.IsSuccess)
            {
                LoginResponseDTO model= JsonConvert.DeserializeObject<LoginResponseDTO>(Convert.ToString(response.Result));
                //tell httpcontext that user signed in 3shan msh kol shwaya yerg3ny 3ala el signin

                var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
                identity.AddClaim(new Claim(ClaimTypes.Name,model.User.UserName));
                identity.AddClaim(new Claim(ClaimTypes.Role, model.User.Role));

                var principal=new ClaimsPrincipal(identity);
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,principal);


                //ba3d ma reg3ly el user wa el token 3yza a3mel store lel token fel session
                HttpContext.Session.SetString(SD.SessionToken, model.Token);
                //                               'JWTToken'
                return RedirectToAction("Index","Home");

            }
            else
            {
                ModelState.AddModelError("CustomError",response.Errors.FirstOrDefault());
                return View(obj);
            }
           
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task< IActionResult> Register(RegisterationRequestDTO obj)
        {
          APIResponse res=  await _authService.RegisterAsync<APIResponse>(obj);
            if (res.IsSuccess && res!=null) { 
              return RedirectToAction("Login");
            }
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> Logout() { 
        
            await HttpContext.SignOutAsync();
            HttpContext.Session.SetString(SD.SessionToken, "");
            return RedirectToAction("Index","Home");
        }
        [HttpGet]
        public IActionResult AccessDenied()
        {

            return View();
        }

    }
}
