using AutoMapper;
using OrderModel=OrderApp.Core.OrderAggregate;
using OrderApp.Web.Orders.Create;
using OrderApp.Web.Orders.GetById;
using UserModel=OrderApp.Core.UserAggregate;
using OrderApp.Web.Users.GetById;
using OrderApp.Core.ProductAggregate;
using OrderApp.Web.Products.GetById;
using OrderApp.Core.CompanyAggregate;
using OrderApp.Web.Companies.GetById;
using OrderApp.Web.Orders.Delete;
using OrderApp.Web.Orders.Update;
using OrderApp.Web.Companies.Create;
using OrderApp.Web.Companies.Delete;
using OrderApp.Web.Companies;
using OrderApp.Core.UserAggregate;
using OrderApp.Web.Users.Create;
using OrderApp.Web.Users.Delete;
using OrderApp.Web.Users.Update;
using OrderApp.Web.Users.List;
using OrderApp.Web.Users;
using OrderApp.Web.Roles.Create;
using OrderApp.Web.Roles.Update;
using OrderApp.Web.Roles.GetById;
using OrderApp.Web.Roles;
using OrderApp.Web.Roles.List;
using OrderApp.Web.Roles.Delete;

namespace OrderApp.Web.Orders.Mappers;

public class Automapper : Profile
{
    public Automapper()
    {
        CreateMap<Company, CreateCompanyRequest>().ReverseMap();
        CreateMap<Company, CreateCompanyResponse>().ReverseMap();
        CreateMap<Company, UpdateCompanyRequest>().ReverseMap();
        CreateMap<Company, UpdateCompanyResponse>().ReverseMap();
        CreateMap<Company, GetCompanyByIdResponse>().ReverseMap();
        CreateMap<Company, CompanyRecord>().ReverseMap();
        CreateMap<Company, CompanyListResponse>().ReverseMap();
        CreateMap<Company, GetCompanyByIdRequest>().ReverseMap();
        CreateMap<Company, UpdateCompanyRequest>().ReverseMap();
        CreateMap<Company, UpdateCompanyResponse>().ReverseMap();
        CreateMap<Company, DeleteCompanyRequest>().ReverseMap();
        CreateMap<Company, DeleteCompanyResponse>().ReverseMap();

        CreateMap<Role, CreateRoleRequest>().ReverseMap();
        CreateMap<Role, CreateRoleResponse>().ReverseMap();
        CreateMap<Role, UpdateRoleRequest>().ReverseMap();
        CreateMap<Role, UpdateRoleResponse>().ReverseMap();
        CreateMap<Role, GetRoleByIdResponse>().ReverseMap();
        CreateMap<Role, RoleRecord>().ReverseMap();
        CreateMap<Role, RoleListResponse>().ReverseMap();
        CreateMap<Role, GetRoleByIdRequest>().ReverseMap();
        CreateMap<Role, UpdateRoleRequest>().ReverseMap();
        CreateMap<Role, UpdateRoleResponse>().ReverseMap();
        CreateMap<Role, DeleteRoleRequest>().ReverseMap();
        CreateMap<Role, DeleteRoleResponse>().ReverseMap();

        CreateMap<CreateUserRequest, User>()
            .ForMember(dest => dest.Roles, opt => opt.MapFrom(src =>
                src.Roles.Select(roleId => new UserRole { RoleId = roleId }).ToList()
            ));
        CreateMap<User, CreateUserResponse>().ReverseMap();
        CreateMap<User, DeleteUserResponse>().ReverseMap();
        CreateMap<User, DeleteUserRequest>().ReverseMap();
        CreateMap<User, UpdateUserRequest>()
            .ForMember(dest => dest.Roles, opt => opt.MapFrom(src =>
                src.Roles.Select(ur => (RoleEnum)ur.RoleId)))
            .ReverseMap()
            .ForMember(dest => dest.Roles, opt => opt.MapFrom(src =>
                src.Roles.Select(role => new UserRole { RoleId = (int)role })));

        CreateMap<User, UpdateUserResponse>()
            .ForCtorParam("id", opt => opt.MapFrom(src => src.Id))
            .ForCtorParam("name", opt => opt.MapFrom(src => src.Name))
            .ForCtorParam("email", opt => opt.MapFrom(src => src.Email))
            .ForCtorParam("password", opt => opt.MapFrom(src => src.Password))
            .ForCtorParam("Roles", opt => opt.MapFrom(src =>
                src.Roles.Select(ur => (RoleEnum)ur.RoleId)))
            .ReverseMap()
            .ForMember(dest => dest.Roles, opt => opt.MapFrom(src =>
                src.Roles.Select(role => new UserRole { RoleId = (int)role })));

        CreateMap<User, UserListResponse>().ReverseMap();
        CreateMap<User, UserRecord>()
            .ForCtorParam("Roles", opt => opt.MapFrom(src =>
                src.Roles.Select(ur => (RoleEnum)ur.RoleId)))
            .ReverseMap()
            .ForMember(dest => dest.Roles, opt => opt.MapFrom(src =>
                src.Roles.Select(role => new UserRole { RoleId = (int)role })));
        CreateMap<User, GetUserByIdRequest>().ReverseMap();
        CreateMap<User, GetUserByIdResponse>()
            .ForMember(dest => dest.Roles, opt => opt.MapFrom(src =>
                src.Roles.Select(ur => (RoleEnum)ur.RoleId)))
            .ReverseMap()
            .ForMember(dest => dest.Roles, opt => opt.MapFrom(src =>
                src.Roles.Select(role => new UserRole { RoleId = (int)role })));
                
        CreateMap<OrderModel.Order, CreateOrderResponse>().ReverseMap();
        CreateMap<OrderModel.Order, CreateOrderRequest>().ReverseMap();
        CreateMap<OrderModel.Order, UpdateOrderResponse>().ReverseMap();
        CreateMap<OrderModel.Order, OrderRecord>().ReverseMap();
        CreateMap<OrderModel.Order, GetOrderByIdResponse>()
            .ForMember(dest => dest.User, opt => opt.MapFrom(src => src.User))
            .ForMember(dest => dest.Product, opt => opt.MapFrom(src => src.Product));
        CreateMap<GetOrderByIdResponse, OrderRecord>();
        CreateMap<Product, GetProductByIdResponse>()
            .ForMember(dest => dest.Company, opt => opt.MapFrom(src => src.Company))
            .ReverseMap();
        CreateMap<OrderModel.Order, DeleteOrderResponse>().ReverseMap();
    }
}
