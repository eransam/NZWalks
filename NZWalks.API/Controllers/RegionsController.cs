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


using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;

namespace NZWalks.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegionsController : ControllerBase
    {

        private readonly NZWalksDbContext dbContext;

        //ctor
        public RegionsController(NZWalksDbContext dbContext)
        {
            this.dbContext = dbContext;

        }

        [HttpGet]
        public IActionResult GetAll()
        {
            //get data from the db
            var regionsDomain = dbContext.Regions.ToList();

            //map domain module to DTOs
            var regionsDto = new List<RegionDto>();

            foreach (var regionDomain in regionsDomain)
            {
                regionsDto.Add(new RegionDto()
                {
                    Id = regionDomain.Id,
                    Code = regionDomain.Code,
                    Name = regionDomain.Name,
                    RegionImageUrl = regionDomain.RegionImageUrl
                });

            }



            return Ok(regionsDto);

        }


        //-------------------------------------------
        //-------------------------------------------


        [HttpGet]
        [Route("{id:guid}")]

        public IActionResult GetById([FromRoute]Guid id)
        {

            var regionsDomain = dbContext.Regions.FirstOrDefault(x=>x.Id == id);
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
        //-------------------------------------------
        //-------------------------------------------

        [HttpPost]

        // AddRegionRequestDto ואנ מכניסים אותו למשתנה בשם AddRegionRequestDto מקבל מהבודי של הבקשה אובייקט מסוג      
        public IActionResult Create([FromBody] AddRegionRequestDto AddRegionRequestDto)

        {
            var regionDomainModel = new Region
            {
                Code = AddRegionRequestDto.Code,
                Name = AddRegionRequestDto.Name,
                RegionImageUrl = AddRegionRequestDto.RegionImageUrl
            };

            //מוסיפים אותו לדאטה בייס ושומרים
            dbContext.Regions.Add(regionDomainModel);
            dbContext.SaveChanges();

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


        //-------------------------------------------
        //-------------------------------------------



        [HttpPut]
        //מקבל פרמטר בקריאה
        [Route("{id:guid}")]
        public IActionResult Update([FromRoute] Guid id, [FromBody] UpdateRegionRequestDto UpdateRegionRequestDto)
        {
            // Convert DTO to Domain model
            var regionDomainModel  = dbContext.Regions.FirstOrDefault(x => x.Id == id);

            if (regionDomainModel == null)
            { 
                return NotFound();

            }
            regionDomainModel.Code = UpdateRegionRequestDto.Code;
            regionDomainModel.Name = UpdateRegionRequestDto.Name;
            regionDomainModel.RegionImageUrl = UpdateRegionRequestDto.RegionImageUrl;

            dbContext.SaveChanges();

            // Convert Domain back to DTO
            var regionDTO = new RegionDto
            {
                Id = regionDomainModel.Id,
                Code = regionDomainModel.Code,
                Name = regionDomainModel.Name,
                RegionImageUrl = regionDomainModel.RegionImageUrl
            };


            // Return Ok response
            return Ok(regionDTO);
        }


        [HttpDelete]
        //מקבל פרמטר בקריאה
        [Route("{id:guid}")]
        public IActionResult Delete([FromRoute] Guid id)
        {
            // Convert DTO to Domain model
            var regionDomainModel = dbContext.Regions.FirstOrDefault(x => x.Id == id);

            if (regionDomainModel == null)
            {
                return NotFound();

            }


            dbContext.Regions.Remove(regionDomainModel);


            dbContext.SaveChanges();

            // Convert Domain back to DTO
            var regionDTO = new RegionDto
            {
                Id = regionDomainModel.Id,
                Code = regionDomainModel.Code,
                Name = regionDomainModel.Name,
                RegionImageUrl = regionDomainModel.RegionImageUrl
            };


            // Return Ok response
            return Ok(regionDTO);
        }


    }


}

