using Bibliotekssystem2026;
using Bibliotekssystem2026.Exceptions;
using Bibliotekssystem2026.Models;

var library = new Library();

// Add sample data
SeedData(library);

bool running = true;

while (running)
{
    Console.WriteLine();
    Console.WriteLine("=== Library System ===");
    Console.WriteLine();
    Console.WriteLine("1. Show all books");
    Console.WriteLine("2. Search book");
    Console.WriteLine("3. Borrow book");
    Console.WriteLine("4. Return book");
    Console.WriteLine("5. Show members");
    Console.WriteLine("6. Statistics");
    Console.WriteLine("0. Exit");
    Console.WriteLine();
    Console.Write("Choose: ");

    var choice = Console.ReadLine();

    switch (choice)
    {
        case "1":
            ShowAllBooks();
            break;
        case "2":
            SearchBook();
            break;
        case "3":
            BorrowBook();
            break;
        case "4":
            ReturnBook();
            break;
        case "5":
            ShowMembers();
            break;
        case "6":
            ShowStatistics();
            break;
        case "0":
            running = false;
            Console.WriteLine("Goodbye!");
            break;
        default:
            Console.WriteLine("Invalid choice, try again.");
            break;
    }
}

void SeedData(Library lib)
{
    lib.BookCatalog.AddBook(new Book("978-91-0-012345-6", "Sagan om ringen", "J.R.R. Tolkien", 1954));
    lib.BookCatalog.AddBook(new Book("978-91-0-012346-3", "Hobbiten", "J.R.R. Tolkien", 1937));
    lib.BookCatalog.AddBook(new Book("978-91-0-012347-0", "1984", "George Orwell", 1949));
    lib.BookCatalog.AddBook(new Book("978-91-0-012348-7", "Pippi Långstrump", "Astrid Lindgren", 1945));
    lib.BookCatalog.AddBook(new Book("978-91-0-012349-4", "Bröderna Lejonhjärta", "Astrid Lindgren", 1973));
    lib.BookCatalog.AddBook(new Book("978-91-0-012350-0", "Ronja Rövardotter", "Astrid Lindgren", 1981));
    lib.BookCatalog.AddBook(new Book("978-91-0-012351-7", "Kallocain", "Karin Boye", 1940));
    lib.BookCatalog.AddBook(new Book("978-91-0-012352-4", "Doktor Glas", "Hjalmar Söderberg", 1905));
    lib.BookCatalog.AddBook(new Book("978-91-0-012353-1", "Nils Holgerssons underbara resa", "Selma Lagerlöf", 1906));
    lib.BookCatalog.AddBook(new Book("978-91-0-012354-8", "Mio, min Mio", "Astrid Lindgren", 1954));

    lib.MemberRegistry.AddMember(new Member("M001", "Anna Andersson", "anna@example.com"));
    lib.MemberRegistry.AddMember(new Member("M002", "Erik Svensson", "erik@example.com"));
    lib.MemberRegistry.AddMember(new Member("M003", "Maria Johansson", "maria@example.com"));
}

void ShowAllBooks()
{
    Console.WriteLine();
    var books = library.BookCatalog.GetAll().ToList();
    if (!books.Any())
    {
        Console.WriteLine("No books in the catalog.");
        return;
    }

    for (int i = 0; i < books.Count; i++)
    {
        var b = books[i];
        var status = b.IsAvailable ? "Available" : "Borrowed";
        Console.WriteLine($"{i + 1}. \"{b.Title}\" by {b.Author} ({b.PublishedYear}) - {status}");
    }
}

void SearchBook()
{
    Console.WriteLine();
    Console.Write("Search term: ");
    var term = Console.ReadLine();

    if (string.IsNullOrWhiteSpace(term))
    {
        Console.WriteLine("Enter a search term.");
        return;
    }

    var results = library.BookCatalog.Search(term).ToList();

    if (!results.Any())
    {
        Console.WriteLine("No results found.");
        return;
    }

    Console.WriteLine();
    Console.WriteLine("Search results:");
    for (int i = 0; i < results.Count; i++)
    {
        var b = results[i];
        var status = b.IsAvailable ? "Available" : "Borrowed";
        Console.WriteLine($"{i + 1}. \"{b.Title}\" by {b.Author} ({b.PublishedYear}) - {status}");
    }
}

void BorrowBook()
{
    Console.WriteLine();
    Console.Write("Enter ISBN: ");
    var isbn = Console.ReadLine();

    Console.Write("Enter member ID: ");
    var memberId = Console.ReadLine();

    var book = library.BookCatalog.GetAll().FirstOrDefault(b => b.ISBN == isbn);
    if (book == null)
    {
        Console.WriteLine("Book not found.");
        return;
    }

    if (!book.IsAvailable)
    {
        Console.WriteLine("The book is already borrowed.");
        return;
    }

    var member = library.MemberRegistry.GetAll().FirstOrDefault(m => m.MemberId == memberId);
    if (member == null)
    {
        Console.WriteLine("Member not found.");
        return;
    }

    try
    {
        var loan = library.LoanManager.CreateLoan(book, member);
        Console.WriteLine();
        Console.WriteLine($"The book \"{book.Title}\" has been borrowed by {member.Name}.");
        Console.WriteLine($"Due date: {loan.DueDate:yyyy-MM-dd}");
    }
    catch (MaxLoansExceededException ex)
    {
        Console.WriteLine(ex.Message);
    }
}

void ReturnBook()
{
    Console.WriteLine();
    Console.Write("Enter ISBN of the book to return: ");
    var isbn = Console.ReadLine();

    var loan = library.LoanManager.GetAllLoans()
        .FirstOrDefault(l => l.Book.ISBN == isbn && !l.IsReturned);

    if (loan == null)
    {
        Console.WriteLine("No active loan found for that book.");
        return;
    }

    loan.ReturnBook();
    Console.WriteLine($"The book \"{loan.Book.Title}\" has been returned by {loan.Member.Name}.");

    if (loan.DaysOverdue > 0)
    {
        var fee = loan.CalculateLateFee(10);
        Console.WriteLine($"The book was {loan.DaysOverdue} days overdue. Late fee: {fee} SEK.");
    }
}

void ShowMembers()
{
    Console.WriteLine();
    var members = library.MemberRegistry.GetAll().ToList();
    if (!members.Any())
    {
        Console.WriteLine("No members registered.");
        return;
    }

    foreach (var m in members)
    {
        var activeLoans = m.Loans.Count(l => !l.IsReturned);
        Console.WriteLine($"  {m.MemberId}: {m.Name} ({m.Email}) - Active loans: {activeLoans}");
    }
}

void ShowStatistics()
{
    Console.WriteLine();
    Console.WriteLine("=== Statistics ===");
    Console.WriteLine($"Total number of books: {library.BookCatalog.TotalBooks()}");
    Console.WriteLine($"Borrowed books: {library.BookCatalog.BorrowedBooksCount()}");

    var mostActive = library.MemberRegistry.GetAll()
        .OrderByDescending(m => m.Loans.Count(l => !l.IsReturned))
        .FirstOrDefault();

    if (mostActive != null)
    {
        var activeLoans = mostActive.Loans.Count(l => !l.IsReturned);
        Console.WriteLine($"Most active borrower: {mostActive.Name} ({activeLoans} active loans)");
    }
}