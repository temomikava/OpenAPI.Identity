using Microsoft.AspNetCore.Mvc;
using OpenAPI.Identity.Data;
using OpenAPI.Identity.Dtos;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace OpenAPI.Identity.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompaniesController : ControllerBase
    {
        private readonly ICompanyRepository _companyRepository;

        public CompaniesController(ICompanyRepository companyRepository)
        {
            _companyRepository = companyRepository;
        }
        [HttpPost]
        public async Task<IActionResult> CreateCompany(CompanyDto companyDto)
        {
            var company = new Company
            {
                Name = companyDto.Name,
                APIKey = Guid.NewGuid().ToString(),
                APISecret = Guid.NewGuid().ToString()
            };
            var createdCompany = await _companyRepository.CreateAsync(company);
            return Ok(createdCompany);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCompanyById(int id)
        {
            var company = await _companyRepository.GetByIdAsync(id);
            if (company == null)
            {
                return NotFound();
            }
            return Ok(company);
        }
    }
}
