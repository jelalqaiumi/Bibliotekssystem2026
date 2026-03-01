using Bibliotekssystem2026.Models;

namespace Bibliotekssystem2026.Services
{
    public class MemberRegistry
    {
        private readonly List<Member> _members = new();

        public void AddMember(Member member) => _members.Add(member);

        public Member? GetMostActiveBorrower() =>
            _members.OrderByDescending(m => m.BorrowedBooks.Count)
                    .FirstOrDefault();
    }
}
