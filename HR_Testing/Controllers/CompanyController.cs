using Hr_Testing.Data;
using Hr_Testing.Entities;
using Hr_Testing.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;

namespace Hr_Testing.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompanyController(Hr_TestingDbContext context) : ControllerBase
    {
        [HttpGet]
        [EndpointSummary("Get all companies")]
        public async Task<IActionResult> GetAsync()
        {
            List<Company> CompaniesData = await context.Companies.ToListAsync();
            if (CompaniesData == null)
            {
                return NotFound(new DefaultResponseModel()
                {
                    Success = false,
                    Statuscode = StatusCodes.Status404NotFound,
                    Message = "Companies not found",
                    Data = null
                });
            }
            else
            {
                return Ok(new DefaultResponseModel()
                {
                    Success = true,
                    Statuscode = StatusCodes.Status200OK,
                    Message = "Companies are found",
                    Data = CompaniesData
                });
            }
        }

        [HttpGet("{Name}")]
        [EndpointSummary("get by Name")]

        public async Task<IActionResult> GetByNameAsync(string Name)
        {
            Company? CompanyData = await context.Companies.FirstOrDefaultAsync(x => x.CompanyName == Name);
            if (CompanyData == null)
            {
                return NotFound(new DefaultResponseModel()
                {
                    Success = false,
                    Statuscode = StatusCodes.Status404NotFound,
                    Message = "Company not found",
                    Data = null
                });
            }

            else
            {
                return Ok(new DefaultResponseModel()
                {
                    Success = true,
                    Statuscode = StatusCodes.Status200OK,
                    Message = "Company data retrieved successfully",
                    Data = CompanyData
                });
            }
        }

        [HttpPost]
        [EndpointSummary("Create Company")]

        public async Task<IActionResult> CreateAsync(Company company)
        {
            bool existedCompany = await context.Companies.AnyAsync(x => x.CompanyId == company.CompanyId);
            if (existedCompany)
            {
                return BadRequest(new DefaultResponseModel()
                {
                    Success = false,
                    Statuscode = StatusCodes.Status400BadRequest,
                    Message = "Company already exists",
                    Data = null
                });
            }

            company.JoinDate = DateOnly.FromDateTime(DateTime.Now);
            context.Companies.Add(company);

            return await context.SaveChangesAsync() > 0
                ? Ok(new DefaultResponseModel()
                {
                    Success = true,
                    Statuscode = StatusCodes.Status201Created,
                    Message = " created success",
                    Data = company
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
        [EndpointSummary("Update data")]

        public async Task<IActionResult> UpdateAsync(Company company)
        {
            Company? CompanyData = await context.Companies.FirstOrDefaultAsync(x => x.CompanyId == company.CompanyId || x.CompanyName == company.CompanyName);

            if (CompanyData == null)
            {
                return NotFound(new DefaultResponseModel()
                {
                    Success = false,
                    Statuscode = StatusCodes.Status404NotFound,
                    Message = "Company not found",
                    Data = null
                });
            }

            CompanyData.CompanyName = company.CompanyName;
            CompanyData.PrimaryPhone = company.PrimaryPhone;
            CompanyData.OtherPhone = company.OtherPhone;
            CompanyData.Email = company.Email;

            context.Companies.Update(CompanyData);

            return await context.SaveChangesAsync() > 0
                ? Ok(new DefaultResponseModel()
                {
                    Success = true,
                    Statuscode = StatusCodes.Status200OK,
                    Message = "Updated Successfully!",
                    Data = CompanyData
                })
                : BadRequest(new DefaultResponseModel()
                {
                    Success = false,
                    Statuscode = StatusCodes.Status400BadRequest,
                    Message = "Updates failed",
                    Data = null
                });
        }

        [HttpDelete]
        [EndpointSummary("Delete company data")]

        public async Task<IActionResult> DeleteAsync(string id)
        {
            Company? CompanyData = await context.Companies.FirstOrDefaultAsync(x => x.CompanyId == id);
            if (CompanyData == null)
            {
                return NotFound(new DefaultResponseModel()
                {
                    Success = false,
                    Statuscode = StatusCodes.Status404NotFound,
                    Message = "Data not found",
                    Data = null
                });
            }

            context.Companies.Remove(CompanyData);

            return await context.SaveChangesAsync() > 0
                ? Ok(new DefaultResponseModel()
                {
                    Success = true,
                    Statuscode = StatusCodes.Status200OK,
                    Message = "State data deleted successfully",
                    Data = null
                })
               : BadRequest(new DefaultResponseModel()
               {
                   Success = false,
                   Statuscode = StatusCodes.Status400BadRequest,
                   Message = "State data deleted failed",
                   Data = null
               });
        }
    }
}
