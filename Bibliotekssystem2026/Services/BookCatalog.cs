using Bibliotekssystem2026.Models;

namespace Bibliotekssystem2026.Services
{
    public class BookCatalog
    {
        private readonly List<Book> _books = new();

        public void AddBook(Book book) => _books.Add(book);

        public IEnumerable<Book> Search(string term) =>
            _books.Where(b => b.Matches(term));

        public IEnumerable<Book> SortByTitle() =>
            _books.OrderBy(b => b.Title);

        public IEnumerable<Book> SortByYear() =>
            _books.OrderBy(b => b.PublishedYear);

        public int TotalBooks() => _books.Count;

        public int BorrowedBooksCount() =>
            _books.Count(b => !b.IsAvailable);
    }
}
