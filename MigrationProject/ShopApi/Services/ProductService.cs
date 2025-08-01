using AutoMapper;
using ShopApi.Dtos;
using ShopApi.Interfaces;
using ShopApi.Models;
using ShopApi.Services.Interfaces;

public class ProductService : IProductService
{
    private readonly IProductRepository _repo;
    private readonly IMapper _mapper;

    public ProductService(IProductRepository repo, IMapper mapper)
    {
        _repo = repo;
        _mapper = mapper;
    }

    public async Task<List<ProductResponseDto>> GetPagedAsync(int page, int pageSize, int? categoryId)
    {
        var products = await _repo.GetPagedAsync(page, pageSize, categoryId);
        return _mapper.Map<List<ProductResponseDto>>(products);
    }

    public async Task<ProductDetailDto?> GetByIdAsync(int id)
    {
        var product = await _repo.GetByIdAsync(id);
        return product is null ? null : _mapper.Map<ProductDetailDto>(product);
    }
    // public async Task<ProductResponseDto> CreateAsync(ProductCreateDto dto)
    // {
    //     var product = _mapper.Map<Product>(dto);
    //     await _repo.AddAsync(product);

    //     var savedProduct = await _repo.GetByIdAsync(product.ProductId);
    //     return _mapper.Map<ProductResponseDto>(savedProduct);
    // }
    public async Task<ProductResponseDto> CreateAsync(ProductCreateDto dto)
    {
        Console.WriteLine("Mapping ProductCreateDto to Product...");
        var product = _mapper.Map<Product>(dto);

        if (product == null)
            throw new Exception("Product mapping returned null!");

        Console.WriteLine("Adding product to repository...");
        await _repo.AddAsync(product);

        Console.WriteLine("Mapping Product to ProductResponseDto...");
        var response = _mapper.Map<ProductResponseDto>(product);
        return response;
    }


}
