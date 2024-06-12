using Questao5.Core.Data;

public class MockUnitOfWork : IUnitOfWork
{
    public Task<bool> Commit()
    {
        return Task.FromResult(true);
    }
}