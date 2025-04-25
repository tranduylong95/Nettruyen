using System.ComponentModel.DataAnnotations;
namespace nettruyen.Dto.Admin
{
    public class CategoryDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
    
        public string Note { get; set; }
        public int Status { get; set; }
    }
}
