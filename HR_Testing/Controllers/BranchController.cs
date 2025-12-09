using Hr_Testing.Data;
using Hr_Testing.Entities;
using Hr_Testing.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;

namespace Hr_Testing.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BranchController(Hr_TestingDbContext context) : ControllerBase
    {
        [HttpGet]
        [EndpointSummary("Get all branches")]

        public async Task<IActionResult> GetAsync()
        {
            List<Branch> BranchData = await context.Branches.ToListAsync();
            if(BranchData == null)
            {
                return NotFound(new DefaultResponseModel()
                {
                    Success = false,
                    Statuscode = StatusCodes.Status404NotFound,
                    Message = "No branches found",
                    Data = null
                });
            }
            else
            {
                return Ok(new DefaultResponseModel()
                {
                    Success = true,
                    Statuscode = StatusCodes.Status200OK,
                    Message = "Branches retrieved successfully",
                    Data = BranchData
                });

            }
        }

        [HttpGet("{id}")]
        [EndpointSummary("Get the branch by Id")]
        public async Task<IActionResult> GetById(int id)
        {
            Branch? BranchData = await context.Branches.FirstOrDefaultAsync(e => e.BranchId == id);
            if (BranchData == null)
            {
                return NotFound(new DefaultResponseModel()
                {
                    Success = false,
                    Statuscode = StatusCodes.Status404NotFound,
                    Message = "Branches not found",
                    Data = null
                });
            }
            else
            {
                return Ok(new DefaultResponseModel()
                {
                    Success = true,
                    Statuscode = StatusCodes.Status200OK,
                    Message = "Branch Data is found",
                    Data = BranchData
                });
            }

        }

        [HttpGet("status")]
        [EndpointSummary("GetStatus")]
        public async Task<IActionResult> GetStatusAsync(bool status)
        {
            List<Branch> StatusData = await context.Branches.Where(e => e.Status == true).ToListAsync();
            if(StatusData == null)
            {
                return NotFound(new DefaultResponseModel()
                {
                    Success = false,
                    Statuscode = StatusCodes.Status404NotFound,
                    Message = "Status Not found",
                    Data = null
                });
            }
            else
            {
                return Ok(new DefaultResponseModel()
                {
                    Success = true,
                    Statuscode = StatusCodes.Status200OK,
                    Message = "Status found",
                    Data = StatusData
                });
            }
        }

        [HttpGet("Pagination")]
        [EndpointSummary("GetPagination")]
        public async Task<IActionResult> GetPagination(int skipRows, int pagesize, string? sortedField, string? q, int order)
        {
            IQueryable<Branch> Branches = BranchQuery(q, sortedField, order);

            int totalRecords = await context.Branches.CountAsync();
            List<Branch> records = await Branches
                .AsNoTracking()
                .Skip(skipRows)
                .Take(pagesize)
                .ToListAsync();

            return Ok(new DefaultResponseModel()
            {
                Success = true,
                Message = "success pagination",
                Statuscode = StatusCodes.Status200OK,
                Data = new { records, totalRecords }
            });

        }

        [HttpPost]
        [EndpointSummary("Create new branch")]
        public async Task<IActionResult> CreateAsync(Branch branch)
        {
            bool BranchExist = await context.Branches.AnyAsync(e => e.BranchId == branch.BranchId);
            if (BranchExist)
            {
                return Conflict(new DefaultResponseModel()
                {
                    Success = false,
                    Statuscode = StatusCodes.Status409Conflict,
                    Message = "Branch already exists",
                    Data = null
                });
            }

            branch.BranchId = branch.BranchId;
            branch.BranchName = branch.BranchName;
            branch.CompanyId = branch.CompanyId;
            branch.PhoneNumber = branch.PhoneNumber;
            branch.Email = branch.Email;
            branch.Status = true;
            branch.CreatedOn = DateTime.Now;
            branch.UpdatedOn = DateTime.Now;
            context.Branches.Add(branch);

            return await context.SaveChangesAsync() > 0
                ? Ok(new DefaultResponseModel()
                {
                    Success = true,
                    Statuscode = StatusCodes.Status200OK,
                    Message = "Created Successfully",
                    Data = branch
                })
                : BadRequest(new DefaultResponseModel()
                {
                    Success = false,
                    Statuscode = StatusCodes.Status400BadRequest,
                    Message = "Creation failed",
                    Data = null
                });
        }

        [HttpPut("{id}")]
        [EndpointSummary("Updated by id")]
        public async Task<IActionResult> UpdateByIdAsync(Branch branch)
        {
            Branch? BranchData = await context.Branches.FirstOrDefaultAsync(e => e.BranchId == branch.BranchId);
            if(BranchData == null)
            {
                return NotFound(new DefaultResponseModel()
                {
                    Success = false,
                    Statuscode = StatusCodes.Status200OK,
                    Message = "data not found",
                    Data = null
                });
            }

            BranchData.BranchId = branch.BranchId;
            BranchData.BranchName = branch.BranchName;
            BranchData.PhoneNumber = branch.PhoneNumber;
            BranchData.Email = branch.Email;
            BranchData.CompanyId = branch.CompanyId;
            BranchData.UpdatedOn = DateTime.Now;
            context.Branches.Update(BranchData);

            return await context.SaveChangesAsync() > 0
                ? Ok(new DefaultResponseModel()
                {
                    Success = true,
                    Statuscode = StatusCodes.Status200OK,
                    Message = "Updated Successfully",
                    Data = BranchData
                })
                : BadRequest(new DefaultResponseModel()
                {
                    Success = false,
                    Statuscode = StatusCodes.Status400BadRequest,
                    Message = "No Data",
                    Data = null
                });
        }

        [HttpDelete("{id}")]
        [EndpointSummary("DeleteById")]

        public async Task<IActionResult> DeleteByIdAsync(Branch branch)
        {
            Branch? BranchData = await context.Branches.FirstOrDefaultAsync(e => e.BranchId == branch.BranchId);
            if(BranchData == null)
            {
                return NotFound(new DefaultResponseModel()
                {
                    Success = false,
                    Statuscode = StatusCodes.Status404NotFound,
                    Message = "Data to delete not found",
                    Data = null
                });
            }

            BranchData.DeletedOn = branch.DeletedOn;
            context.Branches.Remove(BranchData);

            return await context.SaveChangesAsync() > 0
                ? Ok(new DefaultResponseModel()
                {
                    Success = true,
                    Statuscode = StatusCodes.Status200OK,
                    Message = "Deleted Successfully",
                    Data = null
                })
                : BadRequest(new DefaultResponseModel()
                {
                    Success = false,
                    Statuscode = StatusCodes.Status400BadRequest,
                    Message = "Delete failed",
                    Data = null
                });
        }

        [NonAction]
        private IQueryable<Branch> BranchQuery(string? q,string? sortedField,int order)
        {
            IQueryable<Branch> query = context.Branches.AsQueryable();

            if (!string.IsNullOrEmpty(sortedField))
            {
                query = query.OrderBy($"{sortedField} {(order > 0 ? "ascending" : "descending")}");
            }

            if (!string.IsNullOrEmpty(q))
            {
                q = q.ToLower();
                query = query.Where(
                    x => x.BranchId.ToString()!.Contains(q) ||
                    (x.BranchName ?? string.Empty).Contains(q));
            }
            return query;
        }
    }
}
