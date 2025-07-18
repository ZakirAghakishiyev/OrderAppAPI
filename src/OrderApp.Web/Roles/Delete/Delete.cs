// using System.Net;

// namespace OrderApp.Web.Roles.Delete;

// public class Delete(IRoleEndpointService _endpointService):Endpoint<DeleteRoleRequest, DeleteRoleResponse>
// {
//     public override void Configure()
//     {
//         Delete(DeleteRoleRequest.Route);
//         AllowAnonymous();
//     }

//     public override async Task<DeleteRoleResponse> HandleAsync(DeleteRoleRequest request, CancellationToken cancellationToken)
//     {
//         var response = await _endpointService.DeleteAsync(request, cancellationToken);
//         await SendAsync(response??throw new InvalidOperationException("Id not found"));
//         return response;
//     }
// }