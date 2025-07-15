using Ardalis.SharedKernel;
using Mo=OrderApp.Core.UserAggregate;
using OrderApp.Web.Roles.Create;
using OrderApp.Web.Roles.Delete;
using OrderApp.Web.Roles.GetById;
using OrderApp.Web.Roles.List;
using OrderApp.Web.Roles.Update;
using AutoMap=AutoMapper;
using OrderApp.Core.UserAggregate;
using SK=OrderApp.SharedKernel.Interfaces;
using OrderApp.Core.UserAggregate.Specification;

namespace OrderApp.Web.Roles;

public class RoleEndpointService(SK.IRepository<Mo.Role> _repository, AutoMap.IMapper _mapper) : IRoleEndpointService
{
    public async Task<CreateRoleResponse> CreateAsync(CreateRoleRequest req, CancellationToken ct)
    {
        try
        {
            var role = _mapper.Map<Role>(req);

            var existingRole = await _repository.FirstOrDefaultAsync(new RoleByNameSpec(role.Name));
            if (existingRole is not null)
                throw new Exception("Role already exists");
            var res = await _repository.AddAsync(role);
            return _mapper.Map<CreateRoleResponse>(res);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Unhandled");
            return null!;
        }
    }

    public async Task<DeleteRoleResponse?> DeleteAsync(DeleteRoleRequest req, CancellationToken ct)
    {
        try
        {
            if (req == null)
                throw new NullReferenceException();
            var role = _mapper.Map<Role>(req);
            await _repository.DeleteAsync(role, ct);
            return _mapper.Map<DeleteRoleResponse>(role);
        }
        catch (NullReferenceException ex)
        {
            Log.Error(ex, "Request cannot be null");
            return null;
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Unhandled");
            return null;
        }
    }

    public Task<GetRoleByIdResponse?> GetByIdAsync(GetRoleByIdRequest req, CancellationToken ct)
    {
        throw new NotImplementedException();
    }

    public async Task<RoleListResponse> ListAsync(CancellationToken ct)
    {
        try
        {
            var roles = await _repository.ListAsync();
            var response = new RoleListResponse();
            response.Roles = _mapper.Map<List<RoleRecord>>(roles);
            return response;
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Unhandled");
            return new RoleListResponse { Roles = new List<RoleRecord>() };
        }
    }

    public Task<UpdateRoleResponse?> UpdateAsync(UpdateRoleRequest req, CancellationToken ct)
    {
        throw new NotImplementedException();
    }

}
