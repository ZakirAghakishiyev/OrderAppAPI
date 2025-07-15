using OrderApp.Core.UserAggregate;
using SK=OrderApp.SharedKernel.Interfaces;
using OrderApp.Web.Users.Create;
using OrderApp.Web.Users.Delete;
using OrderApp.Web.Users.GetById;
using OrderApp.Web.Users.Update;
using MO = OrderApp.Core.UserAggregate;
using AutoMap = AutoMapper;
using Ardalis.GuardClauses;
using OrderApp.Web.Users.List;


namespace OrderApp.Web.Users;

public class UserEndpointService(SK.IRepository<User> _userRepository, AutoMap.IMapper _mapper) : IUserEndpointService
{
    public async Task<CreateUserResponse> CreateAsync(CreateUserRequest req, CancellationToken ct)
    {
        try
        {
            if (req == null)
                throw new Exception("Request is null");
            var user = _mapper.Map<User>(req);
            await _userRepository.AddAsync(user, ct);
            return _mapper.Map<CreateUserResponse>(user);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Unhandled");
            return null!;
        }
    }

    public async Task<DeleteUserResponse?> DeleteAsync(DeleteUserRequest req, CancellationToken ct)
    {
        try
        {
            if (req == null)
                throw new Exception("Request is null");
            var user = await _userRepository.GetByIdAsync(req.UserId, ct);
            if (user == null)
                throw new NullReferenceException("No such user");
            await _userRepository.DeleteAsync(user);
            return _mapper.Map<DeleteUserResponse>(user);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Unhandled");
            return null!;
        }
    }

    public async Task<GetUserByIdResponse?> GetByIdAsync(GetUserByIdRequest req, CancellationToken ct)
    {
        try
        {
            if (req == null)
                throw new Exception("Request is null");
            var user = await _userRepository.GetByIdAsync(req.UserId, ct);
            if (user == null)
                throw new NullReferenceException("No such user");
            return _mapper.Map<GetUserByIdResponse>(user);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Unhandled");
            return null!;
        }
    }

    public async Task<UserListResponse> ListAsync(CancellationToken ct)
    {
        var users = await _userRepository.ListAsync(ct);
        var res = new UserListResponse();
        res.Users=_mapper.Map<List<UserRecord>>(users);
        return res;
    }

    public async Task<UpdateUserResponse?> UpdateAsync(UpdateUserRequest req, CancellationToken ct)
    {
        try
        {
            if (req == null)
                throw new Exception("Request is null");
            var user = await _userRepository.GetByIdAsync(req.UserId, ct);
            if (user == null)
                throw new NullReferenceException("No such user");
            user.Name = req.Name;
            await _userRepository.UpdateAsync(user, ct);
            return _mapper.Map<UpdateUserResponse>(user);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Unhandled");
            return null!;
        }
    }
}
