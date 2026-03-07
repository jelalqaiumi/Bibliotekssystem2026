namespace Bibliotekssystem2026.Models
{
    public class Member
    {
        public const int MaxActiveLoans = 5;
        public string MemberId { get; }
        public string Name { get; private set; }
        public string Email { get; private set; }
        public DateTime MemberSince { get; }
        private readonly List<Book> _borrowedBooks = new();
        private readonly List<Loan> _loans = new();

        public IReadOnlyCollection<Book> BorrowedBooks => _borrowedBooks.AsReadOnly();
        public IReadOnlyCollection<Loan> Loans => _loans.AsReadOnly();

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

        internal void AddLoanRecord(Loan loan)
        {
            _loans.Add(loan);
        }

        internal void RemoveLoan(Book book)
        {
            _borrowedBooks.Remove(book);
        }

        public bool CanBorrow()
        {
            return Loans.Count(l => !l.IsReturned) < MaxActiveLoans;
        }

        public string GetInfo()
        {
            return $"{Name} ({Email}) - Member since {MemberSince:d}";
        }
    }
}
