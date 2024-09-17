using EshopApi.Domain.Entities;

namespace EshopApi.Domain.Interfaces
{
    public interface IProductRepository
    {
        IQueryable<Product> AsQueryable();
        Task<ICollection<Product>?> GetAllAsync();
        Task<Product?> GetByIdAsync(int id);
        Task<Product?> GetByNameAsync(string name);
        Task<Product> AddAsync(Product product);
        Task<Product?> UpdateAsync(Product product);
        Task<bool> DeleteAsync(int id);
    }
}
