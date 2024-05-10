using GeekShopping.PayamentProcessor;
using GeekShopping.PaymentAPI.Messages;
using GeekShopping.PaymentAPI.RabbitMQSender;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

namespace GeekShopping.OrderApi.MessageConsumer
{
    public class RabbitMQPaymentConsumer : BackgroundService
    {
        private IConnection _connection;
        private IModel _channel;
        private readonly IProcessPayament _processPayament;
        private IRabbitMQMessageSender _messageSender;

        public RabbitMQPaymentConsumer(IProcessPayament processPayament, IRabbitMQMessageSender rabbitMQMessageSender)
        {
            _processPayament = processPayament;
            _messageSender = rabbitMQMessageSender;
            var factory = new ConnectionFactory()
            {
                HostName = "localhost",
                Password = "guest",
                UserName = "guest",
            };

            _connection = factory.CreateConnection();

            _channel = _connection.CreateModel();

            _channel.QueueDeclare(queue: "orderpayamentprocessqueue", false, false, false, arguments: null);
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.ThrowIfCancellationRequested();
            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += (chanel, evt) =>
            {
                var content = Encoding.UTF8.GetString(evt.Body.ToArray());
                PaymentMessage vo = JsonSerializer.Deserialize<PaymentMessage>(content);

                ProcessPayement(vo).GetAwaiter().GetResult();

                _channel.BasicAck(evt.DeliveryTag, false);
            };

            _channel.BasicConsume("orderpayamentprocessqueue", false, consumer);

            return Task.CompletedTask;
        }

        private async Task ProcessPayement(PaymentMessage vo)
        {
            var result = _processPayament.PayamentProcessor();

            UpdatePaymentResultMessage paymentResultMessage = new()
            {
                Status = result,
                OrderID = vo.OrderID,
                Email = vo.Email,
            };

            try
            {
                _messageSender.SendMessage(paymentResultMessage);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
