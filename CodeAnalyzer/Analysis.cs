using CodeAnalyzer.Models;
using CodeAnalyzer.Repositories;

namespace CodeAnalyzer;

public class Analysis
{
    private readonly IContactRepository _contactRepository;

#pragma warning disable MLV001 // Types with TestOnlyAttribute cannot be used in production code
    public Analysis(IContactRepository contactRepository)
#pragma warning restore MLV001 // Types with TestOnlyAttribute cannot be used in production code
    {
        _contactRepository = contactRepository;
    }

    public void Invoke()
    {
        Contact contact = _contactRepository.Get();
        
        Console.Write($"Name: {contact.Name}");
    }
}
