using Azure;
using MagicVillaApI2.Models.DTO;
using MagicVillaApI2.Repositories.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.IdentityModel.Tokens;
using System.Net;

namespace MagicVillaApI2.Controllers
{
    [Route("api/UsersAuth")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        protected APIResponse _response;
        public UsersController(IUserRepository _userRepository)
        {
            this._userRepository = _userRepository;
            _response = new APIResponse();
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequestDTO model )
        {
            var loginResponse=await _userRepository.Login(model);

            if (loginResponse.User == null || string.IsNullOrEmpty(loginResponse.Token))
            {
                _response.StatusCode=HttpStatusCode.BadRequest;
                _response.IsSuccess=false;
                _response.Errors = new List<string> { "Username or Password is Incorrect" };
                return BadRequest(_response);
            }
            _response.Result = loginResponse;
            _response.IsSuccess = true;
            _response.StatusCode = HttpStatusCode.OK;
            return Ok(_response);
            
              
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterationRequestDTO model)
        {
            bool isUniqueUser = _userRepository.IsUniqueUser(model.UserName);
            if (isUniqueUser) { 

            var User= await _userRepository.Register(model);

                if (User == null) {
                    _response.Errors = new List<string> { "Error While registering" };
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }
                 _response.IsSuccess = true;
                _response.StatusCode = HttpStatusCode.OK;
                _response.Result=User;
                return Ok(_response);

            }
            else
            {
                _response.Errors = new List<string> { "Username is already taken" };
                _response.IsSuccess = false;
                _response.StatusCode = HttpStatusCode.BadRequest;
                return BadRequest(_response);
            }
        }
    }
}
