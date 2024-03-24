using CodeAnalyzer.Models;
using CodeAnalyzer.Repositories;

namespace CodeAnalyzer;

public class Analyzer
{
    private readonly IContactRepository _contactRepository;

#pragma warning disable MST001 // Types with TestOnlyAttribute cannot be used in production code
    public Analyzer(IContactRepository contactRepository)
#pragma warning restore MST001 // Types with TestOnlyAttribute cannot be used in production code
    {
        _contactRepository = contactRepository;
    }

    public void Invoke()
    {
        Contact contact = _contactRepository.Get();
        
        Console.Write($"Name: {contact.Name}");
    }
}
