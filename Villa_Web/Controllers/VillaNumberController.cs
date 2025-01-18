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
            VillaNumberCreateVM villaNumVm = new VillaNumberCreateVM();

            if (response != null && response.IsSuccess)
            {
                villaNumVm.villaList = JsonConvert.DeserializeObject<List<VillaDTO>>
                    (Convert.ToString(response.Result)).Select(i => new SelectListItem
                    {

                        Text = i.Name,
                        Value = i.Id.ToString()
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
                    TempData["success"] = "Villa Created Successfully";
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    if (response.Errors.Count > 0)
                    {
                        ModelState.AddModelError("Errors", response.Errors.FirstOrDefault());

                    }
                }

                //3EDTHA TANY 3SHAN LAW HASAL ERROR 
                var resp = await villaService.GetAllAsync<APIResponse>();


                if (resp != null && resp.IsSuccess)
                {
                    ofromreq.villaList = JsonConvert.DeserializeObject<List<VillaDTO>>
                        (Convert.ToString(resp.Result)).Select(i => new SelectListItem
                        {

                            Text = i.Name,
                            Value = i.Id.ToString()
                        });
                }

            }
            //invalid model
            TempData["error"] = "Error encountered";

            return View(ofromreq);
            
        }
        [HttpGet]
        public async Task<IActionResult> UpdateVillaNumber(int villaNo)
        {
            VillaNumberUpdateVM villaNumVm = new VillaNumberUpdateVM();

            var response = await villaNumberService.GetAsync<APIResponse>(villaNo);
            if (response != null && response.IsSuccess)
            {
                //deserialize y3ny ahawel el json l c# object we henna haddedet noo3 el object ely 3yzah eno villadto
                VillaNumberDTO model = JsonConvert.DeserializeObject<VillaNumberDTO>(Convert.ToString(response.Result));
               villaNumVm.villaNumber=_mapper.Map<VillaNumberUpdateDTO>(model);


            }
          var response2 = await villaService.GetAllAsync<APIResponse>();

            if (response2 != null && response2.IsSuccess)
            {
                villaNumVm.villaList = JsonConvert.DeserializeObject<List<VillaDTO>>
                    (Convert.ToString(response2.Result)).Select(i => new SelectListItem
                    {

                        Text = i.Name,
                        Value = i.Id.ToString()
                    });
                return View(villaNumVm);
            }
            return NotFound();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateVillaNumber(VillaNumberUpdateVM ofromreq)
        {
            if (ModelState.IsValid)
            {

                var response = await villaNumberService.UpdateAsync<APIResponse>(ofromreq.villaNumber);//betb3at lel client yendah 3ala el api
                if (response != null && response.IsSuccess)
                {
                    TempData["success"] = "Villa Updated Successfully";

                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    if (response.Errors.Count > 0)
                    {
                        ModelState.AddModelError("Errors", response.Errors.FirstOrDefault());

                    }
                }

                //3EDTHA TANY 3SHAN LAW HASAL ERROR 
                var resp = await villaService.GetAllAsync<APIResponse>();


                if (resp != null && resp.IsSuccess)
                {
                    ofromreq.villaList = JsonConvert.DeserializeObject<List<VillaDTO>>
                        (Convert.ToString(resp.Result)).Select(i => new SelectListItem
                        {

                            Text = i.Name,
                            Value = i.Id.ToString()
                        });
                }

            }
            //invalid model
            TempData["error"] = "Error encountered";

            return View(ofromreq);
        }
        [HttpGet]
        public async Task<IActionResult> DeleteVillaNumber(int villaNo)
        {
            VillaNumberDeleteVM villaNumVm = new VillaNumberDeleteVM();

            var response = await villaNumberService.GetAsync<APIResponse>(villaNo);
            if (response != null && response.IsSuccess)
            {
                //deserialize y3ny ahawel el json l c# object we henna haddedet noo3 el object ely 3yzah eno villadto
                VillaNumberDTO model = JsonConvert.DeserializeObject<VillaNumberDTO>(Convert.ToString(response.Result));
                villaNumVm.villaNumber = model;


            }
            response = await villaService.GetAllAsync<APIResponse>();

            if (response != null && response.IsSuccess)
            {
                villaNumVm.villaList = JsonConvert.DeserializeObject<List<VillaDTO>>
                    (Convert.ToString(response.Result)).Select(i => new SelectListItem
                    {

                        Text = i.Name,
                        Value = i.Id.ToString()
                    });
                return View(villaNumVm);
            }
            return NotFound();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteVillaNumber(VillaNumberDeleteVM ofromreq)
        {


            var response = await villaNumberService.DeleteAsync<APIResponse>(ofromreq.villaNumber.VillaNo);//betb3at lel client yendah 3ala el api
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
