using Villa_Web.Models;
using Villa_Web.Models.DTO;
using Villa_Web.Services.IServices;
using VillaUtility;
using static System.Net.WebRequestMethods;

namespace Villa_Web.Services
{
    public class VillaNumberService:BaseService,IVillaNumberService
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly IConfiguration _configuration;
       // private string villaUrl;

        public VillaNumberService(IHttpClientFactory clientFactory, IConfiguration _configuration) : base(clientFactory)//, IConfiguration configuration
        {
            _clientFactory = clientFactory;
           this._configuration = _configuration;

        }

        public async Task<T> CreateAsync<T>(VillaNumberCreateDTO entity, string token)
        {
           APIRequest request = new APIRequest() { 
               
               APIType=SD.APIType.POST,
               Data=entity,
               URL= _configuration["ServiceUrls:VillaAPI"] + "/api/VillaNumberApi",
               Token = token

           };
            return await SendAsync<T>(request);
        }

        public async Task<T> DeleteAsync<T>(int id,string token)
        {
            APIRequest request = new APIRequest()
            {
                APIType = SD.APIType.DELETE,
                URL = _configuration["ServiceUrls:VillaAPI"] + "/api/VillaNumberApi/" + id,
                Token = token

            };
            return await SendAsync<T>(request);
        }

        public async Task<T> GetAllAsync<T>(string token)
        {
            APIRequest request = new APIRequest()
            {
                APIType = SD.APIType.GET,
                URL = _configuration["ServiceUrls:VillaAPI"] + "/api/VillaNumberApi",
                Token = token

            };
            return await  SendAsync<T>(request);
        }

        public async Task<T> GetAsync<T>(int id, string token)
        {
            APIRequest request = new APIRequest()
            {
                APIType = SD.APIType.GET,
                URL = _configuration["ServiceUrls:VillaAPI"] + "/api/VillaNumberApi/" + id,
                Token = token

            };
            return await SendAsync<T>(request);
        }

        public async Task<T> UpdateAsync<T>(VillaNumberUpdateDTO entity, string token)
        {
            APIRequest request = new APIRequest()
            {
                APIType = SD.APIType.PUT,
                Data = entity,
                URL = _configuration["ServiceUrls:VillaAPI"] + "/api/VillaNumberApi/" + entity.VillaNo,
                Token = token

            };
            return await SendAsync<T>(request);
        }
    }
}
