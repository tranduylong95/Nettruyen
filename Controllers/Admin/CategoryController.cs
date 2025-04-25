using System.ComponentModel.DataAnnotations;
using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using nettruyen.Dto;
using nettruyen.Dto.Admin;
using nettruyen.Services;
namespace nettruyen.Controllers.Admin
{
    [ApiController]
    [Route("api/admin/category")]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _service;
        private readonly IMapper _mapper;
        private readonly IValidator<CategoryDTO> _validator;
        public CategoryController(ICategoryService service, IMapper mapper, IValidator<CategoryDTO> validator)
        {
            _service = service;
            _mapper = mapper;
            _validator = validator;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CategoryDTO>>> GetAll(int pageNumber=1)
        {
            int pageSize = 10;
            var result = await _service.GetAllCategoriesAsync(pageNumber, pageSize);
            if (!result.Items.Any())
            {
                return NotFound(new { message = $"Không tìm thấy dữ liệu ở trang {pageNumber}." });
            }
            return Ok(result);
        }
        [HttpGet("{id}")]

        public async Task<ActionResult<CategoryDTO?>> FindById(int id)

        {
            var category = await _service.FindCategoryById(id);
            if(category==null)
                return NotFound(new { message = $"Không tìm thấy category với id = {id}" });
            return Ok(category);
        }
        [HttpPost]
        public async Task<ActionResult<CategoryDTO>> CreateCategory([FromBody] CategoryDTO dto)
        {
            var validationResult = await _validator.ValidateAsync(dto);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }
            var createdCategory = await _service.CreateAsync(dto);
            var createdCategoryDTO = _mapper.Map<CategoryDTO>(createdCategory);
            return Ok(new
            {
                message = "Tạo danh mục thành công!",
                data = createdCategoryDTO
            });
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCategory(int id, [FromBody] CategoryDTO dto)
        {
            if (id != dto.Id)
            {
                return BadRequest("ID trong URL và DTO không khớp.");
            }

            // Validate dữ liệu
            var validationResult = await _validator.ValidateAsync(dto);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors); // Trả về lỗi nếu không hợp lệ
            }

            // Cập nhật Category
            var updated = await _service.UpdateAsync(dto);
          
            return Ok(new { message = "Cập nhật thành công" });
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            var isDeleted = await _service.DeleteAsync(id);

            if (!isDeleted)
                return NotFound(new { message = $"Không tìm thấy category với id = {id}" });

            return Ok(new { message = "Xóa category thành công!" });
        }
    }
}
