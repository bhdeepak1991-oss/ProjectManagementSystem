using PMS.Domains;
using PMS.Features.Dashboard.ViewModels;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

namespace PMS.CopyProcessor
{
    public class CopyBackGroundProcess : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        public CopyBackGroundProcess(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            string message = string.Empty;
            var factory = new ConnectionFactory()
            {
                HostName = "localhost",
            };

            var connection = await factory.CreateConnectionAsync(stoppingToken);

            var channel = await connection.CreateChannelAsync(cancellationToken: stoppingToken);

            await channel.QueueDeclareAsync(queue: "copy_queue1",
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: null,
                cancellationToken: stoppingToken);

            var consumer = new AsyncEventingBasicConsumer(channel);

            consumer.ReceivedAsync += async (sender, args) =>
            {
                var body = args.Body.ToArray();
                message = Encoding.UTF8.GetString(body);

                await channel.BasicAckAsync(args.DeliveryTag, false, stoppingToken);

                if (!string.IsNullOrEmpty(message))
                {
                    var model= JsonSerializer.Deserialize<CopyProjectModel>(message);

                    using var scope = _scopeFactory.CreateScope();

                    var _copyService = scope.ServiceProvider
                                           .GetRequiredService<ICopyService>();


                    await _copyService.CopyProjectService(model.ProjectId, model.ProjectName);
                }
            };

            await channel.BasicConsumeAsync(
                      queue: "copy_queue1",
                      autoAck: false,
                      consumer: consumer,
                      cancellationToken: stoppingToken);

            await Task.Delay(Timeout.Infinite, stoppingToken);

        }
    }
}
