using AutoMapper;
using Microsoft.EntityFrameworkCore;
using nettruyen.Data;
using nettruyen.Dto.Admin;
using nettruyen.Model;
namespace nettruyen.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        public CategoryService(AppDbContext context, IMapper mapper) {

            _context = context;
            _mapper = mapper;
        }
        public async Task<PagedResult<CategoryDTO>> GetAllCategoriesAsync(int pageNumber, int pageSize)
        {

            var totalRecords = await _context.Categories.CountAsync();
            var category = await _context.Categories
              .Skip((pageNumber - 1) * pageSize)  // Bỏ qua các bản ghi của các trang trước đó
              .Take(pageSize)  // Lấy số bản ghi theo kích thước trang
              .ToListAsync();
            return new PagedResult<CategoryDTO>(_mapper.Map<List<CategoryDTO>>(category), totalRecords, pageNumber, pageSize);
        }
        public async Task<CategoryDTO?> FindCategoryById(int id)
        {
            var category= await _context.Categories.FindAsync(id);

            if (category == null) return null;

            return _mapper.Map<CategoryDTO>(category);
        }
        public async Task<CategoryDTO> CreateAsync(CategoryDTO dto)
        {
            var entity = _mapper.Map<Category>(dto);
            _context.Categories.Add(entity);
            await _context.SaveChangesAsync();
            return _mapper.Map<CategoryDTO>(entity);
        }
        public async Task<CategoryDTO> UpdateAsync(CategoryDTO dto)
        {
            var category = await _context.Categories.FindAsync(dto.Id);
           
            _mapper.Map(dto, category);

            await _context.SaveChangesAsync();

            return _mapper.Map<CategoryDTO>(category);
        }
        public async Task<bool> DeleteAsync(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category == null)
                return false;

            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}
