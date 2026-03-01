namespace Bibliotekssystem2026.Models
{
    public class Member
    {
        public string MemberId { get; }
        public string Name { get; private set; }
        public string Email { get; private set; }
        public DateTime MemberSince { get; }
        private readonly List<Book> _borrowedBooks = new();

        public IReadOnlyCollection<Book> BorrowedBooks => _borrowedBooks.AsReadOnly();

        public Member(string memberId, string name, string email)
        {
            MemberId = memberId;
            Name = name;
            Email = email;
            MemberSince = DateTime.Now;
        }

        internal void AddLoan(Book book)
        {
            _borrowedBooks.Add(book);
        }

        internal void RemoveLoan(Book book)
        {
            _borrowedBooks.Remove(book);
        }

        public string GetInfo()
        {
            return $"{Name} ({Email}) - Member since {MemberSince:d}";
        }
    }
}
