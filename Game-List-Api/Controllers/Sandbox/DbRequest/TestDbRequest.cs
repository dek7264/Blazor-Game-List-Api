using Game_List_Api.Infrastructure;
using Game_List_Api.Infrastructure.DatabaseQueries;
using MediatR;

namespace Game_List_Api.Controllers.Sandbox.DbRequest
{
    public record TestDbRequest() : IRequest<IReadOnlyList<int>>;

    public class TestDbRequestHandler : IRequestHandler<TestDbRequest, IReadOnlyList<int>>
    {
        private IHerokuPostgresDatabaseContext DatabaseContext { get; }

        public TestDbRequestHandler(IHerokuPostgresDatabaseContext databaseContext)
        {
            DatabaseContext = databaseContext;
        }

        public Task<IReadOnlyList<int>> Handle(TestDbRequest request, CancellationToken cancellationToken)
        {
            return DatabaseContext.ExecuteQuery(new GetTestInfoDatabaseQuery());
        }
    }
}
