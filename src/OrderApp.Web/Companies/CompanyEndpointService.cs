using Ardalis.SharedKernel;
using AutoMap=AutoMapper;
using OrderApp.Web.Companies.Create;
using OrderApp.Web.Companies.Delete;
using OrderApp.Web.Companies.GetById;
using OrderApp.Core.CompanyAggregate;
using OrderApp.Web.Orders.Create;
using SH=OrderApp.SharedKernel.Interfaces;

namespace OrderApp.Web.Companies;

public class CompanyEndpointService(SH.IRepository<Company> _companyRepository, AutoMap.IMapper _mapper) : ICompanyEndpointService
{
    public async Task<Result<CreateCompanyResponse>> CreateAsync(CreateCompanyRequest req, CancellationToken ct)
    {
        try
        {
            var company = _mapper.Map<Company>(req);
            await _companyRepository.AddAsync(company, ct);
            return Result.Success(_mapper.Map<CreateCompanyResponse>(company));
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Unhandled");
            return null!;
        }
    }

    public async Task<DeleteCompanyResponse?> DeleteAsync(DeleteCompanyRequest req, CancellationToken ct)
    {
        var company = await _companyRepository.GetByIdAsync(req.CompanyId, ct);
        if (company == null)
        {
            return null;
        }
        await _companyRepository.DeleteAsync(company);
        return _mapper.Map<DeleteCompanyResponse>(company);
    }

    public async Task<GetCompanyByIdResponse?> GetByIdAsync(GetCompanyByIdRequest req, CancellationToken ct)
    {
        try
        {
            var company = await _companyRepository.GetByIdAsync(req.CompanyId, ct);
            return _mapper.Map<GetCompanyByIdResponse>(company);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Unhandled");
            return null!;
        }
    }

    public async Task<CompanyListResponse> ListAsync(CancellationToken ct)
    {
        var companies = await _companyRepository.ListAsync();
        var response = new CompanyListResponse();
        response.Companies=_mapper.Map<List<CompanyRecord>>(companies);
        return response;
    }

    public async Task<UpdateCompanyResponse?> UpdateAsync(UpdateCompanyRequest req, CancellationToken ct)
    {
        try
        {
            var company = _mapper.Map<Company>(req);
            await _companyRepository.UpdateAsync(company, ct);
            return _mapper.Map<UpdateCompanyResponse>(company);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Unhandled");
            return null;
        }
    }
}
