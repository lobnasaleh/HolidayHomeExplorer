using Villa_Web.Models;
using Villa_Web.Models.DTO;
using Villa_Web.Services.IServices;
using VillaUtility;

namespace Villa_Web.Services
{
    public class AuthService : BaseService, IAuthService
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly IConfiguration _configuration;
        // private string villaUrl;

        public AuthService(IHttpClientFactory clientFactory, IConfiguration _configuration) : base(clientFactory)//, IConfiguration configuration
        {
            _clientFactory = clientFactory;
            this._configuration = _configuration;

        }

        public async Task<T> LoginAsync<T>(LoginRequestDTO obj)
        {
            APIRequest request = new APIRequest()
            {

                APIType = SD.APIType.POST,
                Data = obj,
                URL = _configuration["ServiceUrls:VillaAPI"] + "/api/UsersAuth/login"
            };
            return await SendAsync<T>(request);
        }

        public async Task<T> RegisterAsync<T>(RegisterationRequestDTO obj)
        {
            APIRequest request = new APIRequest()
            {

                APIType = SD.APIType.POST,
                Data = obj,
                URL = _configuration["ServiceUrls:VillaAPI"] + "/api/UsersAuth/register"
            };
            return await SendAsync<T>(request);
        }
    }
}
