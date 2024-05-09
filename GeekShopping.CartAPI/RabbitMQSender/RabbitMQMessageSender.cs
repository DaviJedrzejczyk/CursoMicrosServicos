using GeekShopping.CartAPI.Messages;
using GeekShopping.MessageBus;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace GeekShopping.CartAPI.RabbitMQSender
{
    public class RabbitMQMessageSender : IRabbitMQMessageSender
    {
        private readonly string _hostName;
        private readonly string _password;
        private readonly string _username;
        private IConnection _connection;

        public RabbitMQMessageSender()
        {
            _hostName = "localhost";
            _password = "guest";
            _username = "guest";
        }

        public void SendMessage(BaseMessage baseMessage, string queueName)
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

                using var channel = _connection.CreateModel();

                channel.QueueDeclare(queue: queueName, false, false, false, arguments: null);

                byte[] body = GetMessageAsByteArray(baseMessage);
                channel.BasicPublish(exchange: "", routingKey: queueName, basicProperties: null, body: body);
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

            var json = JsonSerializer.Serialize<CheckoutHeaderVO>((CheckoutHeaderVO)baseMessage, opt);

            return Encoding.UTF8.GetBytes(json);
        }
    }
}
