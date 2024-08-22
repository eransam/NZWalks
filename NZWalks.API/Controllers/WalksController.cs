using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;
using NZWalks.API.Repositories;

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
        public async Task<IActionResult> GetAllWalksAsync()
        {
            // Fetch data from database - domain walks
            var walksDomain = await walkRepository.GetAllAsync();
            // Return response
            return Ok(mapper.Map<List<WalkDto>>(walksDomain));
        }

        //[HttpGet]
        //[Route("{id:guid}")]
        //[ActionName("GetWalkAsync")]
        //public async Task<IActionResult> GetWalkAsync(Guid id)
        //{
        //    // Get Walk Domain object from database
        //    var walkDomin = await walkRepository.GetAsync(id);

        //    // Convert Domain object to DTO
        //    var walkDTO = mapper.Map<Models.DTO.Walk>(walkDomin);

        //    // Return response
        //    return Ok(walkDTO);
        //}

        [HttpPost]
        public async Task<IActionResult> AddWalkAsync([FromBody] AddWalkRequestDto AddWalkRequestDto)
        {

            var walkDomainModel = mapper.Map<Walk>(AddWalkRequestDto);
            await walkRepository.CreateAsync(walkDomainModel);
            return Ok(mapper.Map<WalkDto>(walkDomainModel));
        }


    //    [HttpPut]
    //    [Route("{id:guid}")]
    //    public async Task<IActionResult> UpdateWalkAsync([FromRoute] Guid id,
    //        [FromBody] Models.DTO.UpdateWalkRequest updateWalkRequest)
    //    {
    //        // Convert DTO to Domain object
    //        var walkDomain = new Models.Domain.Walk
    //        {
    //            Length = updateWalkRequest.Length,
    //            Name = updateWalkRequest.Name,
    //            RegionId = updateWalkRequest.RegionId,
    //            WalkDifficultyId = updateWalkRequest.WalkDifficultyId
    //        };

    //        // Pass details to Repository - Get Domain object in response (or null)
    //        walkDomain = await walkRepository.UpdateAsync(id, walkDomain);

    //        // Handle Null (not found)
    //        if (walkDomain == null)
    //        {
    //            return NotFound();
    //        }

    //        // Convert back Domain to DTO
    //        var walkDTO = new Models.DTO.Walk
    //        {
    //            Id = walkDomain.Id,
    //            Length = walkDomain.Length,
    //            Name = walkDomain.Name,
    //            RegionId = walkDomain.RegionId,
    //            WalkDifficultyId = walkDomain.WalkDifficultyId
    //        };

    //        // Return Response
    //        return Ok(walkDTO);
    //    }

    //    [HttpDelete]
    //    [Route("{id:guid}")]
    //    public async Task<IActionResult> DeleteWalkAsync(Guid id)
    //    {
    //        // call Repository to delete walk
    //        var walkDomain = await walkRepository.DeleteAsync(id);

    //        if (walkDomain == null)
    //        {
    //            return NotFound();
    //        }

    //        var walkDTO = mapper.Map<Models.DTO.Walk>(walkDomain);

    //        return Ok(walkDTO);
    //    }
  }
    }