//דוגמא ליצירת קונטרולר ארד קודד
//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Mvc;
//using NZWalks.API.Models.Domain;

//namespace NZWalks.API.Controllers
//{
//    [Route("api/[controller]")]
//    [ApiController]
//    public class RegionsController : ControllerBase
//    {

//        [HttpGet]
//        public IActionResult GetAll()
//        {
//            var regions = new List<Region>
//            {
//                new Region
//                {
//                    Id = Guid.NewGuid(),
//                    Name = "Name",
//                    Code = "",
//                    RegionImageUrl = ""
//                },
//                new Region
//                {
//                    Id = Guid.NewGuid(),
//                    Name = "Name",
//                    Code = "",
//                    RegionImageUrl = ""
//                }
//            };

//            return Ok(regions);

//        }
//    }
//}


using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NZWalks.API.CustomActionFilters;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;
using NZWalks.API.Repositories;

namespace NZWalks.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegionsController : ControllerBase
    {

        private readonly NZWalksDbContext dbContext;
        private readonly IMapper mapper;
        private readonly ILogger<RegionsController> logger;

        public IRegionRepository RegionRepository { get; }

        //ctor
        public RegionsController(
            NZWalksDbContext dbContext,
            IRegionRepository RegionRepository,
            IMapper mapper,
              ILogger<RegionsController> logger)
        {
            this.dbContext = dbContext;
            this.RegionRepository = RegionRepository;
            this.mapper = mapper;
            this.logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {

            logger.LogInformation("test");
            //get data from the db
            //var regionsDomain = await dbContext.Regions.ToListAsync(); (the level before)
            var regionsDomain = await RegionRepository.GetAllAsync();

            //the old map domain module to DTOs
            //var regionsDto = new List<RegionDto>();

            //foreach (var regionDomain in regionsDomain)
            //{
            //    regionsDto.Add(new RegionDto()
            //    {
            //        Id = regionDomain.Id,
            //        Code = regionDomain.Code,
            //        Name = regionDomain.Name,
            //        RegionImageUrl = regionDomain.RegionImageUrl
            //    });

            //}

            // the new map domain module to DTOs
            var regionsDto=  mapper.Map<List<RegionDto>>(regionsDomain);

            return Ok(regionsDto);
        }


        [HttpGet]
        [Route("{id:Guid}")]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            var regionsDomain = await RegionRepository.GetByIdAsync(id);
            //var region = dbContext.Regions.Find(id);
            if (regionsDomain == null)
            {
                return NotFound();
            }
            var regionsDto = new RegionDto
            {
                Id = regionsDomain.Id,
                Code = regionsDomain.Code,
                Name = regionsDomain.Name,
                RegionImageUrl = regionsDomain.RegionImageUrl
            };
            return Ok(regionsDto);
        }


        [HttpPost]
        //from ValidateModelAttribute file
        [ValidateModel]
        // AddRegionRequestDto ואנ מכניסים אותו למשתנה בשם AddRegionRequestDto מקבל מהבודי של הבקשה אובייקט מסוג      
        public async Task<IActionResult> Create([FromBody] AddRegionRequestDto AddRegionRequestDto)
        {

                var regionDomainModel = mapper.Map<Region>(AddRegionRequestDto);

                //מוסיפים אותו לדאטה בייס ושומרים
                //בעבר:
                //await dbContext.Regions.AddAsync(regionDomainModel);
                //await dbContext.SaveChangesAsync();
                regionDomainModel = await RegionRepository.CreateAsync(regionDomainModel);

                //ממירים אותו לאובייקט מצומצם אשר יוחזר ליוזר
                var regionDto = new RegionDto
                {
                    Id = regionDomainModel.Id,
                    Code = regionDomainModel.Code,
                    Name = regionDomainModel.Name,
                    RegionImageUrl = regionDomainModel.RegionImageUrl
                };

                //CreatedAtAction  = used to return a 201 Created response
                //nameof(GetById), new { id = regionDto.Id } - מחזיר בהדר של הבקשה את היואראל של האובייקט שהתווסף
                //regionDto = מחזיר את האובייקט שהתווסף
                return CreatedAtAction(nameof(GetById), new { id = regionDto.Id }, regionDto);

        }


        [HttpPut]
        //מקבל פרמטר בקריאה
        [Route("{id:guid}")]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateRegionRequestDto updateRegionRequestDto)
        {

            // Convert DTO to Domain model
            

            var regionDomainModel = new Region {
                Code = updateRegionRequestDto.Code,
                Name = updateRegionRequestDto.Name,
                RegionImageUrl = updateRegionRequestDto.RegionImageUrl
            };

            regionDomainModel=await RegionRepository.UpdateAsync(id, regionDomainModel);
            if (regionDomainModel == null)
            { 
                return NotFound();
            }
            regionDomainModel.Code = updateRegionRequestDto.Code;
            regionDomainModel.Name = updateRegionRequestDto.Name;
            regionDomainModel.RegionImageUrl = updateRegionRequestDto.RegionImageUrl;
            await dbContext.SaveChangesAsync();
            // Convert Domain back to DTO
            var regionDto = new RegionDto
            {
                Id = regionDomainModel.Id,
                Code = regionDomainModel.Code,
                Name = regionDomainModel.Name,
                RegionImageUrl = regionDomainModel.RegionImageUrl
            };
            // Return Ok response
            return Ok(regionDto);
        }


        [HttpDelete]
        //מקבל פרמטר בקריאה
        [Route("{id:guid}")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            // Convert DTO to Domain model
            var regionDomainModel = await RegionRepository.DeleteAsync(id);

            if (regionDomainModel == null)
            {
                return NotFound();
            }

            // Convert Domain back to DTO
            var regionDto = new RegionDto
            {
                Id = regionDomainModel.Id,
                Code = regionDomainModel.Code,
                Name = regionDomainModel.Name,
                RegionImageUrl = regionDomainModel.RegionImageUrl
            };
            // Return Ok response
            return Ok(regionDto);
        }



    }


}

