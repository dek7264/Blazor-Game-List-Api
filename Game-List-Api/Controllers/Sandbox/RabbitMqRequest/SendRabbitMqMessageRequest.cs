using GameListApiWorker.Contracts;
using MassTransit;
using MediatR;

namespace Game_List_Api.Controllers.Sandbox.RabbitMqRequest
{
    public record SendRabbitMqMessageRequest(string TestString, int TestInt) : INotification;

    public class SendRabbitMqMessageRequestHandler : INotificationHandler<SendRabbitMqMessageRequest>
    {
        private ISendEndpointProvider SendEndpointProvider { get; }
        private IConfiguration Configuration { get; }

        public SendRabbitMqMessageRequestHandler(ISendEndpointProvider sendEndpointProvider, IConfiguration configuration)
        {
            SendEndpointProvider = sendEndpointProvider;
            Configuration = configuration;
        }

        public async Task Handle(SendRabbitMqMessageRequest notification, CancellationToken cancellationToken)
        {
            var endpoint = await SendEndpointProvider.GetSendEndpoint(new Uri($"queue:{Configuration["RabbitMqConfiguration:QueueName"]}"));
            await endpoint.Send(new RabbitMqMessage(notification.TestString, notification.TestInt));
        }
    }
}
