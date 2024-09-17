using Microsoft.EntityFrameworkCore;
using EshopApi.Domain.Entities;
using EshopApi.Domain.DTOs;
using EshopApi.Domain.Interfaces;
using EshopApi.Application.Interfaces;

namespace EshopApi.Application.Services
{
    public class ProductService : IProductService
    {
        private readonly IUnitOfWork _unitOfWork;
        private ProductDTO ToProductDTO(Product product)
        {
            return new ProductDTO
            {
                Id = product.Id,
                Name = product.Name,
                Price = product.Price,
                Description = product.Description
            };
        }

        public ProductService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ICollection<ProductDTO>?> GetAllProductsAsync()
        {
            var products = await _unitOfWork.ProductRepository.GetAllAsync();
            return products?.Select(ToProductDTO).ToList();
        }

        public async Task<ICollection<ProductDTO>?> GetProductsAsync(string? searchString,
                                                                     string? sortBy,
                                                                     string? sortDirection,
                                                                     long? minPrice,
                                                                     long? maxPrice,
                                                                     int? pageNumber,
                                                                     int? pageSize)
        {
            var query = _unitOfWork.ProductRepository.AsQueryable();
            if (!string.IsNullOrEmpty(searchString))
            {
                query = query.Where(p => p.Name.ToLower().Contains(searchString.ToLower()));
            }
            if (!string.IsNullOrEmpty(sortBy))
            {
                if (string.IsNullOrEmpty(sortDirection))
                {
                    sortDirection = "asc";
                }
                switch (sortBy.ToLower())
                {
                    case "id":
                        query = (sortDirection == "asc") ? query.OrderBy(p => p.Id) : query.OrderByDescending(p => p.Id);
                        break;
                    case "name":
                        query = (sortDirection == "asc") ? query.OrderBy(p => p.Name) : query.OrderByDescending(p => p.Name);
                        break;
                    case "price":
                        query = (sortDirection == "asc") ? query.OrderBy(p => p.Price) : query.OrderByDescending(p => p.Price);
                        break;
                    default:
                        break;
                }
            }
            if (minPrice.HasValue && minPrice > 0)
            {
                query = query.Where(p => p.Price >= minPrice.Value);
            }
            if (maxPrice.HasValue && maxPrice > 0)
            {
                query = query.Where(p => p.Price <= maxPrice.Value);
            }
            if (pageNumber.HasValue && pageNumber > 0 && pageSize.HasValue && pageSize > 0)
            {
                query = query.Skip((pageNumber.Value - 1) * pageSize.Value).Take(pageSize.Value);
            }
            var products = await query.ToListAsync();
            return products.Select(ToProductDTO).ToList();
        }

        public async Task<ProductDTO?> GetProductByIdAsync(int id)
        {
            var product = await _unitOfWork.ProductRepository.GetByIdAsync(id);
            return (product != null) ? ToProductDTO(product) : null;
        }

        public async Task<ProductDTO?> GetProductByNameAsync(string name)
        {
            var product = await _unitOfWork.ProductRepository.GetByNameAsync(name);
            return (product != null) ? ToProductDTO(product) : null;
        }

        public async Task<ICollection<ProductDTO>?> GetProductByPageAsync(int pageNumber, int pageSize)
        {
            var products = await _unitOfWork.ProductRepository.AsQueryable()
                                                              .OrderBy(p => p.Id)
                                                              .Skip((pageNumber - 1) * pageSize)
                                                              .Take(pageSize)
                                                              .ToListAsync();
            return products?.Select(ToProductDTO).ToList();
        }

        public async Task<ProductDTO?> AddNewProductAsync(ProductNewDTO productNewDto)
        {
            var addedProduct = new Product
            {
                Name = productNewDto.Name,
                Price = productNewDto.Price,
                Description = productNewDto.Description
            };
            try
            {
                using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(10));
                await _unitOfWork.ExecuteTransactionAsync(async () =>
                {
                    addedProduct = await _unitOfWork.ProductRepository.AddAsync(addedProduct);
                }, cts.Token);
                return ToProductDTO(addedProduct);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Transaction failed: {ex.Message}");
                return null;
            }
        }

        public async Task<ProductDTO?> UpdateProductAsync(ProductDTO productDto)
        {
            var updatedProduct = await _unitOfWork.ProductRepository.GetByIdAsync(productDto.Id);
            if (updatedProduct == null)
            {
                return null;
            }
            updatedProduct.Name = productDto.Name;
            updatedProduct.Price = productDto.Price;
            updatedProduct.Description = productDto.Description;
            try
            {
                using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(10));
                await _unitOfWork.ExecuteTransactionAsync(async () =>
                {
                    updatedProduct = await _unitOfWork.ProductRepository.UpdateAsync(updatedProduct);
                }, cts.Token);
                return ToProductDTO(updatedProduct);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Transaction failed: {ex.Message}");
                return null;
            }
        }

        public async Task<bool> DeleteProductAsync(int id)
        {
            try
            {
                using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(10));
                await _unitOfWork.ExecuteTransactionAsync(async () =>
                {
                    await _unitOfWork.ProductRepository.DeleteAsync(id);
                }, cts.Token);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Transaction failed: {ex.Message}");
                return false;
            }
        }
    }
}
