using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace nettruyen.Model
{
    [Table("comic")]
    public class Comic
    {
        [Key]   
        public int Id { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
        public string Describe { get; set; }
        public string Author { get; set; }
        public int Status { get; set; }
        public DateTime Create_at { get; set; }
        public ICollection<ComicCategory> ComicCategories { get; set;}
        [NotMapped]
        public List<int> CategoryIds { get; set; }

    }
}
