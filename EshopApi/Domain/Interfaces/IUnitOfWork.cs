namespace EshopApi.Domain.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IUserRepository UserRepository { get; }
        IProductRepository ProductRepository { get; }
        ICartItemRepository CartItemRepository { get; }

        // int SaveChanges();
        // Task<int> SaveChangesAsync(CancellationToken token);

        void BeginTransaction();
        Task BeginTransactionAsync(CancellationToken token);
        void CommitTransaction();
        Task CommitTransactionAsync(CancellationToken token);
        void RollbackTransaction();
        Task RollbackTransactionAsync(CancellationToken token);

        Task ExecuteTransactionAsync(Action action, CancellationToken token);
        Task ExecuteTransactionAsync(Func<Task> action, CancellationToken token);
    }
}
