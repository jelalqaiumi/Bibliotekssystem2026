namespace Bibliotekssystem2026.Exceptions
{
    public class MaxLoansExceededException : Exception
    {
        public MaxLoansExceededException()
            : base("Member has reached maximum allowed active loans.")
        {
        }
    }
}
