namespace RiskScreenAPI.Shared.Domain.Repositories;

public interface IUnitOfWork
{
    Task CompleteAsync();
}