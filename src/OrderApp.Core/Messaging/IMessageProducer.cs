namespace OrderApp.Core.Messaging;

public interface IMessageProducer
{
    Task SendMessageAsync<T>(T message);
}