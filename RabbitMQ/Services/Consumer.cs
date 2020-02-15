using Microsoft.Extensions.Caching.Memory;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RabbitMQ.Services
{
    public class Consumer
    {
        private readonly IMemoryCache _memoryCache;
        ConnectionFactory _factory { get; set; }
        IConnection _connection { get; set; }
        IModel _channel { get; set; }

        public Consumer(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }

        public void ReceiveMessageFromQ()
        {
            try
            {
                _factory = new ConnectionFactory() { HostName = "54.202.17.105" };
                _connection = _factory.CreateConnection();
                _channel = _connection.CreateModel();

                {
                    _channel.QueueDeclare(queue: "counter",
                                         durable: true,
                                         exclusive: false,
                                         autoDelete: false,
                                         arguments: null);

                    _channel.BasicQos(prefetchSize: 0, prefetchCount: 3, global: false);

                    var consumer = new EventingBasicConsumer(_channel);
                    consumer.Received += async (model, ea) =>
                    {
                        var body = ea.Body;
                        var message = Encoding.UTF8.GetString(body);

                        Dictionary<string, int> messages = null;
                        _memoryCache.TryGetValue<Dictionary<string, int>>("messages", out messages);
                        if (messages == null) messages = new Dictionary<string, int>();

                        Console.WriteLine(" [x] Received {0}", message);
                        Thread.Sleep(3000);

                        messages.Remove(message);
                        _memoryCache.Set<Dictionary<string, int>>("messages", messages);
                        _channel.BasicAck(ea.DeliveryTag, false);
                    };

                    _channel.BasicConsume(queue: "counter",
                                         autoAck: false,
                                         consumer: consumer);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{ex.Message} | {ex.StackTrace}");
            }
        }
    }
}
