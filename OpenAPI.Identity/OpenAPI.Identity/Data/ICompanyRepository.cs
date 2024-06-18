namespace OpenAPI.Identity.Data
{
    public interface ICompanyRepository
    {
        Task<Company> CreateAsync(Company company);
        Task<Company?> GetByIdAsync(int id);
    }
}
