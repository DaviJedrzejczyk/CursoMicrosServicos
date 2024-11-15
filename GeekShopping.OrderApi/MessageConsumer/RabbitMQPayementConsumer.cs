﻿
using GeekShopping.OrderApi.Messages;
using GeekShopping.OrderApi.Model;
using GeekShopping.OrderApi.RabbitMQSender;
using GeekShopping.OrderApi.Repository;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

namespace GeekShopping.OrderApi.MessageConsumer
{
    public class RabbitMQPayementConsumer : BackgroundService
    {
        private readonly OrderRepository _repository;
        private IConnection _connection;
        private IModel _channel;

        private const string ExchangeName = "DirectPayementUpdateExchange";
        private const string PayementOrderUpdateQueueName = "PayementOrderUpdateQueueName";
        string queueName = "";

        public RabbitMQPayementConsumer(OrderRepository repository)
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

            _channel.QueueDeclare(PayementOrderUpdateQueueName, false, false, false, null);

            _channel.QueueBind(PayementOrderUpdateQueueName, ExchangeName, "PayementOrder");
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.ThrowIfCancellationRequested();
            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += (chanel, evt) =>
            {
                var content = Encoding.UTF8.GetString(evt.Body.ToArray());
                UpdatePayamentResultVO vo = JsonSerializer.Deserialize<UpdatePayamentResultVO>(content);

                UpdatePayementStatus(vo).GetAwaiter().GetResult();

                _channel.BasicAck(evt.DeliveryTag, false);
            };

            _channel.BasicConsume(PayementOrderUpdateQueueName, false, consumer);

            return Task.CompletedTask;
        }

        private async Task UpdatePayementStatus(UpdatePayamentResultVO vo)
        {
            try
            {
                await _repository.UpdateOrderPayamentStatus(vo.OrderID, vo.Status);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
