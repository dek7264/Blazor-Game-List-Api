using AutoFixture.Xunit2;
using FakeItEasy;
using FluentAssertions;
using Game_List_Api.Controllers;
using Game_List_Api.Controllers.Sandbox.DbRequest;
using MediatR;
using Xunit;
using static Game_List_Api.Controllers.SandboxController;
using RabbitMqMessageRequest = Game_List_Api.Controllers.Sandbox.RabbitMqRequest.SendRabbitMqMessageRequest;

namespace Game_List_Api.Tests.Unit.Controllers.Sandbox
{
    public class SandboxControllerTests
    {
        public class TestFromDb
        {
            private TestLogger<SandboxController> Logger { get; }
            private IMediator Mediator { get; }
            private SandboxController Controller { get; }

            public TestFromDb()
            {
                Logger = A.Fake<TestLogger<SandboxController>>();
                Mediator = A.Fake<IMediator>();
                Controller = new SandboxController(Logger, Mediator);
            }

            [Fact]
            public async Task ShouldCallMediator()
            {
                await Controller.TestFromDb();

                A.CallTo(() => Mediator.Send(A<TestDbRequest>._, default))
                    .MustHaveHappenedOnceExactly();
            }
        }

        public class PostRabbitMqMessageTests
        {
            private TestLogger<SandboxController> Logger { get; }
            private IMediator Mediator { get; }
            private SandboxController Controller { get; }

            public PostRabbitMqMessageTests()
            {
                Logger = A.Fake<TestLogger<SandboxController>>();
                Mediator = A.Fake<IMediator>();
                Controller = new SandboxController(Logger, Mediator);
            }

            [Theory, AutoData]
            public async Task ShouldCallMediator(PublishInput input)
            {
                RabbitMqMessageRequest? actualNotification = null;
                A.CallTo(() => Mediator.Publish(A<RabbitMqMessageRequest>._, default))
                    .Invokes(call => actualNotification = call.GetArgument<RabbitMqMessageRequest>(0));

                await Controller.PostRabbitMqMessage(input);

                actualNotification.Should().BeEquivalentTo(new RabbitMqMessageRequest(input.TestString, input.TestInt));
            }
        }
    }
}