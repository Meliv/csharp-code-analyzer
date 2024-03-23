using CodeAnalyzer.Models;

namespace CodeAnalyzer.Repositories.Internals
{
    public class ContactRepository : IContactRepository
    {
        public Contact Get() => new Contact()
        {
            Id = Guid.NewGuid(),
            Name = Guid.NewGuid().ToString("N"),
            DateOfBirth = DateTime.Today
        };
    }
}