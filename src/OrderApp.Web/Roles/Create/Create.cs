// using System.Net;

// namespace OrderApp.Web.Roles.Create;

// public class Create(IRoleEndpointService _endpointService):Endpoint<CreateRoleRequest, CreateRoleResponse>
// {
//     public override void Configure()
//     {
//         Post(CreateRoleRequest.Route);
//     }

//     public override async Task<CreateRoleResponse> HandleAsync(CreateRoleRequest request, CancellationToken cancellationToken)
//     {
//         if (request == null)
//         {
//             await SendAsync(new CreateRoleResponse(1, "Invalid request"), (int)HttpStatusCode.BadRequest, cancellationToken);
//             return null!;
//         }

//         var response = await _endpointService.CreateAsync(request, cancellationToken);
//         await SendAsync(response);
//         return response;
//     }
// }