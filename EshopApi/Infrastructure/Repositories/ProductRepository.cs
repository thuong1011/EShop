using Microsoft.EntityFrameworkCore;
using EshopApi.Domain.Entities;
using EshopApi.Domain.Interfaces;
using EshopApi.Infrastructure.Data;

namespace EshopApi.Infrastructure.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly EshopDbContext _context;
        public ProductRepository(EshopDbContext context)
        {
            _context = context;
        }

        public IQueryable<Product> AsQueryable()
        {
            return _context.Products.AsQueryable();
        }

        public async Task<ICollection<Product>?> GetAllAsync()
        {
            return await _context.Products.ToListAsync();
        }

        public async Task<Product?> GetByIdAsync(int id)
        {
            return await _context.Products.FindAsync(id);
        }

        public async Task<Product?> GetByNameAsync(string name)
        {
            return await _context.Products.FirstOrDefaultAsync(p => p.Name == name);
        }

        public async Task<Product> AddAsync(Product product)
        {
            await _context.Products.AddAsync(product);
            return product;
        }

        public async Task<Product?> UpdateAsync(Product product)
        {
            var updatedProduct = await _context.Products.FindAsync(product.Id);
            if (updatedProduct == null)
            {
                return null;
            }
            updatedProduct.Name = product.Name;
            updatedProduct.Price = product.Price;
            updatedProduct.Description = product.Description;
            return updatedProduct;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var removedProduct = await _context.Products.FindAsync(id);
            if (removedProduct == null)
            {
                return false;
            }
            _context.Products.Remove(removedProduct);
            return true;
        }
    }
}
