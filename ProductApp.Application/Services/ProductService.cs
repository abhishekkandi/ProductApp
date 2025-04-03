using ProductApp.Application.DTOS;
using ProductApp.Application.Interfaces;
using ProductApp.Infrastructure.Data.Configurations;
using ProductApp.Infrastructure.Data;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;

namespace ProductApp.Application.Services
{
    public class ProductService : IProductService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ProductService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ProductDto> GetProductByIdAsync(int id)
        {
            var product = await _unitOfWork.Products.GetByIdAsync(id);
            return _mapper.Map<ProductDto>(product);
        }

        public async Task<ProductDto> CreateProductAsync(ProductDto productDto)
        {
            var product = _mapper.Map<Product>(productDto);
            await _unitOfWork.Products.AddAsync(product);
            await _unitOfWork.CompleteAsync();
            return _mapper.Map<ProductDto>(product);
        }

        public async Task<ProductDto> UpdateProductAsync(int id, ProductDto productDto)
        {
            var existingProduct = await _unitOfWork.Products.GetByIdAsync(id);
            if (existingProduct != null)
            {
                _mapper.Map(productDto, existingProduct);
                await _unitOfWork.Products.UpdateAsync(existingProduct);
                await _unitOfWork.CompleteAsync();
                return _mapper.Map<ProductDto>(existingProduct);
            }
            return null;
        }

        public async Task<bool> DeleteProductAsync(int id)
        {
            var product = await _unitOfWork.Products.GetByIdAsync(id);
            if (product != null)
            {
                await _unitOfWork.Products.DeleteAsync(product);
                int result = await _unitOfWork.CompleteAsync();
                return result > 0;
            }
            return false;
        }
    }
}


