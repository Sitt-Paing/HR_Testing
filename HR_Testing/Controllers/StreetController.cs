using Hr_Testing.Data;
using Hr_Testing.Entities;
using Hr_Testing.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Hr_Testing.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StreetController(Hr_TestingDbContext context) : ControllerBase
    {
        [HttpGet]
        [EndpointSummary("Get all streets")]
        public async Task<IActionResult> GetAsync()
        {
            List<Street> streetData = await context.Streets.ToListAsync();
            if (streetData == null)
            {
                return NotFound(new DefaultResponseModel()
                {
                    Success = false,
                    Statuscode = StatusCodes.Status404NotFound,
                    Message = "Streets not found",
                    Data = null
                });
            }
            else
            {
                return Ok(new DefaultResponseModel()
                {
                    Success = true,
                    Statuscode = StatusCodes.Status200OK,
                    Message = "Streets retrieved successfully",
                    Data = streetData
                });
            }
        }

        [HttpGet("{id}")]
        [EndpointSummary("Get the street by Id")]
        public async Task<IActionResult> GetStreetByIdAsync(int id)
        {
            Street? streetData = await context.Streets.FindAsync(id);
            if(streetData == null)
            {
                return NotFound(new DefaultResponseModel()
                {
                    Success = false,
                    Statuscode = StatusCodes.Status404NotFound,
                    Message = "Street not found",
                    Data = null
                });
            }
            else
            {
                return Ok(new DefaultResponseModel()
                {
                    Success = true,
                    Statuscode = StatusCodes.Status200OK,
                    Message = "Street retrieved successfully",
                    Data = streetData
                });
            }
        }

        [HttpGet("township/{townshipId}")]
        [EndpointSummary("Get streets by Township Id")]
        public async Task<IActionResult> GetStreetByTownshipAsync(int townshipId)
        {
            IQueryable<Street> filteredStreets = from s in context.Streets
                                  join t in context.Townships on s.TownshipId equals t.TownshipId
                                  where s.TownshipId == townshipId
                                  select s;

            if(filteredStreets == null)
            {
                return NotFound(new DefaultResponseModel()
                {
                    Success = false,
                    Statuscode = StatusCodes.Status404NotFound,
                    Message = "Streets retrieved failed!",
                    Data = null
                });
            }

            else
            {
                return Ok(new DefaultResponseModel()
                {
                    Success = true,
                    Statuscode = StatusCodes.Status200OK,
                    Message = "Streets retrieved successfully!",
                    Data = filteredStreets
                });
            }
        }

        [HttpPost]
        [EndpointSummary("Create a new street")]
        public async Task<IActionResult> CreateAsync(Street street)
        {
            bool existingStreet = await context.Streets.AnyAsync(s => s.StreetName == street.StreetName);
            if (existingStreet)
            {
                return Conflict(new DefaultResponseModel()
                {
                    Success = false,
                    Statuscode = StatusCodes.Status409Conflict,
                    Message = "Street with the same name already exists",
                    Data = null
                });
            }

            street.StreetId = street.StreetId;
            street.StreetName = street.StreetName;
            street.TownshipId = street.TownshipId;
            street.CreatedAt = DateTime.Now;
            context.Streets.Add(street);

            return await context.SaveChangesAsync() > 0
                ? Ok(new DefaultResponseModel()
                {
                    Success = true,
                    Statuscode = StatusCodes.Status201Created,
                    Message = "Street created successfully",
                    Data = street
                })
                : BadRequest(new DefaultResponseModel()
                {
                    Success = false,
                    Statuscode = StatusCodes.Status400BadRequest,
                    Message = "Failed to create street",
                    Data = null
                });
        }

        [HttpPut]
        [EndpointSummary("Update Streets data")]
        public async Task<IActionResult> UpdateAsync(Street street)
        {
            Street? streetData = await context.Streets.FirstOrDefaultAsync(s => s.StreetId == street.StreetId);
            if(streetData == null)
            {
                return NotFound(new DefaultResponseModel()
                {
                    Success = false,
                    Statuscode = StatusCodes.Status404NotFound,
                    Message = "Data to Update not found!",
                    Data = null
                });
            }

            streetData.StreetName = street.StreetName;
            streetData.TownshipId = street.TownshipId;
            streetData.UpdatedAt = DateTime.Now;
            context.Streets.Update(street);

            return await context.SaveChangesAsync() > 0
                ? Ok(new DefaultResponseModel()
                {
                    Success = true,
                    Statuscode = StatusCodes.Status200OK,
                    Message = "Updated Successfully!",
                    Data = streetData
                })
                : BadRequest(new DefaultResponseModel()
                {
                    Success = false,
                    Statuscode = StatusCodes.Status400BadRequest,
                    Message = "Update failed.",
                    Data = null
                });
        }

        [HttpDelete("{id}")]
        [EndpointSummary("Delete Street Data")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            Street? streetData = await context.Streets.FindAsync(id);
            if(streetData == null)
            {
                return NotFound(new DefaultResponseModel()
                {
                    Success = false,
                    Statuscode = StatusCodes.Status200OK,
                    Message = "Data to delete not found.",
                    Data = null
                });
            }

            streetData.DeletedAt = DateTime.Now;
            //context.Streets.Remove(streetData);

            return await context.SaveChangesAsync() > 0
                ? Ok(new DefaultResponseModel()
                {
                    Success = true,
                    Statuscode = StatusCodes.Status200OK,
                    Message = "Deleted successfully!",
                    Data = null
                })
                : BadRequest(new DefaultResponseModel()
                {
                    Success = true,
                    Statuscode = StatusCodes.Status400BadRequest,
                    Message = "Delete failed",
                    Data = null
                });
        }

        [HttpGet("deleted")]
        [EndpointSummary("GetDeletedData")]
        public async Task<IActionResult> GetDeleteStreetAsync()
        {
            List<Street> deletedStreet = await context.Streets
                .IgnoreQueryFilters()
                .Where(p => p.DeletedAt != null)
                .ToListAsync();

            if(deletedStreet == null)
            {
                return NotFound(new DefaultResponseModel()
                {
                    Success = false,
                    Statuscode = StatusCodes.Status404NotFound,
                    Message = "Deleted Data not found",
                    Data = null
                });
            }

            else
            {
                return Ok(new DefaultResponseModel()
                {
                    Success = true,
                    Statuscode = StatusCodes.Status200OK,
                    Message = "Retrieving Deleted Data successfully",
                    Data = deletedStreet
                });
            }
        }
    }
}
