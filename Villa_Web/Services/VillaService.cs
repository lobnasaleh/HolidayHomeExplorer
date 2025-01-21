using Villa_Web.Models;
using Villa_Web.Models.DTO;
using Villa_Web.Services.IServices;
using VillaUtility;
using static System.Net.WebRequestMethods;

namespace Villa_Web.Services
{
    public class VillaService:BaseService,IVillaService
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly IConfiguration _configuration;
       // private string villaUrl;

        public VillaService(IHttpClientFactory clientFactory, IConfiguration _configuration) : base(clientFactory)//, IConfiguration configuration
        {
            _clientFactory = clientFactory;
           this._configuration = _configuration;

        }

        public async Task<T> CreateAsync<T>(VillaCreateDTO entity, string token)
        {
           APIRequest request = new APIRequest() { 
               
               APIType=SD.APIType.POST,
               Data=entity,
               URL= _configuration["ServiceUrls:VillaAPI"] + "/api/VillaApi",
               Token=token

           };
            return await SendAsync<T>(request);
        }

        public async Task<T> DeleteAsync<T>(int id, string token)
        {
            APIRequest request = new APIRequest()
            {
                APIType = SD.APIType.DELETE,
                URL = _configuration["ServiceUrls:VillaAPI"] + "/api/VillaApi/"+id,
               Token = token

            };
            return await SendAsync<T>(request);
        }

        public async Task<T> GetAllAsync<T>(string token)
        {
            APIRequest request = new APIRequest()
            {
                APIType = SD.APIType.GET,
                URL = _configuration["ServiceUrls:VillaAPI"] + "/api/VillaApi",
                Token = token

            };
            return await  SendAsync<T>(request);
        }

        public async Task<T> GetAsync<T>(int id, string token)
        {
            APIRequest request = new APIRequest()
            {
                APIType = SD.APIType.GET,
                URL = _configuration["ServiceUrls:VillaAPI"] + "/api/VillaApi/" + id,
                Token = token

            };
            return await SendAsync<T>(request);
        }

        public async Task<T> UpdateAsync<T>(VillaUpdateDTO entity, string token)
        {
            APIRequest request = new APIRequest()
            {
                APIType = SD.APIType.PUT,
                Data = entity,
                URL = _configuration["ServiceUrls:VillaAPI"] + "/api/VillaApi/" + entity.Id,
                Token = token

            };
            return await SendAsync<T>(request);
        }
    }
}
