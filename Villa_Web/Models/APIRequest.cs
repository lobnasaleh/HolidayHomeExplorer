

using static VillaUtility.SD;

namespace Villa_Web.Models
{
    public class APIRequest
    {
        public APIType APIType { get; set; }=APIType.GET;

        public string URL { get; set; }//el url ely by3ml 3aleeh request

        public object Data { get; set; }//law post request hab3at data

        public string Token { get; set; }
    }
}
