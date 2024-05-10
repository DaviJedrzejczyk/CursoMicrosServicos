
using GeekShopping.Email.Messages;
using GeekShopping.Email.Model;
using GeekShopping.Email.Repository;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

namespace GeekShopping.Email.MessageConsumer
{
    public class RabbitMQPayementConsumer : BackgroundService
    {
        private readonly EmailRepository _repository;
        private IConnection _connection;
        private IModel _channel;

        private const string ExchangeName = "DirectPayementUpdateExchange";
        private const string PayementEmailUpdateQueueName = "PayementEmailUpdateQueueName";

        string queueName = "";

        public RabbitMQPayementConsumer(EmailRepository repository)
        {
            _repository = repository;

            var factory = new ConnectionFactory()
            {
                HostName = "localhost",
                Password = "guest",
                UserName = "guest",
            };

            _connection = factory.CreateConnection();

            _channel = _connection.CreateModel();

            //_channel.ExchangeDeclare(ExchangeName, ExchangeType.Fanout); Caso queira usar o Fanout

            _channel.ExchangeDeclare(ExchangeName, ExchangeType.Direct);

            //queueName = _channel.QueueDeclare().QueueName; somente usa se for usar o Fanout

            _channel.QueueDeclare(PayementEmailUpdateQueueName, false, false, false, null);

            _channel.QueueBind(PayementEmailUpdateQueueName, ExchangeName, "PayementEmail");
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.ThrowIfCancellationRequested();
            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += (chanel, evt) =>
            {
                var content = Encoding.UTF8.GetString(evt.Body.ToArray());
                UpdatePayamentResultMessage message = JsonSerializer.Deserialize<UpdatePayamentResultMessage>(content);

                ProcessLogs(message).GetAwaiter().GetResult();

                _channel.BasicAck(evt.DeliveryTag, false);
            };

            _channel.BasicConsume(PayementEmailUpdateQueueName, false, consumer);

            return Task.CompletedTask;
        }

        private async Task ProcessLogs(UpdatePayamentResultMessage message)
        {
            try
            {
                await _repository.LogEmail(message);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
