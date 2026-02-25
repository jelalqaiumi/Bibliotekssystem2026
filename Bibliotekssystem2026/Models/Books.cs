namespace Bibliotekssystem2026.Models
{
    public class Books : ISearchable
    {
        public string ISBN { get; }
        public string Title { get; private set; }
        public string Author { get; private set; }
        public int PublishedYear { get; private set; }
        public bool IsAvailable { get; internal set; }

        public Books(string isbn, string title, string author, int publishedYear)
        {
            if (string.IsNullOrWhiteSpace(isbn))
                throw new ArgumentException("ISBN cannot be empty");

            ISBN = isbn;
            Title = title;
            Author = author;
            PublishedYear = publishedYear;
            IsAvailable = true;
        }

        public string GetInfo()
        {
            return $"{Title} by {Author} ({PublishedYear}) - ISBN: {ISBN}";
        }

        public bool Matches(string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
                return false;

            searchTerm = searchTerm.ToLower();

            return Title.ToLower().Contains(searchTerm)
                || Author.ToLower().Contains(searchTerm)
                || ISBN.ToLower().Contains(searchTerm);
        }
    }
}
