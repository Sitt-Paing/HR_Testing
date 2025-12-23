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
    public class TownshipController(Hr_TestingDbContext context) : ControllerBase
    {
        [HttpGet]
        [EndpointSummary("Get all townships")]
        public async Task<IActionResult> GetAsync()
        {
            List<Township> TownshipData = await context.Townships.ToListAsync();
            if (TownshipData == null)
            {
                return NotFound(new DefaultResponseModel()
                {
                    Success = false,
                    Statuscode = StatusCodes.Status404NotFound,
                    Message = "Townships not found",
                    Data = null
                });
            }
            else
            {
                return Ok(new DefaultResponseModel()
                {
                    Success = true,
                    Statuscode = StatusCodes.Status200OK,
                    Message = "Townships retrieved successfully",
                    Data = TownshipData
                });
            }
        }

        [HttpGet("{id}")]
        [EndpointSummary("Get the township by Id")]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            Township? TownshipData = await context.Townships.FindAsync(id);
            if (TownshipData == null)
            {
                return NotFound(new DefaultResponseModel()
                {
                    Success = false,
                    Statuscode = StatusCodes.Status404NotFound,
                    Message = "Township not found",
                    Data = null
                });
            }
            else
            {
                return Ok(new DefaultResponseModel()
                {
                    Success = true,
                    Statuscode = StatusCodes.Status200OK,
                    Message = "Township data retrieved successfully",
                    Data = TownshipData
                });
            }
        }

        [HttpGet("State/{stateId}")]
        [EndpointSummary("Get townships by State Id")]
        public async Task<IActionResult> GetByStateIdAsync(int stateId)
        {
            IQueryable<Township> filteredTownships =
                from t in context.Townships
                join s in context.States on t.StateId equals s.StateId
                where s.StateId == stateId
                select t;

            if (filteredTownships == null || !filteredTownships.Any())
            {
                return NotFound(new DefaultResponseModel()
                {
                    Success = false,
                    Statuscode = StatusCodes.Status404NotFound,
                    Message = "No townships found for the specified State Id",
                    Data = null
                });
            }
            else
            {
                return Ok(new DefaultResponseModel()
                {
                    Success = true,
                    Statuscode = StatusCodes.Status200OK,
                    Message = "Townships retrieved successfully",
                    Data = filteredTownships
                });
            }
        }

        [HttpPost]
        [EndpointSummary("Create a new township")]
        public async Task<IActionResult> CreataeAsync(Township township)
        {
            bool existingTownship = await context.Townships.AnyAsync(t => t.TownshipName == township.TownshipName);
            if (existingTownship)
            {
                return Conflict(new DefaultResponseModel()
                {
                    Success = false,
                    Statuscode = StatusCodes.Status409Conflict,
                    Message = "Township already exists",
                    Data = null
                });
            }

            township.TownshipName = township.TownshipName;
            township.TownshipId = township.TownshipId;
            township.StateId = township.StateId;
            township.CreatedAt = DateTime.UtcNow;
            context.Townships.Add(township);

            return await context.SaveChangesAsync() > 0
                ? Ok(new DefaultResponseModel()
                {
                    Success = true,
                    Statuscode = StatusCodes.Status201Created,
                    Message = "Township created successfully",
                    Data = township
                })
                : BadRequest(new DefaultResponseModel()
                {
                    Success = false,
                    Statuscode = StatusCodes.Status400BadRequest,
                    Message = "Failed to create township",
                    Data = null
                });
        }

        [HttpPut]
        [EndpointSummary("Update a township")]
        public async Task<IActionResult> UpdateAsync(Township updatedTownship)
        {
            Township? township = await context.Townships.FirstOrDefaultAsync(t => t.TownshipId == updatedTownship.TownshipId);
            if (township == null)
            {
                return NotFound(new DefaultResponseModel()
                {
                    Success = false,
                    Statuscode = StatusCodes.Status404NotFound,
                    Message = "Township not found",
                    Data = null
                });
            }
            township.UpdatedAt = DateTime.UtcNow;
            township.TownshipName = updatedTownship.TownshipName;
            township.StateId = updatedTownship.StateId;
            context.Townships.Update(township);
            return await context.SaveChangesAsync() > 0
                ? Ok(new DefaultResponseModel()
                {
                    Success = true,
                    Statuscode = StatusCodes.Status200OK,
                    Message = "Township updated successfully",
                    Data = township
                })
                : BadRequest(new DefaultResponseModel()
                {
                    Success = false,
                    Statuscode = StatusCodes.Status400BadRequest,
                    Message = "Failed to update township",
                    Data = null
                });
        }

        [HttpDelete("{id}")]
        [EndpointSummary("Delete a township")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            Township? township = await context.Townships.FindAsync(id);
            if (township == null)
            {
                return NotFound(new DefaultResponseModel()
                {
                    Success = false,
                    Statuscode = StatusCodes.Status404NotFound,
                    Message = "Township not found",
                    Data = null
                });
            }

            township.DeletedAt = DateTime.Now;
            //context.Townships.Remove(township);
            return await context.SaveChangesAsync() > 0
                ? Ok(new DefaultResponseModel()
                {
                    Success = true,
                    Statuscode = StatusCodes.Status200OK,
                    Message = "Township deleted successfully",
                    Data = null
                })
                : BadRequest(new DefaultResponseModel()
                {
                    Success = false,
                    Statuscode = StatusCodes.Status400BadRequest,
                    Message = "Failed to delete township",
                    Data = null
                });
        }


        [HttpGet("deleted")]
        [EndpointSummary("GetDeletedData")]
        public async Task<IActionResult> GetDeleteTownshipAsync()
        {
            List<Township> deletedTownship = await context.Townships
                .IgnoreQueryFilters()
                .Where(p => p.DeletedAt != null)
                .ToListAsync();

            if (deletedTownship == null)
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
                    Data = deletedTownship
                });
            }
        }
    }
}
