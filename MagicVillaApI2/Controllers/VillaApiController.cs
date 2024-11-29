using MagicVillaApI2.Models.DTO;
using MagicVillaApI2.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MagicVillaApI2.Repositories.Interfaces;

namespace MagicVillaApI2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VillaApiController : ControllerBase
    {
        readonly IVillaRepository _villaRepository;
        public VillaApiController(IVillaRepository _villaRepository)
        {
            this._villaRepository = _villaRepository;
        }
        [HttpGet]
        public ActionResult<List<VillaDTO>> getAllVillas()
        {

            List<Villa> vsFromdb = _villaRepository.GetAll();
            if (vsFromdb == null)
            {
                return NotFound();
            }

            List<VillaDTO> villaDTOlist = new List<VillaDTO>();

            foreach (var item in vsFromdb)
            {
                VillaDTO v = new VillaDTO();
                v.Id = item.Id;
                v.Name = item.Name;
                villaDTOlist.Add(v);
            }

            return villaDTOlist;

        }


        [HttpGet("{id:int}")]
        [ProducesResponseType(statusCode: 200)]
        [ProducesResponseType(statusCode: 400)]
        [ProducesResponseType(statusCode: 404)]

        public ActionResult<VillaDTO> getVillaById(int id)
        {
            if (id <= 0)
            {
                return BadRequest();
            }


            Villa vfromdb = _villaRepository.GetById(id);

            if (vfromdb == null)
            {
                return NotFound();
            }

            VillaDTO vo = new VillaDTO();
            vo.Id = id;
            vo.Name = vfromdb.Name;

            return vo;

        }
    }
}
