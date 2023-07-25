using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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

        public RegionsController(NZWalksDbContext dbContext) 
        {
            this.dbContext = dbContext;
        }

        //Get All Regions
        //GET: https://localhost:port/api/regions
        [HttpGet]
        public async Task<IActionResult> GetAll() 
        {
            //Get data from database - domain models
            var regionsDomain = await dbContext.Regions.ToListAsync();

            //Map domain modes to DTOs
            var regionsDto = new List<RegionDto>();
            foreach (var regionDomain in regionsDomain)
            {
                regionsDto.Add(new RegionDto
                {
                    Code = regionDomain.Code,
                    Name = regionDomain.Name,
                    RegionImageUrl = regionDomain.RegionImageUrl
                });
            }


            //return Ok(regions);
            //return DTOs
            return Ok(regionsDto);
        }

        //Get a single region
        //GET: https://localhost:port/api/regions/{id}
        [HttpGet("{id:Guid}")]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            //var region = dbContext.Regions.Find(id);
            var regionDomain = await dbContext.Regions.FirstOrDefaultAsync(r => r.Id == id);

            if (regionDomain == null)
            {
                return NotFound();
            }

            var regionDto = new RegionDto
            {
                Code = regionDomain.Code,
                Name = regionDomain.Name,
                RegionImageUrl = regionDomain.RegionImageUrl
            };

            return Ok(regionDto);
        }

        //Post to create a new region
        //POST: https://localhost:port/api/regions
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] AddRegionRequestDto addRegionRequestDto)
        {
            //Map or Convert DTO to Domain model
            var regionDomainModel = new Region
            {
                Code = addRegionRequestDto.Code,
                Name = addRegionRequestDto.Name,
                RegionImageUrl = addRegionRequestDto.RegionImageUrl
            };

            //Use Domain model to create Region
            await dbContext.Regions.AddAsync(regionDomainModel);
            await dbContext.SaveChangesAsync();

            //Map Domain mode back to DTO
            
            var regionDto = new RegionDto
            {
                Code = regionDomainModel.Code,
                Name = regionDomainModel.Name,
                RegionImageUrl = regionDomainModel.RegionImageUrl
            };

            return CreatedAtAction(nameof(GetById), new { id = regionDto.Id}, regionDto ); 
        }

        //Update a single region
        //PUT: https://localhost:port/api/regions/{id}
        [HttpPut("{id:Guid}")]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateRegionRequestDto updateRegionRequestDto)
        {
            //Check if resource exists
            var regionDomainModel = await dbContext.Regions.FirstOrDefaultAsync(r => r.Id == id);

            if (regionDomainModel == null)
            {
                return NotFound();
            }
            // Map DTO to Domain Model
            regionDomainModel.Code = updateRegionRequestDto.Code;
            regionDomainModel.Name = updateRegionRequestDto.Name;
            regionDomainModel.RegionImageUrl = updateRegionRequestDto.RegionImageUrl;

            await dbContext.SaveChangesAsync();

            //Convert Domain Model to DTO
            var regionDto = new RegionDto
            {
                Code = regionDomainModel.Code,
                Name = regionDomainModel.Name,
                RegionImageUrl = regionDomainModel.RegionImageUrl
            };

            return Ok(regionDto);
        }

        //Delete a single region
        //DELETE: https://localhost:port/api/regions/{id}
        [HttpDelete("{id:Guid}")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            //var region = dbContext.Regions.Find(id);
            var regionDomain = await dbContext.Regions.FirstOrDefaultAsync(r => r.Id == id);

            if (regionDomain == null)
            {
                return NotFound();
            }

            dbContext.Regions.Remove(regionDomain);
            await dbContext.SaveChangesAsync();

            return Ok();
        }
    }
}
