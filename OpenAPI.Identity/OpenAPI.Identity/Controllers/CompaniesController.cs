﻿using IntegrationEvents;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;
using OpenAPI.Identity.Data;
using SharedKernel;
using System.ComponentModel.DataAnnotations;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace OpenAPI.Identity.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompaniesController : ControllerBase
    {
        private readonly IRepository<Company,int> _companyRepository;
        private readonly IIntegrationEventService _integrationEventService;

        public CompaniesController(IRepository<Company,int> companyRepository, IIntegrationEventService integrationEventService)
        {
            _companyRepository = companyRepository;
            _integrationEventService = integrationEventService;
        }
        [HttpPost]
        public async Task<IActionResult> CreateCompany(string name, CancellationToken token)
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
            await _integrationEventService.AddEventAsync(new CompanyRegisteredIntegrationEvent(name, apiKey, apiSecret));
            await _integrationEventService.PublishEventsAsync(Guid.NewGuid(), token);
            return Ok(createdCompany);
        }
        [HttpGet("{id}")]
        [ServiceFilter(typeof(ApiKeyAuthAttribute))]
        public async Task<IActionResult> GetCompanyById([FromHeader, Required] string apiKey, [FromHeader, Required] string apiSecret, int id)
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
