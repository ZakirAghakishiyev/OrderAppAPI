using OrderApp.Core.UserAggregate;
using OrderApp.Core.UserAggregate.Specifications;
using OrderApp.SharedKernel.Interfaces;
using System.Net;
using System.Net.Mail;

namespace OrderApp.Web.RegisterEndpoint;

public class RegisterEndpointService(IRepository<User> _userRepository, MailService _mailService)
{
    public async Task<RegisterResponse?> RegisterAsync(RegisterRequest req, CancellationToken ct)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(req.Username) || string.IsNullOrWhiteSpace(req.Password) || string.IsNullOrWhiteSpace(req.Email))
            {
                throw new ArgumentException("Username, Password, and Email are required.");
            }

            var existingUser = await _userRepository.FirstOrDefaultAsync(new UserByMailorNameSpec(req.Email, req.Username), ct);
            if (existingUser != null)
            {
                throw new InvalidOperationException("User already exists.");
            }

            var user = new User() { Name = req.Username, Password = req.Password, Email = req.Email, Roles = new List<UserRole> { } };
            var createdUser = await _userRepository.AddAsync(user, ct);
            if (createdUser == null)
            {
                throw new InvalidOperationException("User creation failed.");
            }
            user.Roles.Add(new UserRole { UserId = createdUser.Id, RoleId = (int)RoleEnum.Customer });
            await _userRepository.UpdateAsync(user, ct);
            await _mailService.SendEmailAsync(
                req.Email,
                "Welcome to OrderApp!",
                $"<h3>Hello {req.Username},</h3><p>Thanks for registering at OrderApp!</p>"
            );
            return new RegisterResponse
            {
                Id = createdUser.Id,
                Username = user.Name,
                Email = user.Email
            };
        }
        catch (ArgumentException ex)
        {
            Log.Error(ex, "Missing required fields during registration.");
            return new RegisterResponse{Username=null!,Email=ex.Message};
        }
        catch (InvalidOperationException ex)
        {
            Log.Error(ex, "Already Exsisting User");
            return new RegisterResponse{Username=null!,Email=ex.Message};
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Exception Occured");
            return new RegisterResponse{Username=null!,Email=ex.Message};
        }
    }
}
