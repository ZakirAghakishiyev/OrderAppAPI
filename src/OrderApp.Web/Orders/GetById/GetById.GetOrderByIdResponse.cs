using OrderApp.Web.Products.GetById;
using OrderApp.Web.Users.GetById;
using Org.BouncyCastle.Bcpg;

namespace OrderApp.Web.Orders.GetById;

public class GetOrderByIdResponse
{
    public int Id { set; get; }
    public int userId { set; get; }
    public GetUserByIdResponse? User { set; get; }
    public int productId { set; get; }
    public GetProductByIdResponse? Product{ get; set; }
}
