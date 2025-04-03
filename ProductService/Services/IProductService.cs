﻿using ProductService.Dto;
using ProductService.Models;

namespace ProductService.Services
{
    public interface IProductService
    {
        Task<Guid> CreateOrUpdateProduct(ProductCreateRequestDTO dto);
        Task<bool> DeleteProductAsync(int sellerId, Guid productId);
        Task<List<Product>> GetProducts(int sellerId);
    }
}
