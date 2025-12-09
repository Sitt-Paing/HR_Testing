using System.Linq.Dynamic.Core;

using Microsoft.AspNetCore.Mvc;
using Hr_Testing.Models;
using Microsoft.EntityFrameworkCore;
using Hr_Testing.Entities;
using Hr_Testing.Data;


namespace Hr_Testing.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PositionController(Hr_TestingDbContext context) : ControllerBase
    {
        [HttpGet]
        [EndpointSummary("Get all positions")]
        public async Task<IActionResult> GetAllAsync()
        {
            List<Position> positions = await context.Positions.ToListAsync();
            if (positions == null)
            {
                return NotFound(new DefaultResponseModel()
                {
                    Success = false,
                    Statuscode = StatusCodes.Status404NotFound,
                    Message = "No positions found",
                    Data = null
                });
            }
            else
            {
                return Ok(new DefaultResponseModel()
                {
                    Success = true,
                    Statuscode = StatusCodes.Status200OK,
                    Message = "Positions retrieved successfully",
                    Data = positions
                });
            }
        }

        [HttpGet("{id}")]
        [EndpointSummary("Get position by ID")]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            Position? PositionData = await context.Positions.FindAsync(id);
            if (PositionData == null)
            {
                return NotFound(new DefaultResponseModel()
                {
                    Success = false,
                    Statuscode = StatusCodes.Status404NotFound,
                    Message = "Position not found",
                    Data = null
                });
            }
            else
            {
                return Ok(new DefaultResponseModel()
                {
                    Success = true,
                    Statuscode = StatusCodes.Status200OK,
                    Message = "Position retrieved successfully",
                    Data = PositionData
                });
            }
        }

        [HttpGet("Status")]
        [EndpointSummary("Get positions by status")]
        public async Task<IActionResult> GetByStatusAsync(bool status)
        {
            List<Position> StatusData = await context.Positions.Where(e => e.Status == status).ToListAsync();
            if (StatusData == null)
            {
                return NotFound(new DefaultResponseModel()
                {
                    Success = false,
                    Statuscode = StatusCodes.Status404NotFound,
                    Message = "No positions found with the specified status",
                    Data = null
                });
            }
            else
            {
                return Ok(new DefaultResponseModel()
                {
                    Success = true,
                    Statuscode = StatusCodes.Status200OK,
                    Message = "Positions retrieved successfully",
                    Data = StatusData
                });
            }
        }

        [HttpGet("PositionPagination")]
        [EndpointSummary("GetPositionPagination")]

        public async Task<IActionResult> GetPagination(int skipRows, int pageSize, string? q, string? sortField, int order)
        {
            IQueryable<Position> postions = PositionQuery(q, sortField, order);

            int recordsTotal = await postions.CountAsync();
            List<Position> records = await postions
                    .AsNoTracking()
                    .Skip(skipRows)
                    .Take(pageSize)
                    .ToListAsync();


            return Ok(new DefaultResponseModel()
            {
                Success = true,
                Message = "success pagination",
                Statuscode = StatusCodes.Status200OK,
                Data = new { records, recordsTotal }
            });

        }

        [HttpGet("Creation time Order")]
        [EndpointSummary("GetCreationTimeOrder")]

        public async Task<IActionResult> GetAsync()
        {
            List<Position> creationTime = await context.Positions
                .Where(p => !p.DeletedOn.HasValue)
                .OrderByDescending(p => p.CreatedOn)
                .ToListAsync();

            if (creationTime.Count > 0)
            {
                return Ok(new DefaultResponseModel()
                {
                    Success = true,
                    Statuscode = StatusCodes.Status200OK,
                    Message = "Creation Time were retrieved successfully!",
                    Data = creationTime
                });
            }
            else
            {
                return NotFound(new DefaultResponseModel()
                {
                    Success = false,
                    Statuscode = StatusCodes.Status400BadRequest,
                    Message = "Retrieving data failed!",
                    Data = null
                });
            }
        }

        [HttpPost]
        [EndpointSummary("Create a new position")]
        public async Task<IActionResult> CreateAsync(Position position)
        {
            bool existingPosition = await context.Positions.AnyAsync(p => p.PositionId == position.PositionId);
            if (existingPosition)
            {
                return Conflict(new DefaultResponseModel()
                {
                    Success = false,
                    Statuscode = StatusCodes.Status409Conflict,
                    Message = "Position with the same PositionId already exists",
                    Data = null
                });
            }

            position.PositionId = position.PositionId;
            position.DepartmentId = position.DepartmentId;
            position.PositionName = position.PositionName;
            position.Status = true;
            position.CreatedOn = DateTime.Now;
            context.Positions.Add(position);

            return await context.SaveChangesAsync() > 0
                ? Ok(new DefaultResponseModel()
                {
                    Success = true,
                    Statuscode = StatusCodes.Status201Created,
                    Message = "Position created successfully",
                    Data = position
                })
                : BadRequest(new DefaultResponseModel()
                {
                    Success = false,
                    Statuscode = StatusCodes.Status400BadRequest,
                    Message = "Failed to create position",
                    Data = null
                });
        }

        [HttpPut("{id}")]
        [EndpointSummary("Update an existing position")]
        public async Task<IActionResult> UpdateAsync(Position updatedPosition)
        {
            Position? position = await context.Positions.FirstOrDefaultAsync(e => e.PositionId == updatedPosition.PositionId);
            if (position == null)
            {
                return NotFound(new DefaultResponseModel()
                {
                    Success = false,
                    Statuscode = StatusCodes.Status404NotFound,
                    Message = "Position not found",
                    Data = null
                });
            }
            position.PositionName = updatedPosition.PositionName;
            position.DepartmentId = updatedPosition.DepartmentId;
            position.Status = updatedPosition.Status;
            position.UpdatedOn = DateTime.UtcNow;
            context.Positions.Update(position);
            return await context.SaveChangesAsync() > 0
                ? Ok(new DefaultResponseModel()
                {
                    Success = true,
                    Statuscode = StatusCodes.Status200OK,
                    Message = "Position updated successfully",
                    Data = position
                })
                : BadRequest(new DefaultResponseModel()
                {
                    Success = false,
                    Statuscode = StatusCodes.Status400BadRequest,
                    Message = "Failed to update position",
                    Data = null
                });
        }

        [HttpDelete("{id}")]
        [EndpointSummary("Delete a position")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            Position? position = await context.Positions.FindAsync(id);
            if (position == null)
            {
                return NotFound(new DefaultResponseModel()
                {
                    Success = false,
                    Statuscode = StatusCodes.Status404NotFound,
                    Message = "Position not found",
                    Data = null
                });
            }

            position.DeletedOn = DateTime.UtcNow;
            //context.Positions.Remove(position);
            return await context.SaveChangesAsync() > 0
                ? Ok(new DefaultResponseModel()
                {
                    Success = true,
                    Statuscode = StatusCodes.Status200OK,
                    Message = "Position deleted successfully",
                    Data = null
                })
                : BadRequest(new DefaultResponseModel()
                {
                    Success = false,
                    Statuscode = StatusCodes.Status400BadRequest,
                    Message = "Failed to delete position",
                    Data = null
                });
        }

        [HttpGet("deleted")]
        [EndpointSummary("GetDeletedPositions")]
        public async Task<IActionResult> GetDeletedPositions()
        {
            List<Position> deletedPositions = await context.Positions
                .IgnoreQueryFilters()
                .Where(p => p.DeletedOn != null)
                .ToListAsync();

            return Ok(new DefaultResponseModel()
            {
                Success = true,
                Statuscode = StatusCodes.Status200OK,
                Message = "Deleted positions fetched successfully",
                Data = deletedPositions
            });
        }


        [NonAction]
        private IQueryable<Position> PositionQuery(string? q, string? sortField, int order)
        {
            IQueryable<Position> query = context.Positions.AsQueryable();


            // Sorting
            if (!string.IsNullOrEmpty(sortField))
            {
                query = query.OrderBy($"{sortField} {(order > 0 ? "ascending" : "descending")}");
            }

            // Filtering

            if (!string.IsNullOrEmpty(q))
            {
                q = q.ToLower();

                query = query.Where(
                    x => x.PositionId.ToString()!.Contains(q) ||
                    (x.PositionName ?? string.Empty).Contains(q));

            }

            return query;
        }
    }
}
