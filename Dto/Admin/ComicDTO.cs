using nettruyen.Model;

namespace nettruyen.Dto.Admin
{
    public class ComicDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
        public string Describe { get; set; }
        public string Author { get; set; }
        public int Status { get; set; }
        public DateTime Create_at { get; set; }
        public List<CategoryDTO> Categories { get; set; }
    }
}
