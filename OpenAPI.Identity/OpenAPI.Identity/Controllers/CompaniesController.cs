using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;
using OpenAPI.Identity.Data;

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
        public async Task<IActionResult> CreateCompany(string name)
        {
            var apiKey = KeyGenerator.GenerateApiKey();
            var apiSecret = KeyGenerator.GenerateApiSecret();  
            var company = new Company
            {
                Name = name,
                APIKey = apiKey,
                APISecret = apiSecret
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
