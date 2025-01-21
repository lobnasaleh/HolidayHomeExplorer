using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Diagnostics;
using Villa_Web.Models;
using Villa_Web.Models.DTO;
using Villa_Web.Services.IServices;
using VillaUtility;

namespace Villa_Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IVillaService villaService;
        private readonly IMapper mapper;
        public HomeController(IVillaService villaService, IMapper mapper)
        {
            this.villaService = villaService;
            this.mapper = mapper;
        }
        public async Task<IActionResult> Index()
        {
            List<VillaDTO> lis = new List<VillaDTO>();

            var response = await villaService.GetAllAsync<APIResponse>(HttpContext.Session.GetString(SD.SessionToken));//betb3at lel client yendah 3ala el api
            if (response != null && response.IsSuccess)
            {
                lis = JsonConvert.DeserializeObject<List<VillaDTO>>(Convert.ToString(response.Result));
            }

            return View("Index", lis);
        }
    }
}
