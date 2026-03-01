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

        public Loan(Book book, Member member, DateTime loanDate, DateTime dueDate)
        {
            Book = book;
            Member = member;
            LoanDate = loanDate;
            DueDate = dueDate;

            book.IsAvailable = false;
            member.AddLoan(book);
        }

        public void ReturnBook()
        {
            if (IsReturned)
                throw new InvalidOperationException("Book already returned.");

            ReturnDate = DateTime.Now;
            Book.IsAvailable = true;
            Member.RemoveLoan(Book);
        }
    }
}
