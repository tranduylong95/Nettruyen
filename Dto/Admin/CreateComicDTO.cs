using Microsoft.AspNetCore.Http;
namespace nettruyen.Dto.Admin
{
    public class CreateComicDTO
    {
        public string Name { get; set; }
        public string Describe { get; set; }
        public string Author { get; set; }
        public int Status { get; set; }
        public List<int> CategoryIds { get; set; }
        public IFormFile? UploadImage { get; set; }
    }
}
