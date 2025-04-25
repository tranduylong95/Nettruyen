using AutoMapper;
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
        public ComicController(IComicService service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
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
            var result = await _service.CreateAsync(dto);
            return Ok(result);
        }
    }
}
