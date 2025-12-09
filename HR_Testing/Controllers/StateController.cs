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
    public class StateController(Hr_TestingDbContext context) : ControllerBase
    {
        [HttpGet]
        [EndpointSummary("Get all states")]
        public async Task<IActionResult> GetAsync()
        {
            List<State> StateData = await context.States.ToListAsync();
            if (StateData == null)
            {
                return NotFound(new DefaultResponseModel()
                {
                    Success = false,
                    Statuscode = StatusCodes.Status404NotFound,
                    Message = "States not found",
                    Data = null
                });
            }
            else
            {
                return Ok(new DefaultResponseModel()
                {
                    Success = true,
                    Statuscode = StatusCodes.Status200OK,
                    Message = "States retrieved successfully",
                    Data = StateData
                });
            }
        }

        [HttpGet("{id}")]
        [EndpointSummary("Get the state by Id")]
        public async Task<IActionResult> GetbyIdAsync(int id)
        {
            State? StateData = await context.States.FirstOrDefaultAsync(s => s.StateId == id);
            if (StateData == null)
            {
                return NotFound(new DefaultResponseModel()
                {
                    Success = false,
                    Statuscode = StatusCodes.Status404NotFound,
                    Message = "State not found",
                    Data = null
                });
            }
            else
            {
                return Ok(new DefaultResponseModel()
                {
                    Success = true,
                    Statuscode = StatusCodes.Status200OK,
                    Message = "State retrieved successfully",
                    Data = StateData
                });
            }
        }

        [HttpPost]
        [EndpointSummary("Create a new state")]
        public async Task<IActionResult> CreateStateAsync(State state)
        {
            bool existingState = await context.States.AnyAsync(s => s.StateName == state.StateName);
            if (existingState)
            {
                return Conflict(new DefaultResponseModel()
                {
                    Success = false,
                    Statuscode = StatusCodes.Status409Conflict,
                    Message = "State with the same name already exists",
                    Data = state
                });
            }

            state.StateName = state.StateName;
            state.StateId = state.StateId;
            state.CreateAt = DateTime.UtcNow;
            context.States.Add(state);

            return await context.SaveChangesAsync() > 0
                ? Ok(new DefaultResponseModel()
                {
                    Success = true,
                    Statuscode = StatusCodes.Status201Created,
                    Message = "State created successfully",
                    Data = state
                })
                : BadRequest(new DefaultResponseModel()
                {
                    Success = false,
                    Statuscode = StatusCodes.Status400BadRequest,
                    Message = "Failed to create state",
                    Data = null
                });
        }

        [HttpPut("{id}")]
        [EndpointSummary("Update an existing state")]
        public async Task<IActionResult> UpdateAsync(State updatedState)
        {
            State? state = await context.States.FirstOrDefaultAsync(s => s.StateId == updatedState.StateId);
            if (state == null)
            {
                return NotFound(new DefaultResponseModel()
                {
                    Success = false,
                    Statuscode = StatusCodes.Status404NotFound,
                    Message = "State not found",
                    Data = null
                });
            }
            state.StateName = updatedState.StateName;
            state.UpdatedAt= DateTime.UtcNow;
            return await context.SaveChangesAsync() > 0
                ? Ok(new DefaultResponseModel()
                {
                    Success = true,
                    Statuscode = StatusCodes.Status200OK,
                    Message = "State updated successfully",
                    Data = state
                })
                : BadRequest(new DefaultResponseModel()
                {
                    Success = false,
                    Statuscode = StatusCodes.Status400BadRequest,
                    Message = "Failed to update state",
                    Data = null
                });
        }

        [HttpDelete("{id}")]
        [EndpointSummary("Delete a state")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            State? state = await context.States.FindAsync(id);
            if (state == null)
            {
                return NotFound(new DefaultResponseModel()
                {
                    Success = false,
                    Statuscode = StatusCodes.Status404NotFound,
                    Message = "State not found",
                    Data = null
                });
            }
            state.DeletedAt = DateTime.UtcNow;
            //context.States.Remove(state);
            return await context.SaveChangesAsync() > 0
                ? Ok(new DefaultResponseModel()
                {
                    Success = true,
                    Statuscode = StatusCodes.Status200OK,
                    Message = "State deleted successfully",
                    Data = null
                })
                : BadRequest(new DefaultResponseModel()
                {
                    Success = false,
                    Statuscode = StatusCodes.Status400BadRequest,
                    Message = "Failed to delete state",
                    Data = null
                });
        }


        [HttpGet("deleted")]
        [EndpointSummary("GetDeletedData")]
        public async Task<IActionResult> GetDeleteStreetAsync()
        {
            List<State> deletedState = await context.States
                .IgnoreQueryFilters()
                .Where(p => p.DeletedAt != null)
                .ToListAsync();

            if (deletedState == null)
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
                    Data = deletedState
                });
            }
        }
    }

}
