using Bibliotekssystem2026.Exceptions;
using Bibliotekssystem2026.Models;
using Xunit;

namespace Bibliotekssystem2026.LibrarySystem.Tests
{
    public class LoanTests
    {
        [Fact]
        public void CreateLoan_ShouldThrow_WhenMaxLoansReached()
        {
            var member = new Member("M1", "Test", "test@test.com");

            for (int i = 0; i < Member.MaxActiveLoans; i++)
            {
                var book = new Book($"ISBN{i}", "Title", "Author", 2024);
                new Loan(book, member, DateTime.Now, DateTime.Now.AddDays(7));
            }

            var extraBook = new Book("Extra", "Title", "Author", 2024);

            Assert.Throws<MaxLoansExceededException>(() =>
                new Loan(extraBook, member, DateTime.Now, DateTime.Now.AddDays(7)));
        }

        [Fact]
        public void CalculateLateFee_ShouldReturnCorrectAmount()
        {
            var book = new Book("1", "Title", "Author", 2024);
            var member = new Member("M1", "Test", "test@test.com");

            var loan = new Loan(book, member, DateTime.Now.AddDays(-20), DateTime.Now.AddDays(-10));

            var fee = loan.CalculateLateFee(10);

            Assert.True(fee > 0);
        }
    }
}