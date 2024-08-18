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
            var regions = dbContext.Regions.ToList();
                return Ok(regions);

        }


        [HttpGet]
        [Route("{id:guid}")]

        public IActionResult GetById([FromRoute]Guid id)
        {

            var region = dbContext.Regions.FirstOrDefault(x=>x.Id == id);
            //var region = dbContext.Regions.Find(id);

            if (region == null)
            {
                return NotFound();
            }
            return Ok(region);

        }



    }
}


