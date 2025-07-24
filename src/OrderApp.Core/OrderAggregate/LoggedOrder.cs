using OrderApp.Core.BaseAggregate;
using OrderApp.SharedKernel.Interfaces;

namespace OrderApp.Core.OrderAggregate;

public class LoggedOrder : OrderBase, ILoggedEntity, IAggregateRoot
{
        public int LogActionId { get; set; }
        public int OrderId { get; set; }
        public Order? Order{ get; set; }
}
