using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ShopApi.Data;
using ShopApi.Dtos;
using ShopApi.Models;
using ShopApi.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OfficeOpenXml;

namespace ShopApi.Services
{
    public class NewsService : INewsService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public NewsService(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<IEnumerable<NewsDto>> GetPagedNewsAsync(int page, int pageSize)
        {
            var news = await _context.News
                .OrderByDescending(n => n.NewsId)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return _mapper.Map<IEnumerable<NewsDto>>(news);
        }

        public async Task<IEnumerable<NewsResponseDto>> GetAllNewsAsync()
        {
            var newsList = await _context.News
                .Include(n => n.User)
                .OrderByDescending(n => n.NewsId)
                .ToListAsync();

            return _mapper.Map<IEnumerable<NewsResponseDto>>(newsList);
        }

        public async Task<NewsResponseDto> GetNewsByIdAsync(int id)
        {
            var news = await _context.News
                .Include(n => n.User)
                .FirstOrDefaultAsync(n => n.NewsId == id);

            if (news == null)
                throw new KeyNotFoundException("News not found");

            return _mapper.Map<NewsResponseDto>(news);
        }

        public async Task<NewsResponseDto> CreateNewsAsync(NewsCreateDto dto)
        {
            var news = _mapper.Map<News>(dto);
            news.CreatedDate = DateTime.UtcNow;

            _context.News.Add(news);
            await _context.SaveChangesAsync();

            return _mapper.Map<NewsResponseDto>(news);
        }

        public async Task<NewsResponseDto> UpdateNewsAsync(int id, NewsUpdateDto dto)
        {
            var news = await _context.News.FindAsync(id);
            if (news == null)
                throw new KeyNotFoundException("News not found");

            _mapper.Map(dto, news);
            await _context.SaveChangesAsync();

            return _mapper.Map<NewsResponseDto>(news);
        }

        public async Task<bool> DeleteNewsAsync(int id)
        {
            var news = await _context.News.FindAsync(id);
            if (news == null)
                return false;

            _context.News.Remove(news);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<byte[]> ExportToCsvAsync()
        {
            var newsList = await _context.News.OrderBy(x => x.NewsId).ToListAsync();
            var sb = new StringBuilder();

            sb.AppendLine("NewsId,Title,ShortDescription,CreatedDate,Status");

            foreach (var news in newsList)
            {
                sb.AppendLine($"{news.NewsId},\"{Escape(news.Title)}\",\"{Escape(news.ShortDescription)}\",{news.CreatedDate},{news.Status}");
            }

            return Encoding.UTF8.GetBytes(sb.ToString());
        }

        private string Escape(string value) =>
            string.IsNullOrEmpty(value) ? "" : value.Replace("\"", "\"\"");

        public async Task<byte[]> ExportToExcelAsync()
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("News");

                var newsList = await _context.News.OrderBy(x => x.NewsId).ToListAsync();

                worksheet.Cells[1, 1].Value = "NewsId";
                worksheet.Cells[1, 2].Value = "Title";
                worksheet.Cells[1, 3].Value = "ShortDescription";
                worksheet.Cells[1, 4].Value = "CreatedDate";
                worksheet.Cells[1, 5].Value = "Status";

                for (int i = 0; i < newsList.Count; i++)
                {
                    var n = newsList[i];
                    worksheet.Cells[i + 2, 1].Value = n.NewsId;
                    worksheet.Cells[i + 2, 2].Value = n.Title;
                    worksheet.Cells[i + 2, 3].Value = n.ShortDescription;
                    worksheet.Cells[i + 2, 4].Value = n.CreatedDate.ToString("yyyy-MM-dd");
                    worksheet.Cells[i + 2, 5].Value = n.Status;
                }

                return package.GetAsByteArray();
            }
        }
    }
}
