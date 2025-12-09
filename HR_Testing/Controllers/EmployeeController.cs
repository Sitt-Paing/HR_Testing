using Hr_Testing.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Hr_Testing.Models;
using Microsoft.EntityFrameworkCore;

using Microsoft.Identity.Client;
using Hr_Testing.Data;

namespace Hr_Testing.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController(Hr_TestingDbContext context) : ControllerBase
    {
        [HttpGet]
        [EndpointSummary("Get all employees")]

        public async Task<IActionResult> GetAsync()
        {
            List<Employee> EmployeeData = await context.Employees.ToListAsync();
            if(EmployeeData == null)
            {
                return NotFound(new DefaultResponseModel()
                {
                    Success = false,
                    Statuscode = StatusCodes.Status404NotFound,
                    Message = "Employees not found",
                    Data = null
                });
            }
            else
            {
                return Ok(new DefaultResponseModel()
                {
                    Success = true,
                    Statuscode = StatusCodes.Status200OK,
                    Message = "Employees retrieved successfully",
                    Data = EmployeeData
                });
            }
        }

        [HttpGet("{Name}")]
        [EndpointSummary("Get employee by Name")]

        public async Task<IActionResult> GetByEmployeeName(string Name)
        {
            Employee? EmployeeData = await context.Employees.FirstOrDefaultAsync(e => e.FullName == Name);
            if (EmployeeData == null)
            {
                return NotFound(new DefaultResponseModel()
                {
                    Success = false,
                    Statuscode = StatusCodes.Status404NotFound,
                    Message = "Employee not found",
                    Data = null
                });

            }
            else
            {
                return Ok(new DefaultResponseModel()
                {
                    Success = true,
                    Statuscode = StatusCodes.Status200OK,
                    Message = "Employee retrieved successfully",
                    Data = EmployeeData
                });
            }
        }

        [HttpPost]
        [EndpointSummary("Create a new employee")]
        public async Task<IActionResult> CreateAsync(Employee employee)
        {
            bool Employee = await context.Employees.AnyAsync(e => e.FullName == employee.FullName);
            if (Employee)
            {
                return Conflict(new DefaultResponseModel()
                {
                    Success = false,
                    Statuscode = StatusCodes.Status409Conflict,
                    Message = "Employee already exists",
                    Data = null
                });
            }

            employee.FullName = employee.FullName;
            employee.JoinDate = DateOnly.FromDateTime(DateTime.Now);
            employee.DateOfBirth = employee.DateOfBirth;
            employee.Gender = employee.Gender;
            employee.Nrc = employee.Nrc;

            context.Employees.Add(employee);

            return await context.SaveChangesAsync() > 0
                ? Ok(new DefaultResponseModel()
                {
                    Success = true,
                    Statuscode = StatusCodes.Status201Created,
                    Message = " created success",
                    Data = employee
                })
                : BadRequest(new DefaultResponseModel()
                {
                    Success = false,
                    Statuscode = StatusCodes.Status400BadRequest,
                    Message = "created Failed",
                    Data = null
                });
        }

        [HttpPut]
        [EndpointSummary("Update employee data")]
        public async Task<IActionResult> UpdateAsync(Employee employee)
        {
            Employee? EmployeeData = await context.Employees.FirstOrDefaultAsync(e => e.EmployeeId == employee.EmployeeId);
            if (EmployeeData == null)
            {
                return NotFound(new DefaultResponseModel()
                {
                    Success = false,
                    Statuscode = StatusCodes.Status404NotFound,
                    Message = "Data to update not found",
                    Data = null
                });
            }

            EmployeeData.FullName = employee.FullName;
            EmployeeData.Gender = employee.Gender;
            EmployeeData.Nrc = employee.Nrc;
            EmployeeData.DateOfBirth = employee.DateOfBirth;
            context.Employees.Update(employee);

            return await context.SaveChangesAsync() > 0
                ? Ok(new DefaultResponseModel()
                {
                    Success = true,
                    Statuscode = StatusCodes.Status200OK,
                    Message = "Updated Successfully.",
                    Data = EmployeeData
                })
                : BadRequest(new DefaultResponseModel()
                {
                    Success = false,
                    Statuscode = StatusCodes.Status400BadRequest,
                    Message = "Update failed",
                    Data = null
                });
        }

        [HttpDelete("{id}")]
        [EndpointSummary("Delete an employee")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            Employee? employee = await context.Employees.FindAsync(id);
            if (employee == null)
            {
                return NotFound(new DefaultResponseModel()
                {
                    Success = false,
                    Statuscode = StatusCodes.Status404NotFound,
                    Message = "Employee not found",
                    Data = null
                });
            }
            context.Employees.Remove(employee);
            return await context.SaveChangesAsync() > 0
                ? Ok(new DefaultResponseModel()
                {
                    Success = true,
                    Statuscode = StatusCodes.Status200OK,
                    Message = "Employee deleted successfully",
                    Data = null
                })
                : BadRequest(new DefaultResponseModel()
                {
                    Success = false,
                    Statuscode = StatusCodes.Status400BadRequest,
                    Message = "Failed to delete employee",
                    Data = null
                });
        }
    }
}
