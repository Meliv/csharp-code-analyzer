namespace CodeAnalyzer.Models
{
    public class Contact
    {
        public Guid Id { get; init; }
        public string Name { get; init; } = null!;
        public DateTime DateOfBirth { get; init; }
    }
}