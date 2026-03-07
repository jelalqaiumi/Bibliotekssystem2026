namespace LibrarySystem.Exceptions;

public class BookAlreadyLoanedException : Exception
{
    public BookAlreadyLoanedException()
        : base("The book is already loaned.")
    {
    }
}