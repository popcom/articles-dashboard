namespace ArticleDashboard.Application.Common.Interfaces;

public interface ICommandHandler<TCommand, TResult>
{
    Task<TResult> Handle(TCommand command);
}

public interface ICommandHandler<TCommand> // for void-returning handlers
{
    Task Handle(TCommand command);
}