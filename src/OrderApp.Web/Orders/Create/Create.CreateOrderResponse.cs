namespace OrderApp.Web.Orders.Create;

public class CreateOrderResponse
{

    public CreateOrderResponse(int id, DateTime orderDate, int userId, int productId)
    {
        Id = id;
        OrderDate = orderDate;
        UserId = userId;
        ProductId = productId;
    }
    public CreateOrderResponse()
    {

    }
    public int Id { get; set; }
    public DateTime OrderDate { get; set; }
    public int UserId { get; set; }
    public int ProductId { get; set; }
}
