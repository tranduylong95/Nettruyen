using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Localization;
using nettruyen.Data;
using nettruyen.Dto.Admin;

namespace nettruyen.Validators.Admin
{
    public class ComicDTOCreateValidator : AbstractValidator<CreateComicDTO>
    {
        private readonly AppDbContext _context;

        public ComicDTOCreateValidator(AppDbContext context)
        {
            _context = context;
            RuleFor(x => x.Name)
           .NotNull().WithMessage("Tên truyện là bắt buộc.")
           .NotEmpty().WithMessage("Tên truyện là bắt buộc.")
           .MaximumLength(100).WithMessage("Tên truyện không được quá 100 ký tự.")
           .MustAsync(BeUniqueName).WithMessage("Tên truyện đã tồn tại.");
            RuleFor(x => x.UploadImage)
            .Must(file => file == null || file.Length < 5 * 1024 * 1024).WithMessage("File phải nhỏ hơn 5MB.")
            .Must(file => file == null || file.ContentType.StartsWith("image/")).WithMessage("File phải là hình ảnh.");
            RuleFor(x => x.CategoryIds)
            .NotNull().WithMessage("Danh sách danh mục không được để trống.")
            .Must(x => x.Any()).WithMessage("Danh sách danh mục phải có ít nhất một mục.")
            .MustAsync(CheckAllCategoryExist).WithMessage("Có danh mục không hợp lệ.");
            RuleFor(x => x.Author)
            .NotEmpty().WithMessage("Tên tác giả là bắt buộc.")
            .MaximumLength(100).WithMessage("Tên danh mục không được quá 100 ký tự.");
            RuleFor(x => x.Describe)
            .NotEmpty().WithMessage("Nội dung là bắt buộc");
        }

        private async Task<bool> BeUniqueName(string name, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(name))
            {
                return true; // Nếu tên rỗng hoặc null, coi như hợp lệ
            }
            return !await _context.Categories
                .AnyAsync(c => c.Name.ToLower() == name.ToLower(), cancellationToken);
        }

        private async Task<bool> CheckAllCategoryExist(List<int> categoryIds, CancellationToken cancellationToken)
        {
            // Kiểm tra tất cả CategoryIds có tồn tại trong bảng Category
            return !categoryIds.Any(id => !_context.Categories.Any(c => c.Id == id));
        }
    }
}
