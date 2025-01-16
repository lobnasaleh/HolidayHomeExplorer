using AutoMapper;
using MagicVillaApI2.Models;
using MagicVillaApI2.Models.DTO;
using MagicVillaApI2.Repositories;
using MagicVillaApI2.Repositories.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Net;

namespace MagicVillaApI2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VillaNumberApiController : ControllerBase
    {
        private readonly IVillaNumberRepository villanorepo;

        private readonly IVillaRepository villarepo;

        private readonly IMapper mapper;
        protected APIResponse response;
        public VillaNumberApiController(IVillaNumberRepository villanorepo, IMapper mapper, IVillaRepository villarepo)
        {
            this.villanorepo = villanorepo;
            this.mapper = mapper;
            response = new APIResponse();
            this.villarepo = villarepo;
        }

        [HttpGet]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<APIResponse>> GetAllVillaNumbers()
        {//<ActionResult<List<VillaNumberDTO>>>
            try
            {
                List<VillaNumber> vs = await villanorepo.GetAllAsyncWithExpression(includeProperties:"Villa");

                if (vs == null) {
                    response.IsSuccess = false;
                    return NotFound(response);
                }

                //map villaNumber to VillaNumberDto
                List<VillaNumberDTO> villanumberslist = mapper.Map<List<VillaNumberDTO>>(vs);
                response.IsSuccess = true;
                response.StatusCode = HttpStatusCode.OK;
                response.Result = villanumberslist;

                return Ok(response);
            }
            catch (Exception ex) {
                response.IsSuccess = false;
                response.Errors = new List<string> { ex.Message };

            }

            return response;

        }

        [HttpGet("{id:int}", Name = "GetVillaByNo")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<APIResponse>> Get(int id)
        {//<ActionResult<VillaNumberDTO>>

            try
            {
                VillaNumber vn = await villanorepo.GetWithExpressionAsync(vn => vn.VillaNo == id);
                if (vn == null) {
                    response.IsSuccess = false;
                    response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(response);
                }
                if (id <= 0)
                {
                    response.IsSuccess = false;
                    response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(response);
                }
                response.IsSuccess = true;
                response.StatusCode = HttpStatusCode.OK;
                VillaNumberDTO vnD = mapper.Map<VillaNumberDTO>(vn);

                response.Result = vnD;
                return Ok(response);

            }
            catch (Exception ex) {
                response.IsSuccess = false;
                response.Errors = new List<string> { ex.Message };
            }

            return response;

        }

        [HttpPost]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<APIResponse>> CreateVillaNumber([FromBody] VillaNumberCreateDTO vDfromreq)
        {

            try {

                if (!ModelState.IsValid) {
                    response.IsSuccess = false;
                    response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(ModelState);
                }
                if (vDfromreq == null) {
                    response.IsSuccess = false;
                    response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(response);
                }
                if (await villanorepo.GetWithExpressionAsync(vn => vn.VillaNo == vDfromreq.VillaNo)!=null)
                {

                    ModelState.AddModelError("Errors", "duplicate Villa Number");
                    return BadRequest(ModelState);
                }
                //ata2ked bas en el id ely 3yz yrefernece 3aleeh already mawgood 3shan mayatal3shn moshkela 3nd el foreign key
                if (await villarepo.GetWithExpressionAsync(v=>v.Id== vDfromreq.VillaId) == null)
                {
                    ModelState.AddModelError("Errors", "No Villa With this Id");
                    return BadRequest(ModelState);
                }

                //map villanumberdto to the model of db villanumber
                VillaNumber vn = mapper.Map<VillaNumber>(vDfromreq);
                await villanorepo.AddAsync(vn);
                await villanorepo.saveAsync();
                response.StatusCode = HttpStatusCode.Created;
                response.IsSuccess = true;
                response.Result = vDfromreq;
               ActionResult v2= CreatedAtAction(nameof(Get), new { id = vDfromreq.VillaNo }, response);
                return v2;

            }
            catch (Exception ex) {

                response.IsSuccess = false;
                response.Errors = new List<string> { ex.Message };
            }
            return response;
        }
        [HttpDelete("{id:int}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<APIResponse>> RemoveVillaNo(int id)
        {
            try
            {
                VillaNumber vn = await villanorepo.GetWithExpressionAsync(vn => vn.VillaNo == id);

                if (id <= 0)
                {
                    response.StatusCode = HttpStatusCode.BadRequest;
                    response.IsSuccess = false;
                    return BadRequest(response);
                }
                if (vn == null)
                {
                    response.StatusCode = HttpStatusCode.NotFound;
                    response.IsSuccess = false;
                    return NotFound(response);
                }

                villanorepo.Remove(vn);
                await villanorepo.saveAsync();
                response.StatusCode = HttpStatusCode.NoContent;
                response.IsSuccess = true;
                return Ok(response);


            }
            catch (Exception ex) {
                response.Errors = new List<string>() { ex.Message };
                response.IsSuccess = false;

            }

            return response;

        }
        [HttpPut("{id:int}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<APIResponse>>UpdateVillaNo(int id,VillaNumberUpdateDTO vnfromrequest){

            try {

                VillaNumber vn=  await villanorepo.GetWithExpressionAsync(vn=>vn.VillaNo == id,false);
               if (vnfromrequest == null || id != vnfromrequest.VillaNo)
                {
                    response.StatusCode = HttpStatusCode.BadRequest;
                    response.IsSuccess = false;
                    return BadRequest(response);
                }
                
                
                if (id <= 0)
                {
                    response.StatusCode = HttpStatusCode.BadRequest;
                    response.IsSuccess = false;
                    return BadRequest(response);
                }
                if (vn == null)
                {
                    response.StatusCode = HttpStatusCode.NotFound;
                    response.IsSuccess = false;
                    return NotFound(response);
                }
                //ata2ked bas en el id ely 3yz yrefernece 3aleeh already mawgood 3shan mayatal3shn moshkela 3nd el foreign key
                if (await villarepo.GetWithExpressionAsync(v => v.Id == vnfromrequest.VillaId) == null)
                {
                    ModelState.AddModelError("Custom Error", "No Villa With this Id");
                    return BadRequest(ModelState);
                }
                if (!ModelState.IsValid) {

                    response.StatusCode = HttpStatusCode.BadRequest;
                    response.IsSuccess = false;
                    return BadRequest(ModelState);
                }
                //map Villanodto to villno
            VillaNumber vnmapped=    mapper.Map<VillaNumber>(vnfromrequest);

                villanorepo.Update(vnmapped);
                await villanorepo.saveAsync();

                response.IsSuccess = true;
                response.StatusCode=HttpStatusCode.NoContent;
                return Ok(response);

            } catch (Exception ex) { 
        
               response.Errors=new List<string> {ex.Message};
               response.IsSuccess = false;
            }

            return response;
         }

    }
}
