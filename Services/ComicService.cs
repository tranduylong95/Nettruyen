using AutoMapper;
using Microsoft.EntityFrameworkCore;
using nettruyen.Data;
using nettruyen.Dto.Admin;
using nettruyen.Model;

namespace nettruyen.Services
{
    public class ComicService:IComicService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _env;
        public ComicService(AppDbContext context, IMapper mapper, IWebHostEnvironment env)
        {
            _context = context;
            _mapper = mapper;
            _env = env;
        }
        public async Task<PagedResult<ComicDTO>> GetAllComicsAsync(int pageNumber, int pageSize)
        {
            var totalRecords = await _context.Comic.CountAsync();
            var comic = await _context.Comic
              .Include(c => c.ComicCategories)
              .ThenInclude(cc => cc.Category)
              .Skip((pageNumber - 1) * pageSize)  // Bỏ qua các bản ghi của các trang trước đó
              .Take(pageSize)  // Lấy số bản ghi theo kích thước trang
              .ToListAsync();
            return new PagedResult<ComicDTO>(_mapper.Map<List<ComicDTO>>(comic), totalRecords, pageNumber, pageSize);

        }
        public async Task<ComicDTO?> FindComicById(int id)
        {
            var comic = await _context.Comic
                .Include(c => c.ComicCategories)
                .ThenInclude(cc => cc.Category)
                .FirstOrDefaultAsync(c => c.Id == id);
            if (comic == null) return null;
            return _mapper.Map<ComicDTO>(comic);
        }
        public async Task<ComicDTO> CreateAsync(CreateComicDTO dto)
        {
        
            var comic = _mapper.Map<Comic>(dto);
            comic.Image = await SaveImageOrDefaultAsync(dto.UploadImage);

           

            _context.Comic.Add(comic);
             await _context.SaveChangesAsync();
            comic.ComicCategories = dto.CategoryIds.Select(cid => new ComicCategory
            {
                idComic = comic.Id,
                idCategory = cid
            }).ToList();
            _context.ComicCategories.AddRange(comic.ComicCategories);
            await _context.SaveChangesAsync();
           
            comic = await _context.Comic
            .Include(c => c.ComicCategories)
           .ThenInclude(cc => cc.Category)
           .FirstOrDefaultAsync(c => c.Id == comic.Id);

            return _mapper.Map<ComicDTO>(comic);
        }
        public async Task<ComicDTO>UpdateAsync(UpdateComicDTO dto)
        {
            var comic = await _context.Comic
            .Include(c => c.ComicCategories)
            .FirstOrDefaultAsync(c => c.Id == dto.Id);

            _mapper.Map(dto, comic);

            // Lấy danh sách category hiện có
            var currentCategoryIds = comic.ComicCategories.Select(cc => cc.idCategory).ToList();

            // Xác định category cần thêm
            var categoriesToAdd = dto.CategoryIds.Except(currentCategoryIds).ToList();

            // Xác định category cần xóa
            var categoriesToRemove = comic.ComicCategories
                .Where(cc => !dto.CategoryIds.Contains(cc.idCategory))
                .ToList();

            // Xóa những cái không còn
            _context.ComicCategories.RemoveRange(categoriesToRemove);

            // Thêm những cái mới
            foreach (var categoryId in categoriesToAdd)
            {
                comic.ComicCategories.Add(new ComicCategory
                {
                    idComic = comic.Id,
                    idCategory = categoryId
                });
            }
            await _context.SaveChangesAsync();
            return _mapper.Map<ComicDTO>(comic);
        }
        private async Task<string> SaveImageOrDefaultAsync(IFormFile? image)
        {
            if (image == null || image.Length == 0)
                return "/images/no-image.png";

            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(image.FileName)}";
            var path = Path.Combine(_env.WebRootPath, "images", fileName);
            Directory.CreateDirectory(Path.GetDirectoryName(path)!);

            using (var stream = new FileStream(path, FileMode.Create))
            {
                await image.CopyToAsync(stream);
            }

            return $"/images/{fileName}";
        }
    }
}
