namespace Questao5.Core.Data
{
    public interface IRepository : IDisposable
    {
        IUnitOfWork UnitOfWork { get;  }
    }
}
