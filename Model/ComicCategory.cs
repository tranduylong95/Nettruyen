using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace nettruyen.Model
{
    [Table("comic_category")]
    public class ComicCategory
    {
        [Key]
        public int idComic { get;set; }
        public Comic Comic { get; set; }
        [Key]
        public int idCategory { get; set; }
        public Category Category { get; set; }

    }
}
