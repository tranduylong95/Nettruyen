using System.ComponentModel.DataAnnotations;
using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using nettruyen.Dto.Admin;
using nettruyen.Services;

namespace nettruyen.Controllers.Admin
{
    [ApiController]
    [Route("api/admin/comic")]
    public class ComicController : ControllerBase
    {
        private readonly IComicService _service;
        private readonly IMapper _mapper;
        private readonly IValidator<CreateComicDTO> _validatorCreate;
        private readonly IValidator<UpdateComicDTO> _validatorUpdate;
        public ComicController(IComicService service, IMapper mapper, IValidator<CreateComicDTO> validatorCreate, IValidator<UpdateComicDTO> validatorUpdate)
        {
            _service = service;
            _mapper = mapper;
            _validatorCreate = validatorCreate;
            _validatorUpdate = validatorUpdate;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ComicDTO>>> GetAll(int pageNumber = 1)
        {
            int pageSize = 10;
            var result = await _service.GetAllComicsAsync(pageNumber, pageSize);
            if (!result.Items.Any())
            {
                return NotFound(new { message = $"Không tìm thấy dữ liệu ở trang {pageNumber}." });
            }
            return Ok(result);
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<ComicDTO?>> FindById(int id)
        {
            var category = await _service.FindComicById(id);
            if (category == null)
                return NotFound(new { message = $"Không tìm thấy comic với id = {id}" });
            return Ok(category);
        }
        [HttpPost("create")]
        public async Task<IActionResult> Create([FromForm] CreateComicDTO dto)
        {
            var validationResult = await _validatorCreate.ValidateAsync(dto);
            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors
            .GroupBy(e => e.PropertyName)
            .ToDictionary(
                g => g.Key,
                g => g.Select(e => e.ErrorMessage).ToArray()
            );

                return BadRequest(new { errors });
            }
            var result = await _service.CreateAsync(dto);
            return Ok(result);
        }
        [HttpPut("{id}")]

        public async Task<IActionResult> UpdateCategory(int id, [FromForm] UpdateComicDTO dto)
        {
            dto.Id = id;
            var validationResult = await _validatorUpdate.ValidateAsync(dto);
            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors
            .GroupBy(e => e.PropertyName)
            .ToDictionary(
                g => g.Key,
                g => g.Select(e => e.ErrorMessage).ToArray()
            );

                return BadRequest(new { errors });
            }
            var updated = await _service.UpdateAsync(dto);

            return Ok(new { message = "Cập nhật thành công" });
        }
    }
}
