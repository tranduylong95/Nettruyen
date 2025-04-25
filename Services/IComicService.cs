using nettruyen.Dto.Admin;
using nettruyen.Model;

namespace nettruyen.Services
{
    public interface IComicService
    {
        Task<PagedResult<ComicDTO>> GetAllComicsAsync(int pageNumber, int pageSize);
        Task<ComicDTO?> FindComicById(int id);
        Task<ComicDTO> CreateAsync(CreateComicDTO dto);
    }
}
