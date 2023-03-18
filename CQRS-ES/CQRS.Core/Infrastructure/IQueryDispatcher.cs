using CQRS.Core.Queries;

namespace CQRS.Core.Infrastructure;

public interface IQueryDispatcher<TEntity>
{
    /*
     The IQueryDispatcher is the mediator that manages the distribution of queries to the relevant query handler methods.
     */

    void RegisterHadler<TQuery>(Func<TQuery, Task<List<TEntity>>> handler) where TQuery : BaseQuery;

    Task<List<TEntity>> SendAsync(BaseQuery query);
}
