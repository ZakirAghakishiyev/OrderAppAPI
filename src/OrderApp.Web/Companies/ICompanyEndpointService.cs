using OrderApp.Web.Companies.Create;
using OrderApp.Web.Companies.Delete;
using OrderApp.Web.Companies.GetById;

namespace OrderApp.Web.Companies;

public interface ICompanyEndpointService
{
    public Task<CreateCompanyResponse> CreateAsync(CreateCompanyRequest req, CancellationToken ct);
    public Task<DeleteCompanyResponse?> DeleteAsync(DeleteCompanyRequest req, CancellationToken ct);
    public Task<CompanyListResponse> ListAsync(CancellationToken ct);
    public Task<GetCompanyByIdResponse?> GetByIdAsync(GetCompanyByIdRequest req, CancellationToken ct);
    public Task<UpdateCompanyResponse?> UpdateAsync(UpdateCompanyRequest req, CancellationToken ct);
}
