﻿using Newtonsoft.Json;
using System.Text;
using Villa_Web.Models;
using Villa_Web.Services.IServices;
using VillaUtility;

namespace Villa_Web.Services
{
    public class BaseService : IBaseServicecs
    {
        public APIResponse responseModel { get; set; }

        public IHttpClientFactory httpClient { get; set; }//alreadu registered

        public BaseService(IHttpClientFactory httpClient)
        { //to call api

            responseModel = new APIResponse();
            this.httpClient = httpClient;
        }

        public async Task<T> SendAsync<T>(APIRequest apiRequest)//we need to call the api found in reuest
        {
            try
            {
                var client = httpClient.CreateClient("MagicAPI");
                HttpRequestMessage message = new HttpRequestMessage();//hahtag ahaded el message de hykoon feeha eh
                message.Headers.Add("Accept", "application/json");
                message.RequestUri = new Uri(apiRequest.URL);

                if (apiRequest.Data != null)
                { //law ha3ml create aw update fa el client hyb3at lel api data
                  //sa3etha hathtag a3ml serialize lel data 3shan ab3tha lel api
                    message.Content = new StringContent(JsonConvert.SerializeObject(apiRequest.Data), Encoding.UTF8, "application/json");

                }
                switch (apiRequest.APIType)
                {
                    case SD.APIType.POST:
                        message.Method = HttpMethod.Post;
                        break;
                    case SD.APIType.DELETE:
                        message.Method = HttpMethod.Delete;
                        break;
                    case SD.APIType.PUT:
                        message.Method = HttpMethod.Put;
                        break;
                    default:
                        message.Method = HttpMethod.Get;
                        break;

                }

               HttpResponseMessage apiResponse = null;
               apiResponse = await client.SendAsync(message); //add breakpoint here 

                //Read content and deserialize the result 3shan yeb2a ma3aya object 22dar ata3amel ma3ah
                var apiContent =await apiResponse.Content.ReadAsStringAsync();
                var APIresponse=JsonConvert.DeserializeObject<T>(apiContent);// <T> hya noo3 el data ely gaya fel response
                return APIresponse;
            }
            catch (Exception ex)
            {
                var dto = new APIResponse
                {
                    Errors = new List<string>() { Convert.ToString(ex.Message) },
                    IsSuccess = false,
                };
                //a3ml serialize wa deserialize 3shan a3araf araga3o ka response l2eny msh ha3raf arag3o l2eny mehtaga arga3 haga of Type T 
                var res=JsonConvert.SerializeObject(dto);
                var APIresponse=JsonConvert.DeserializeObject<T>(res);
                return APIresponse;

            }
        }
    }
}
