namespace LibraryManagementSystem.Models
{
    public class Book
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;      // Добавлено = string.Empty
        public int AuthorId { get; set; }
        public int GenreId { get; set; }
        public int PublishYear { get; set; }
        public string ISBN { get; set; } = string.Empty;       // Добавлено = string.Empty
        public int QuantityInStock { get; set; }
        
        // Навигационные свойства
        public Author? Author { get; set; }    // ? означает, что может быть null
        public Genre? Genre { get; set; }      // ? означает, что может быть null
    }
}