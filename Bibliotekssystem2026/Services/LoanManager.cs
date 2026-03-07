using Bibliotekssystem2026.Models;

namespace Bibliotekssystem2026.Services
{
    public class LoanManager
    {
        private readonly List<Loan> _loans = new();

        public Loan CreateLoan(Book book, Member member, int days = 14)
        {
            var loan = new Loan(book, member, DateTime.Now, DateTime.Now.AddDays(days));
            _loans.Add(loan);
            return loan;
        }

        public IEnumerable<Loan> GetAllLoans() => _loans;
    }
}
