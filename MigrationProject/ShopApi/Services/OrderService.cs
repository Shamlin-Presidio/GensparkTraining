using AutoMapper;
using ShopApi.Dtos;
using ShopApi.Interfaces;
using ShopApi.Models;
using ShopApi.Services.Interfaces;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;

public class OrderService : IOrderService
{
    private readonly IOrderRepository _orderRepo;
    private readonly IMapper _mapper;

    public OrderService(IOrderRepository orderRepo, IMapper mapper)
    {
        _orderRepo = orderRepo;
        _mapper = mapper;
    }

    public async Task<IEnumerable<OrderResponseDto>> GetPagedOrdersAsync(int page, int pageSize)
    {
        var orders = await _orderRepo.GetPagedAsync(page, pageSize);
        return orders.Select(o => _mapper.Map<OrderResponseDto>(o));
    }

    public async Task<OrderResponseDto?> GetByIdAsync(int id)
    {
        var order = await _orderRepo.GetByIdAsync(id);
        return order == null ? null : _mapper.Map<OrderResponseDto>(order);
    }

    public async Task<OrderResponseDto> CreateAsync(OrderCreateDto dto)
    {
        var order = _mapper.Map<Order>(dto);
        order.OrderDate = DateTime.UtcNow;

        await _orderRepo.AddAsync(order);
        return _mapper.Map<OrderResponseDto>(order);
    }

    public async Task<bool> UpdateAsync(int id, OrderUpdateDto dto)
    {
        var existing = await _orderRepo.GetByIdAsync(id);
        if (existing == null) return false;

        existing.Status = dto.Status;

        await _orderRepo.UpdateAsync(existing);
        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var order = await _orderRepo.GetByIdAsync(id);
        if (order == null) return false;

        await _orderRepo.DeleteAsync(order.OrderId);
        return true;
    }

    // public async Task<byte[]> ExportToPdfAsync()
    // {
    //     var orders = await _orderRepo.GetAllAsync();

    //     using var ms = new MemoryStream();
    //     Document doc = new Document();
    //     PdfWriter.GetInstance(doc, ms);
    //     doc.Open();

    //     doc.Add(new Paragraph("Order Listing"));
    //     doc.Add(new Paragraph($"Exported on: {DateTime.UtcNow}\n\n"));

    //     PdfPTable table = new PdfPTable(4);
    //     table.AddCell("OrderId");
    //     table.AddCell("OrderDate");
    //     table.AddCell("Status");
    //     table.AddCell("UserId");

    //     foreach (var order in orders)
    //     {
    //         table.AddCell(order.OrderId.ToString());
    //         table.AddCell(order.OrderDate.ToString("yyyy-MM-dd"));
    //         table.AddCell(order.Status ?? "-");
    //         table.AddCell(order.UserId.ToString());
    //     }

    //     doc.Add(table);
    //     doc.Close();

    //     return ms.ToArray();
    // }
    public async Task<byte[]> ExportToPdfAsync()
{
    var orders = await _orderRepo.GetAllAsync();

    using var ms = new MemoryStream();
    var doc = new Document(PageSize.A4);
    PdfWriter.GetInstance(doc, ms);

    doc.Open();
    doc.Add(new Paragraph("Order Listing"));
    doc.Add(new Paragraph($"Exported on: {DateTime.UtcNow}\n\n"));

    PdfPTable table = new PdfPTable(4); // 4 columns

    table.AddCell("OrderId");
    table.AddCell("OrderDate");
    table.AddCell("Status");
    table.AddCell("UserId");

    foreach (var order in orders)
    {
        table.AddCell(order.OrderId.ToString());
        table.AddCell(order.OrderDate.ToShortDateString());
        table.AddCell(order.Status ?? "-");
        table.AddCell(order.UserId.ToString());
    }

    doc.Add(table);
    doc.Close();

    return ms.ToArray();
}

}