using System.ComponentModel.DataAnnotations;

namespace Backend.Models
{
    public class BookModel
    {
        [Key]
        public Guid BookId { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public string? Author { get; set; }
        public string? Series { get; set; }
        public List<string>? Genres { get; set; }
        public DateTime? Published {  get; set; }
    }
}
