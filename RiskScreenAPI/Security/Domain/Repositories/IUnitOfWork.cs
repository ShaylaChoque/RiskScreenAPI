namespace RiskScreenAPI.Security.Domain.Repositories;

public interface IUnitOfWork
{
    Task CompleteAsync();
}