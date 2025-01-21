using MagicVillaApI2.Models.DTO;
using MagicVillaApI2.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MagicVillaApI2.Repositories.Interfaces;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.JsonPatch;
using AutoMapper;
using System.Net;
using Microsoft.AspNetCore.Authorization;

namespace MagicVillaApI2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VillaApiController : ControllerBase
    {
       protected readonly APIResponse response; //to prevent redundant intialization for every method 
       private readonly IVillaRepository _villaRepository;
       private readonly ILogger<VillaApiController> _logger;
       private readonly IMapper _mapper;
        public VillaApiController(IMapper _mapper,IVillaRepository _villaRepository, ILogger<VillaApiController> _logger)
        {
            this.response=new APIResponse();
            this._mapper = _mapper;
            this._villaRepository = _villaRepository;
            this._logger = _logger;
        }
        [HttpGet]
        [Authorize]//3ady sawa2 user aw admin
        [ProducesResponseType(statusCode: 200)]
        [ProducesResponseType(statusCode: 404)]
        [ProducesResponseType(statusCode: 401)]//unauthorized
        [ProducesResponseType(statusCode: 403)]//forbidden
        public async Task<ActionResult<APIResponse>> getAllVillas([FromQuery] int occupancy,
            [FromQuery] string? search)
        {
            try
            {

                List<Villa> vsFromdb;
                if (occupancy > 0) { //law matb3atetsh byt3melha bind b 0

                    vsFromdb = await _villaRepository.GetAllAsyncWithExpression(v=>v.Occupancy==occupancy);
                }
                else
                {
                    vsFromdb = await _villaRepository.GetAllAsyncWithExpression();
                }
                if (!string.IsNullOrEmpty(search))
                {
                    vsFromdb=vsFromdb.Where(u=>u.Name.ToLower().Contains(search.ToLower())).ToList();
                }

                if (vsFromdb == null)
                {
                    response.StatusCode=HttpStatusCode.NotFound;
                    return NotFound(response);
                }

                List<VillaDTO> villaDTOlist = _mapper.Map<List<VillaDTO>>(vsFromdb);

                /* used mapping instead
                   List<VillaDTO> villaDTOlist = new List<VillaDTO>();
                  foreach (var item in vsFromdb)
                 {
                     VillaDTO v = new VillaDTO();
                     v.Id = item.Id;
                     v.Name = item.Name;
                     v.Occupancy = item.Occupancy;
                     v.Amenity = item.Amenity;
                     v.Rate = item.Rate;
                     v.Details = item.Details;
                     v.ImageUrl = item.ImageUrl;
                     v.Sqft = item.Sqft;

                     villaDTOlist.Add(v);
                 }*/

                response.IsSuccess = true;
                response.Result = villaDTOlist;
                response.StatusCode = HttpStatusCode.OK;
                _logger.LogInformation("getting All Villas");
                return Ok(response);
            }
            catch (Exception ex) {

                response.IsSuccess = false;
                response.Errors=new List<string> { ex.Message };
                // response.Errors.Add(ex.Message);//law 3mlt keda hyb2a ghalt l2en el list not intialized 
            }
                return response;


        }


        [HttpGet("{id:int}", Name = "getVillaById")]
       // [Authorize(Roles ="admin")]
        [ProducesResponseType(statusCode: 200)]
        [ProducesResponseType(statusCode: 400)]//bad request
        [ProducesResponseType(statusCode: 404)]//not found
        [ProducesResponseType(statusCode: 401)]//unauthorized
        [ProducesResponseType(statusCode: 403)]//forbidden


        public async Task<ActionResult<APIResponse>> getVillaById(int id)
        {
            try
            {

                if (id <= 0)
                {
                    response.StatusCode=HttpStatusCode.BadRequest;
                    return BadRequest(response);
                }

                Villa vfromdb = await _villaRepository.GetWithExpressionAsync(v => v.Id == id);

                if (vfromdb == null)
                {
                    _logger.LogError("No Villa With This id");
                    response.StatusCode=HttpStatusCode.NotFound;

                    return NotFound(response);

                }
                /*Used Mapping Instead
     VillaDTO vo = new VillaDTO();
     vo.Id = id;
     vo.Name = vfromdb.Name;
     vo.Occupancy = vfromdb.Occupancy;
     vo.Amenity = vfromdb.Amenity;
     vo.Sqft = vfromdb.Sqft;
     vo.Rate = vfromdb.Rate;
     vo.Details = vfromdb.Details;
     vo.ImageUrl = vfromdb.ImageUrl;*/
                VillaDTO Vo = _mapper.Map<VillaDTO>(vfromdb);
                response.StatusCode=HttpStatusCode.OK;
                response.Result = Vo;
                response.IsSuccess=true;

    

                _logger.LogInformation($"getting Villa with id:{id}");

                return Ok(response);
            }
            catch (Exception ex) { 
            
                response.IsSuccess=false;
                response.Errors=new List<string> {ex.Message};


            }
            return response;

        }
        [HttpPost]
        [Authorize(Roles ="admin")]
        [ProducesResponseType(statusCode: 201)]//created
        [ProducesResponseType(statusCode: 400)]//bad request
        [ProducesResponseType(statusCode: 401)]//unauthorized
        [ProducesResponseType(statusCode: 403)]//forbidden
        public async Task<ActionResult<APIResponse>> CreateVilla([FromBody] VillaCreateDTO villafromreq)
        {
            try
            {
                //check validity
                if (!ModelState.IsValid)
                {
                    response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(ModelState);
                }
                //check if not created before 
                if (await _villaRepository.ExistsByNameAsync(villafromreq.Name))
                {
                    ModelState.AddModelError("Errors", "Villa already exists");
                    response.StatusCode = HttpStatusCode.BadRequest;

                    return BadRequest(ModelState);
                }
                /* Used Mapping instead
                 Villa v = new Villa();

                  v.Name = villafromreq.Name;
                  v.CreatedAt = DateTime.Now;
                  v.UpdatedDate = DateTime.Now;
                  v.Amenity= villafromreq.Amenity;
                  v.Sqft = villafromreq.Sqft;
                  v.Rate = villafromreq.Rate;
                  v.Details = villafromreq.Details;
                  v.ImageUrl = villafromreq.ImageUrl;
                  v.Occupancy = villafromreq.Occupancy;*/

                Villa Vo = _mapper.Map<Villa>(villafromreq);
                response.StatusCode = HttpStatusCode.Created;
                response.Result = Vo;
                response.IsSuccess = true;


                await _villaRepository.AddAsync(Vo);
                await _villaRepository.saveAsync();


                return CreatedAtAction("getVillaById", new { id = Vo.Id }, response);//
            }
            catch (Exception ex) { 
              response.IsSuccess=false;
              response.Errors = new List<string>() { ex.Message};
            
            
            }
            return response;
     }

        [HttpDelete("{id:int}")]
        [Authorize(Roles = "admin")]


        [ProducesResponseType(statusCode: 400)]//Bad Request
        [ProducesResponseType(statusCode: 404)]//Not Found
        [ProducesResponseType(statusCode: 200)]
        [ProducesResponseType(statusCode: 401)]//unauthorized
        [ProducesResponseType(statusCode: 403)]//forbidden
        // [ProducesResponseType(statusCode: 204)]//No Content haraha3 ok 3shan nocontent mlhash parameters ab3t feeha response
        public async Task<ActionResult<APIResponse>> RemoveVilla(int id) {
            try
            {
                Villa v = await _villaRepository.GetWithExpressionAsync(v => v.Id == id);

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                if (v == null)
                {
                    response.IsSuccess = false;
                    response.StatusCode=HttpStatusCode.NotFound;
                    return NotFound();
                }

                _villaRepository.Remove(v);
                await _villaRepository.saveAsync();
                response.StatusCode = HttpStatusCode.NoContent;
                response.IsSuccess=true;
                
                return Ok(response);
            }
            catch (Exception ex) { 
            
            response.Errors=new List<string>() { ex.Message};
            response.IsSuccess=false;
            }
            return response;

        }
        [Authorize(Roles = "admin")]
        [HttpPut("{id:int}")]
        [ProducesResponseType(statusCode: 200)]
        [ProducesResponseType(statusCode: 400)]
        [ProducesResponseType(statusCode: 404)]
        [ProducesResponseType(statusCode: 401)]//unauthorized
        [ProducesResponseType(statusCode: 403)]//forbidden

        public async Task<ActionResult<APIResponse>> UpdateVilla(int id,[FromBody] VillaUpdateDTO newupdate)
        {
            try {
            Villa v = await _villaRepository.GetWithExpressionAsync(v=>v.Id==id,false);
            if (v == null)
            {
              response.StatusCode = HttpStatusCode.NotFound;
                
                return NotFound(response);
            }
            if (!ModelState.IsValid)
            {
                    response.IsSuccess = false;
                    response.StatusCode = HttpStatusCode.BadRequest;
                return BadRequest(ModelState);
            }
           
          Villa v2= _mapper.Map<Villa>(newupdate);
            v2.Id = id;

          /* used mapping instaed
           v.Name = newupdate.Name;
            v.Occupancy = newupdate.Occupancy;
            v.UpdatedDate=DateTime.Now;
            v.Rate = newupdate.Rate;
           // v.CreatedAt = DateTime.Now;
            v.Amenity = newupdate.Amenity;
            v.Details = newupdate.Details;
            v.Sqft = newupdate.Sqft;    
            v.ImageUrl = newupdate.ImageUrl;
            v.Details=newupdate.Details;*/
       
          _villaRepository.Update(v2); 
                //no need if i am already tracking obj returned from db

            await _villaRepository.saveAsync();

                response.StatusCode = HttpStatusCode.NoContent;
                response.IsSuccess=true;
 
               return Ok(response);
            }
            catch (Exception ex)
            {
                response.Errors = new List<string>() { ex.Message };
                response.IsSuccess=false;
            }
            return response;

        }
        [Authorize(Roles = "admin")]
        [HttpPatch("{id:int}")]
        [ProducesResponseType(statusCode: 204)]
        [ProducesResponseType(statusCode: 400)]
        [ProducesResponseType(statusCode: 404)]
        [ProducesResponseType(statusCode: 401)]//unauthorized
        [ProducesResponseType(statusCode: 403)]//forbidden
        public async Task<IActionResult> PatchVillaUpdate(int id,JsonPatchDocument<VillaUpdateDTO> jsonPatchDocument)
        {
            if (jsonPatchDocument == null)
            {
            return BadRequest(); 
            }
            if (!ModelState.IsValid)
            {

                return BadRequest(ModelState);
            }

            Villa v = await _villaRepository.GetWithExpressionAsync(v=>v.Id==id,false);

         VillaUpdateDTO   villaDto= _mapper.Map<VillaUpdateDTO>(v);
        /* Used Mapping instaed
           VillaUpdateDTO villaDto = new VillaUpdateDTO();
            villaDto.Name=v.Name;
            villaDto.Occupancy = v.Occupancy;
            villaDto.Rate=v.Rate;
            villaDto.Amenity=v.Amenity;
            villaDto.Sqft=v.Sqft;
            villaDto.Details=v.Details;
            villaDto.ImageUrl=v.ImageUrl;
       */

            if (v == null)
            {
                return NotFound();
            }
           
            jsonPatchDocument.ApplyTo(villaDto, ModelState);
            Villa v2=_mapper.Map<Villa>(villaDto);
        
        /*  used mapping instead
            v.Name=villaDto.Name;
            v.Occupancy=villaDto.Occupancy;
            v.Rate=villaDto.Rate;
            v.Amenity =villaDto.Amenity;
            v.Sqft=villaDto.Sqft;
            v.Details=villaDto.Details;
            v.ImageUrl=villaDto.ImageUrl;*/

            _villaRepository.Update(v2);
          await _villaRepository.saveAsync();

        return NoContent(); 
        
        }
    
    
    }

        }

