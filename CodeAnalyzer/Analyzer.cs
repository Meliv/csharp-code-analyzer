using CodeAnalyzer.Models;
using CodeAnalyzer.Repositories;

namespace CodeAnalyzer;

public class Analyzer
{
    private readonly IContactRepository _contactRepository;

    public Analyzer(IContactRepository contactRepository)
    {
        _contactRepository = contactRepository;
    }

    public void Invoke()
    {
        Contact contact = _contactRepository.Get();
        
        Console.Write($"Name: {contact.Name}");
    }
}
