using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using OrderApp.Core.Messaging;
using RabbitMQ.Client;

namespace OrderApp.Infrastructure.Messaging;

public class RabbitMQProducer : IMessageProducer
    {
        public async Task SendMessageAsync<T>(T message)
        {
                    
            var factory = new ConnectionFactory() { HostName = "localhost", Port = 5672,UserName="guest", Password="guest" };
            var connection = await factory.CreateConnectionAsync();
            using (var channel = await connection.CreateChannelAsync())
            {

                await channel.QueueDeclareAsync(queue: "orders",
                                        durable: false,
                                        exclusive: false,
                                        autoDelete: false,
                                        arguments: null);
                // var json = JsonConvert.SerializeObject(message);
                // var body = Encoding.UTF8.GetBytes(json);
              
                byte[] messageBodyBytes = System.Text.Encoding.UTF8.GetBytes("Hello, world!");
                var props = new BasicProperties();
                props.ContentType = "text/plain";
                props.DeliveryMode = (DeliveryModes)2;
                await channel.BasicPublishAsync("", "orders",
                    mandatory: true, basicProperties: props, body: messageBodyBytes);

            }

        }
    }
