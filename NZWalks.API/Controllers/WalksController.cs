using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;
using NZWalks.API.Repositories;
using System.Net;

namespace NZWalks.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WalksController : Controller
    {
        private readonly IWalkRepository walkRepository;
        private readonly IMapper mapper;

        public WalksController(IWalkRepository walkRepository, IMapper mapper)
        {
            this.walkRepository = walkRepository;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllWalksAsync
            (
            [FromQuery] string? filterOn,
            [FromQuery] string? filterQuery,
            [FromQuery] string? sortBy,
            [FromQuery] bool? isAscending,
            [FromQuery] int pageNumber=1,
            [FromQuery] int pageSize = 1000

            )
        {

            try
            {
                // Fetch data from database - domain walks
                //***isAscending ?? true - its mean that if its null torn it to true***
                var walksDomain = await walkRepository.GetAllAsync(
                    filterOn,
                    filterQuery,
                    sortBy,
                    isAscending ?? true,
                    pageNumber,
                    pageSize

                    );
                // Return response
                return Ok(mapper.Map<List<WalkDto>>(walksDomain));
            }
            catch (Exception ex)
            {
                return Problem("SOMTHING WAS WRONG", null, (int)HttpStatusCode.InternalServerError);
            }
        }

        [HttpGet]
        [Route("{id:guid}")]
        public async Task<IActionResult> GetById([FromRoute] Guid id)

        {
            // Get Walk Domain object from database
            var walkDomainModel = await walkRepository.GetByIdAsync(id);

            if (walkDomainModel == null)
            {
                return NotFound();
            }

            // Return response
            return Ok(mapper.Map<WalkDto>(walkDomainModel));
        }


        [HttpPost]
        public async Task<IActionResult> AddWalkAsync([FromBody] AddWalkRequestDto AddWalkRequestDto)
        {
            if (ModelState.IsValid)
            {
                var walkDomainModel = mapper.Map<Walk>(AddWalkRequestDto);
                await walkRepository.CreateAsync(walkDomainModel);
                return Ok(mapper.Map<WalkDto>(walkDomainModel));
            }
            else
            {
                return BadRequest();
            }


        }


        [HttpPut]
        [Route("{id:guid}")]
        public async Task<IActionResult> UpdateWalkAsync([FromRoute] Guid id,UpdateWalkRequestDto UpdateWalkRequestDto)
        {
            if (ModelState.IsValid)
            {            // Convert DTO to Domain object
                var walkDomainModel = mapper.Map<Walk>(UpdateWalkRequestDto);
                walkDomainModel = await walkRepository.UpdateAsync(id, walkDomainModel);

                if (walkDomainModel == null)
                {
                    return NotFound();
                }

                return Ok(mapper.Map<WalkDto>(walkDomainModel));
            } else { return BadRequest(); }



        }


        [HttpDelete]
        [Route("{id:guid}")]
        public async Task<IActionResult> DeleteWalkAsync([FromRoute] Guid id)
        {
            // call Repository to delete walk
            var deleteWalkDomainModel = await walkRepository.DeleteAsync(id);

            if (deleteWalkDomainModel  == null)
            {
                return NotFound();
            }
            return Ok(mapper.Map<WalkDto>(deleteWalkDomainModel));
        }
    }
}