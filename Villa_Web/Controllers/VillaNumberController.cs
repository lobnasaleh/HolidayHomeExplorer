using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System.Collections.Generic;
using Villa_Web.Models;
using Villa_Web.Models.DTO;
using Villa_Web.Models.VM;
using Villa_Web.Services;
using Villa_Web.Services.IServices;

namespace Villa_Web.Controllers
{
    public class VillaNumberController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IVillaNumberService villaNumberService;
        private readonly IVillaService villaService;


        public VillaNumberController(IMapper _mapper, IVillaNumberService villaNumberService, IVillaService villaService)
        {
            this._mapper = _mapper;
            this.villaNumberService = villaNumberService;
            this.villaService = villaService;
        }
        public async Task<IActionResult> Index()
        {
            List<VillaNumberDTO> lis = new List<VillaNumberDTO>();

            var response = await villaNumberService.GetAllAsync<APIResponse>();//betb3at lel client yendah 3ala el api
            if (response != null && response.IsSuccess)
            {
                lis = JsonConvert.DeserializeObject<List<VillaNumberDTO>>(Convert.ToString(response.Result));
            }

            return View("Index", lis);
        }

        [HttpGet]
        public async Task<IActionResult> CreateVillaNumber()
        {
            var response = await villaService.GetAllAsync<APIResponse>();
            VillaNumberCreateVM villaNumVm=new VillaNumberCreateVM();

            if (response != null && response.IsSuccess )
            {
                villaNumVm.villaList = JsonConvert.DeserializeObject<List<VillaDTO>>
                    (Convert.ToString(response.Result)).Select(i=>new SelectListItem
                    {
                        
                        Text = i.Name,
                        Value=i.Id.ToString()
                    });
            }
            return View(villaNumVm);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateVillaNumber(VillaNumberCreateVM ofromreq)
        {
            if (ModelState.IsValid)
            {

                var response = await villaNumberService.CreateAsync<APIResponse>(ofromreq.villaNumber);//betb3at lel client yendah 3ala el api
                if (response != null && response.IsSuccess)
                {
                    return RedirectToAction(nameof(Index));
                }
            }
            //invalid model
            return View(ofromreq);
        }
    }
}
