using GeekShopping.PaymentAPI.Messages;
using GeekShopping.MessageBus;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace GeekShopping.PaymentAPI.RabbitMQSender
{
    public class RabbitMQMessageSender : IRabbitMQMessageSender
    {
        private readonly string _hostName;
        private readonly string _password;
        private readonly string _username;
        private IConnection _connection;

        private const string ExchangeName = "FanoutPayementUpdateExchange";

        public RabbitMQMessageSender()
        {
            _hostName = "localhost";
            _password = "guest";
            _username = "guest";
        }

        public void SendMessage(BaseMessage baseMessage)
        {
            try
            {
                if (ConnectionExists())
                {
                    using var channel = _connection.CreateModel();

                    channel.ExchangeDeclare(ExchangeName, ExchangeType.Fanout, durable: false);

                    byte[] body = GetMessageAsByteArray(baseMessage);
                    channel.BasicPublish(exchange: ExchangeName, "", basicProperties: null, body: body);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private byte[] GetMessageAsByteArray(BaseMessage baseMessage)
        {
            var opt = new JsonSerializerOptions
            {
                WriteIndented = true,
            };

            var json = JsonSerializer.Serialize<UpdatePaymentResultMessage>((UpdatePaymentResultMessage)baseMessage, opt);

            return Encoding.UTF8.GetBytes(json);
        }

        private bool ConnectionExists()
        {
            if (_connection != null) return true;

            CreateConnection();

            return _connection != null;
        }

        private void CreateConnection()
        {
            try
            {
                var factory = new ConnectionFactory()
                {
                    HostName = _hostName,
                    Password = _password,
                    UserName = _username
                };

                _connection = factory.CreateConnection();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
