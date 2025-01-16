using System.Net;

namespace Villa_Web.Models
{
    public class APIResponse
    {
        public HttpStatusCode StatusCode { get; set; }
        public bool IsSuccess { get; set; }//=true;//unless changed in controller code
        public List<string> Errors { get; set; }
        public object Result { get; set; }
    }
}
