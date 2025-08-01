using Microsoft.AspNetCore.Mvc;
using ShopApi.Dtos;
using ShopApi.Services.Interfaces;

[Route("api/[controller]")]
[ApiController]
public class ProductsController : ControllerBase
{
    private readonly IProductService _service;

    public ProductsController(IProductService service)
    {
        Console.WriteLine(service == null ? "ProductService is NULL" : "ProductService is OK");
        _service = service;
    }

    [HttpGet]
    public async Task<ActionResult<List<ProductResponseDto>>> GetAll([FromQuery] int page = 1, [FromQuery] int pageSize = 10, [FromQuery] int? categoryId = null)
    {
        var products = await _service.GetPagedAsync(page, pageSize, categoryId);
        return Ok(products);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ProductDetailDto>> GetById(int id)
    {
        var product = await _service.GetByIdAsync(id);
        if (product == null) return NotFound();
        return Ok(product);
    }
    [HttpPost]
    public async Task<ActionResult<ProductResponseDto>> Create([FromBody] ProductCreateDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var createdProduct = await _service.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = createdProduct.ProductId }, createdProduct);
    }


}
