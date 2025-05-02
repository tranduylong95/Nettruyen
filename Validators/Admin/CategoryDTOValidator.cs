using FluentValidation;
using nettruyen.Dto.Admin;
using nettruyen.Data;
using Microsoft.EntityFrameworkCore;
using System.Xml.Linq;
using System.Threading;

namespace nettruyen.Validators.Admin
{
    public class CategoryDTOValidator : AbstractValidator<CategoryDTO>
    {
        private readonly AppDbContext _context ;

        // Constructor nhận vào DbContext để kiểm tra CSDL
        public CategoryDTOValidator(AppDbContext context)
        {
            _context = context;

            RuleFor(x => x.Name)
             .NotNull().WithMessage("Tên danh mục là bắt buộc.")
             .NotEmpty().WithMessage("Tên danh mục là bắt buộc.")
             .MaximumLength(100).WithMessage("Tên danh mục không được quá 100 ký tự.")
             .MustAsync(BeUniqueName).WithMessage("Tên danh mục đã tồn tại.");
        }

       

        private async Task<bool> BeUniqueName(CategoryDTO dto, string name, CancellationToken cancellationToken)
        {
            return !await _context.Categories
            .AnyAsync(c =>
                c.Name.ToLower() == name.ToLower() &&
                c.Id != dto.Id, // bỏ qua chính bản ghi đang cập nhật
                cancellationToken);
        }
    }
}
