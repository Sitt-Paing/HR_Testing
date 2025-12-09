using Hr_Testing.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Hr_Testing.Models;
using Microsoft.EntityFrameworkCore;
using Hr_Testing.Data;


namespace Hr_Testing.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentController(Hr_TestingDbContext context) : ControllerBase
    {
        [HttpGet]
        [EndpointSummary("Get all departments")]
        public async Task<IActionResult> GetAsync()
        {
            List<Department> DepartmentData = await context.Departments.ToListAsync();
            if (DepartmentData == null)
            {
                return NotFound(new DefaultResponseModel()
                {
                    Success = false,
                    Statuscode = StatusCodes.Status404NotFound,
                    Message = "Departments not found",
                    Data = null
                });
            }

            else
            {
                return Ok(new DefaultResponseModel()
                {
                    Success = true,
                    Statuscode = StatusCodes.Status200OK,
                    Message = "Departments retrieved successfully",
                    Data = DepartmentData
                });
            }
        }
        [HttpGet("{id}")]
        [EndpointSummary("Get department by Name")]
        public async Task<IActionResult> GetByDepartmentName(string departmentName)
        {
            Department? DepartmentData = await context.Departments.FirstOrDefaultAsync(d => d.DepartmentName == departmentName);
            if (DepartmentData == null)
            {
                return NotFound(new DefaultResponseModel()
                {
                    Success = false,
                    Statuscode = StatusCodes.Status404NotFound,
                    Message = "Department not found",
                    Data = null
                });
            }
            else
            {
                return Ok(new DefaultResponseModel()
                {
                    Success = true,
                    Statuscode = StatusCodes.Status200OK,
                    Message = "Department retrieved successfully",
                    Data = DepartmentData
                });
            }
        }

        [HttpGet("status")]
        [EndpointSummary("Get departments by status")]  
        public async Task<IActionResult> GetByStatusAsync(bool status)
        {
            List<Department> DepartmentData = await context.Departments.Where(d => d.Status == true).ToListAsync();
            if (DepartmentData == null || DepartmentData.Count == 0)
            {
                return NotFound(new DefaultResponseModel()
                {
                    Success = false,
                    Statuscode = StatusCodes.Status404NotFound,
                    Message = "No departments found with the specified status",
                    Data = null
                });
            }
            else
            {
                return Ok(new DefaultResponseModel()
                {
                    Success = true,
                    Statuscode = StatusCodes.Status200OK,
                    Message = "Departments retrieved successfully",
                    Data = DepartmentData
                });
            }
        }


        [HttpPost]
        [EndpointSummary("Create a new department")]
        public async Task<IActionResult> CreateAsync(Department department)
        {
            if (await context.Departments.AnyAsync(d => d.DepartmentName == department.DepartmentName))
            {
                return Conflict(new DefaultResponseModel()
                {
                    Success = false,
                    Statuscode = StatusCodes.Status409Conflict,
                    Message = "Department with the same name already exists",
                    Data = null
                });
            }
            else
            {

                context.Departments.Add(department);
                department.Status = true;

                return await context.SaveChangesAsync() > 0
                    ? Ok(new DefaultResponseModel()
                    {
                        Success = true,
                        Statuscode = StatusCodes.Status201Created,
                        Message = "Department created successfully",
                        Data = department
                    })
                    : BadRequest(new DefaultResponseModel()
                    {
                        Success = false,
                        Statuscode = StatusCodes.Status400BadRequest,
                        Message = "Failed to create department",
                        Data = null
                    });
            }
        }

        [HttpPut("{id}")]
        [EndpointSummary("Update State")]

        public async Task<IActionResult> UpdateAsync(Department department)
        {
            Department? DepartmentData = await context.Departments.FirstOrDefaultAsync(x => x.DepartmentId == department.DepartmentId);

            if (DepartmentData == null)
            {
                return NotFound(new DefaultResponseModel()
                {
                    Success = false,
                    Statuscode = StatusCodes.Status404NotFound,
                    Message = "State data not found",
                    Data = null
                });
            }

            DepartmentData.DepartmentName = department.DepartmentName;

            context.Departments.Update(DepartmentData);

            return await context.SaveChangesAsync() > 0
                ? Ok(new DefaultResponseModel()
                {
                    Success = true,
                    Statuscode = StatusCodes.Status200OK,
                    Message = "Department data updated successfully",
                    Data = DepartmentData
                })
                : BadRequest(new DefaultResponseModel()
                {
                    Success = false,
                    Statuscode = StatusCodes.Status400BadRequest,
                    Message = "Department data update failed",
                    Data = null
                });

        }

        [HttpDelete("{Name}")]
        [EndpointSummary("Delete department by Name")]
        public async Task<IActionResult> DeleteByName(string Name)
        {
            Department? DepartmentData = await context.Departments.FirstOrDefaultAsync(d => d.DepartmentName == Name);
            if (DepartmentData == null)
            {
                return NotFound(new DefaultResponseModel()
                {
                    Success = false,
                    Statuscode = StatusCodes.Status404NotFound,
                    Message = "Department not found",
                    Data = null
                });
            }

            context.Departments.Remove(DepartmentData);

            return await context.SaveChangesAsync() > 0
                ? Ok(new DefaultResponseModel()
                {
                    Success = true,
                    Statuscode = StatusCodes.Status200OK,
                    Message = "Department deleted successfully",
                    Data = null
                })
                : BadRequest(new DefaultResponseModel()
                {
                    Success = false,
                    Statuscode = StatusCodes.Status400BadRequest,
                    Message = "Failed to delete department",
                    Data = null
                });
        }
    }
}
