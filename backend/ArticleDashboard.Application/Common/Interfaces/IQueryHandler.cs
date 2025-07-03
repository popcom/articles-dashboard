namespace ArticleDashboard.Application.Common.Interfaces;

public interface IQueryHandler<TQuery, TResult>
{
    Task<TResult> Handle(TQuery query);
}

public interface IQueryHandler<TResult> // for parameterless queries
{
    Task<TResult> Handle();
}