using AutoFixture.Xunit2;
using FakeItEasy;
using FluentAssertions;
using Game_List_Api.Controllers.Sandbox.RabbitMqRequest;
using GameListApiWorker.Contracts;
using MassTransit;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Game_List_Api.Tests.Unit.Controllers.Sandbox.SendRabbitMqMessage
{
    public class SendRabbitMqMessageRequestTests
    {
        private const string QueueName = "queue-name";
        private ISendEndpointProvider Provider { get; }
        private ISendEndpoint Endpoint { get; }
        private SendRabbitMqMessageRequestHandler Handler { get; }

        public SendRabbitMqMessageRequestTests()
        {
            Provider = A.Fake<ISendEndpointProvider>();
            var configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string>
                {
                    { "RabbitMqConfiguration:QueueName", QueueName }
                })
                .Build();
            Endpoint = A.Fake<ISendEndpoint>();
            A.CallTo(() => Provider.GetSendEndpoint(AMatch.Of(new Uri($"queue:{QueueName}"))))
                .Returns(Endpoint);
            Handler = new SendRabbitMqMessageRequestHandler(Provider, configuration);
        }

        [Theory, AutoData]
        public async Task ShouldSendMessage(SendRabbitMqMessageRequest request)
        {
            RabbitMqMessage? actualMessage = null;
            A.CallTo(() => Endpoint.Send(A<RabbitMqMessage>._, default))
                .Invokes(call => actualMessage = call.GetArgument<RabbitMqMessage>(0));

            await Handler.Handle(request, default);

            actualMessage.Should().BeEquivalentTo(new RabbitMqMessage(request.TestString, request.TestInt));
        }
    }
}
