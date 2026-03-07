using Bibliotekssystem2026.Exceptions;
using LibrarySystem.Exceptions;

namespace Bibliotekssystem2026.Models
{
    public class Loan
    {
        public Book Book { get; }
        public Member Member { get; }
        public DateTime LoanDate { get; }
        public DateTime DueDate { get; }
        public DateTime? ReturnDate { get; private set; }

        public bool IsReturned => ReturnDate.HasValue;

        public bool IsOverdue =>
            !IsReturned && DateTime.Now > DueDate;

        public int DaysOverdue =>
            IsOverdue ? (DateTime.Now - DueDate).Days : 0;

        public Loan(Book book, Member member, DateTime loanDate, DateTime dueDate)
        {
            if (!member.CanBorrow())
                throw new MaxLoansExceededException();

            Book = book;
            Member = member;
            LoanDate = loanDate;
            DueDate = dueDate;

            book.IsAvailable = false;
            member.AddLoan(book);
            member.AddLoanRecord(this);
        }

        public void ReturnBook()
        {
            if (!Book.IsAvailable)
                throw new BookAlreadyLoanedException();

            ReturnDate = DateTime.Now;
            Book.IsAvailable = true;
            Member.RemoveLoan(Book);
        }

        public decimal CalculateLateFee(decimal dailyRate)
        {
            if (dailyRate < 0)
                throw new ArgumentException("Daily rate must be positive.");

            return DaysOverdue * dailyRate;
        }
    }
}
