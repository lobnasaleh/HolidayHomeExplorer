using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections.Generic;
using Villa_Web.Models;
using Villa_Web.Models.DTO;
using Villa_Web.Services.IServices;
using VillaUtility;

namespace Villa_Web.Controllers
{
    public class VillaController : Controller
    {
        private readonly IVillaService villaService;
        private readonly IMapper mapper;
        public VillaController(IVillaService villaService, IMapper mapper)
        {
            this.villaService = villaService;
            this.mapper = mapper;
        }
        public async Task<IActionResult> Index()
        {
            List<VillaDTO> lis = new List<VillaDTO>();

            var response =  await villaService.GetAllAsync<APIResponse>(HttpContext.Session.GetString(SD.SessionToken));//betb3at lel client yendah 3ala el api
          if (response!=null && response.IsSuccess)
            {
                lis = JsonConvert.DeserializeObject<List<VillaDTO>>(Convert.ToString(response.Result));
            }
            
            return View("Index",lis);
        }
        [HttpGet]
        [Authorize(Roles="admin") ]
        public async Task<IActionResult> CreateVilla()
        {

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> CreateVilla(VillaCreateDTO ofromreq)
        {
            if (ModelState.IsValid) {

                var response = await villaService.CreateAsync<APIResponse>(ofromreq, HttpContext.Session.GetString(SD.SessionToken));//betb3at lel client yendah 3ala el api
                if (response != null && response.IsSuccess)
                {
                    TempData["success"] = "Villa Created Successfully";

                    return RedirectToAction(nameof(Index));
                }
            }
            //invalid model
            TempData["error"] = "Error encountered";

            return View(ofromreq);
        }
        [HttpGet]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> UpdateVilla(int villaId)
        {
            var response = await villaService.GetAsync<APIResponse>(villaId,HttpContext.Session.GetString(SD.SessionToken));
            if (response != null && response.IsSuccess)
            {
                //deserialize y3ny ahawel el json l c# object we henna haddedet noo3 el object ely 3yzah eno villadto
                VillaDTO model =JsonConvert.DeserializeObject<VillaDTO>(Convert.ToString(response.Result));
                return View(mapper.Map<VillaUpdateDTO>(model));
            }

            return NotFound();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> UpdateVilla(VillaUpdateDTO ofromreq)
        {
            if (ModelState.IsValid)
            {

                var response = await villaService.UpdateAsync<APIResponse>(ofromreq, HttpContext.Session.GetString(SD.SessionToken));//betb3at lel client yendah 3ala el api
                if (response != null && response.IsSuccess)
                {
                    TempData["success"] = "Villa Updated Successfully";

                    return RedirectToAction(nameof(Index));
                }
            }
            //invalid model
            TempData["error"] = "Error encountered";

            return View(ofromreq);
        }

        [HttpGet]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> DeleteVilla(int villaId)
        {
            var response = await villaService.GetAsync<APIResponse>(villaId, HttpContext.Session.GetString(SD.SessionToken));
            if (response != null && response.IsSuccess)
            {
                //deserialize y3ny ahawel el json l c# object we henna haddedet noo3 el object ely 3yzah eno villadto
                VillaDTO model = JsonConvert.DeserializeObject<VillaDTO>(Convert.ToString(response.Result));
                return View(model);//mesh mehtaga mapper l2eny msh ba3ml create waala update
            }

            return NotFound();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
         [Authorize(Roles="admin") ]
        public async Task<IActionResult> DeleteVilla(VillaDTO ofromreq)
        {
            

                var response = await villaService.DeleteAsync<APIResponse>(ofromreq.Id, HttpContext.Session.GetString(SD.SessionToken));//betb3at lel client yendah 3ala el api
                if (response != null && response.IsSuccess)
                {
                TempData["success"] = "Villa Deleted Successfully";

                return RedirectToAction(nameof(Index));
                }

            //invalid model
            TempData["error"] = "Error encountered";

            return View(ofromreq);
        }
    }
}
