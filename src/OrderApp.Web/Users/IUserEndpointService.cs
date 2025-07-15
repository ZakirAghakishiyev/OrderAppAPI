using OrderApp.Web.Users.Create;
using OrderApp.Web.Users.Delete;
using OrderApp.Web.Users.GetById;
using OrderApp.Web.Users.List;
using OrderApp.Web.Users.Update;
using MO=OrderApp.Core.UserAggregate;

namespace OrderApp.Web.Users;

public interface IUserEndpointService
{
    public Task<UserListResponse> ListAsync(CancellationToken ct);
    public Task<CreateUserResponse> CreateAsync(CreateUserRequest req, CancellationToken ct);
    public Task<DeleteUserResponse?> DeleteAsync(DeleteUserRequest req, CancellationToken ct);
    public Task<GetUserByIdResponse?> GetByIdAsync(GetUserByIdRequest req, CancellationToken ct);
    public Task<UpdateUserResponse?> UpdateAsync(UpdateUserRequest req, CancellationToken ct); 
}
