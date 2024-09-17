using EshopApi.Domain.DTOs;

namespace EshopApi.Application.Interfaces
{
    public interface IProductService
    {
        Task<ICollection<ProductDTO>?> GetAllProductsAsync();
        Task<ICollection<ProductDTO>?> GetProductsAsync(string? searchString,
                                                        string? sortBy,
                                                        string? sortDirection,
                                                        long? minPrice,
                                                        long? maxPrice,
                                                        int? pageNumber,
                                                        int? pageSize);
        Task<ProductDTO?> GetProductByIdAsync(int id);
        Task<ProductDTO?> GetProductByNameAsync(string name);
        Task<ICollection<ProductDTO>?> GetProductByPageAsync(int pageNumber, int pageSize);
        Task<ProductDTO?> AddNewProductAsync(ProductNewDTO productNewDto);
        Task<ProductDTO?> UpdateProductAsync(ProductDTO productDto);
        Task<bool> DeleteProductAsync(int id);
    }
}
