using OrderApp.Web.Roles.Create;
using OrderApp.Web.Roles.Delete;
using OrderApp.Web.Roles.GetById;
using OrderApp.Web.Roles.List;
using OrderApp.Web.Roles.Update;

namespace OrderApp.Web.Roles;

public interface IRoleEndpointService
{
    public Task<CreateRoleResponse> CreateAsync(CreateRoleRequest req, CancellationToken ct);
    public Task<DeleteRoleResponse?> DeleteAsync(DeleteRoleRequest req, CancellationToken ct);
    public Task<RoleListResponse> ListAsync(CancellationToken ct);
    public Task<GetRoleByIdResponse?> GetByIdAsync(GetRoleByIdRequest req, CancellationToken ct);
    public Task<UpdateRoleResponse?> UpdateAsync(UpdateRoleRequest req, CancellationToken ct);
}
