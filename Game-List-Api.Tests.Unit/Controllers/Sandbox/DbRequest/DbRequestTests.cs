using AutoFixture.Xunit2;
using FakeItEasy;
using Game_List_Api.Controllers.Sandbox.DbRequest;
using Game_List_Api.Infrastructure;
using Game_List_Api.Infrastructure.DatabaseQueries;
using Xunit;

namespace Game_List_Api.Tests.Unit.Controllers.Sandbox.DbRequest
{
    public class DbRequestTests
    {
        private IHerokuPostgresDatabaseContext DatabaseContext { get; }
        private TestDbRequestHandler Handler { get; }

        public DbRequestTests()
        {
            DatabaseContext = A.Fake<IHerokuPostgresDatabaseContext>();
            Handler = new TestDbRequestHandler(DatabaseContext);
        }

        [Theory, AutoData]
        public async Task ShouldCallDatabaseContext(TestDbRequest request)
        {
            await Handler.Handle(request, default);

            A.CallTo(() => DatabaseContext.ExecuteQuery(A<GetTestInfoDatabaseQuery>._, default))
                .MustHaveHappenedOnceExactly();
        }
    }
}
