using nettruyen.Dto.Admin;
using nettruyen.Model;

namespace nettruyen.Services
{
    public interface ICategoryService
    {
        Task<PagedResult<CategoryDTO>> GetAllCategoriesAsync(int pageNumber, int pageSize);
        Task<CategoryDTO?> FindCategoryById(int id);
        Task<CategoryDTO> CreateAsync(CategoryDTO dto);
        Task<CategoryDTO> UpdateAsync(CategoryDTO categoryDTO);
        Task<bool> DeleteAsync(int id);
    }
}
