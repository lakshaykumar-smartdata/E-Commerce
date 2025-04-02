using ProductService.Dto;

namespace ProductService.Services
{
    public interface IProductService
    {
        Task<Guid> CreateOrUpdateProduct(ProductCreateRequestDTO dto);
    }
}
