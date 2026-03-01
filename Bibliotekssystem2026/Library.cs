using Bibliotekssystem2026.Services;

namespace Bibliotekssystem2026
{
    public class Library
    {
        public BookCatalog BookCatalog { get; } = new();
        public MemberRegistry MemberRegistry { get; } = new();
        public LoanManager LoanManager { get; } = new();
    }
}
