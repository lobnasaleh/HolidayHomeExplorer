using MagicVillaApI2.Models.DTO;
using MagicVillaApI2.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MagicVillaApI2.Repositories.Interfaces;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.JsonPatch;
using AutoMapper;

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
        [ProducesResponseType(statusCode: 200)]
        [ProducesResponseType(statusCode: 404)]
        public async Task<ActionResult<List<VillaDTO>>> getAllVillas()
        {

            List<Villa> vsFromdb =await _villaRepository.GetAllAsyncWithExpression();
            if (vsFromdb == null)
            {
                return NotFound();
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
            _logger.LogInformation("getting All Villas");
            return Ok(villaDTOlist);
       
        }


        [HttpGet("{id:int}", Name = "getVillaById")]
        [ProducesResponseType(statusCode: 200)]
        [ProducesResponseType(statusCode: 400)]//bad request
        [ProducesResponseType(statusCode: 404)]//not found

        public async Task<ActionResult<VillaDTO>> getVillaById(int id)
        {
            if (id <= 0)
            {
                return BadRequest();
            }

            Villa vfromdb = await _villaRepository.GetWithExpressionAsync(v=>v.Id==id);

            if (vfromdb == null)
            {
                _logger.LogError("No Villa With This id");

                return NotFound();

            }
         VillaDTO Vo= _mapper.Map<VillaDTO>(vfromdb);

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

            _logger.LogInformation($"getting Villa with id:{id}");

            return Ok(Vo);

        }
        [HttpPost]
        [ProducesResponseType(statusCode: 201)]//created
        [ProducesResponseType(statusCode: 400)]//bad request
        public async Task<ActionResult<VillaDTO>> CreateVilla([FromBody] VillaCreateDTO villafromreq)
        {
            //check validity
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            //check if not created before 
            if (await _villaRepository.ExistsByNameAsync(villafromreq.Name))
            {
                ModelState.AddModelError("", "Villa already exists");
                return BadRequest(ModelState);
            }
          Villa Vo=  _mapper.Map<Villa>(villafromreq);
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
             

          await _villaRepository.AddAsync(Vo);
         await _villaRepository.saveAsync();


            return CreatedAtAction("getVillaById", new { id = Vo.Id },Vo);
        }

        [HttpDelete("{id:int}")]
        [ProducesResponseType(statusCode: 400)]//Bad Request
        [ProducesResponseType(statusCode: 404)]//Not Found
        [ProducesResponseType(statusCode: 204)]//No Content
        public async Task<IActionResult> RemoveVilla(int id) {

            Villa v =await  _villaRepository.GetWithExpressionAsync(v=>v.Id == id);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (v == null)
            {
                return NotFound();
            }

            _villaRepository.Remove(v);
         await _villaRepository.saveAsync();

            return NoContent();

        }
        [HttpPut("{id:int}")]
        [ProducesResponseType(statusCode:204)]
        [ProducesResponseType(statusCode: 400)]
        [ProducesResponseType(statusCode: 404)]

        public async Task<IActionResult> UpdateVilla(int id,[FromBody] VillaUpdateDTO newupdate)
        {
            Villa v = await _villaRepository.GetWithExpressionAsync(v=>v.Id==id,false);
            if (v == null)
            {
                return NotFound();
            }
            if (!ModelState.IsValid)
            {
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
       
           _villaRepository.Update(v2); //no need if i am already tracking obj returned from db

            await _villaRepository.saveAsync();
            return NoContent();
        
        }
        [HttpPatch("{id:int}")]
        [ProducesResponseType(statusCode: 204)]
        [ProducesResponseType(statusCode: 400)]
        [ProducesResponseType(statusCode: 404)]
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

