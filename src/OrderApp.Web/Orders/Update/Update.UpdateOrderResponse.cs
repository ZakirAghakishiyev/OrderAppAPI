namespace OrderApp.Web.Orders.Update;

public class UpdateOrderResponse
{
    public int Id { get; set; }
    public DateTime OrderDate { get; set; }
    public int UserId { get; set; }
    public int ProductId { get; set; }
}
