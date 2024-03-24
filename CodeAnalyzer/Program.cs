using CodeAnalyzer;
using CodeAnalyzer.Repositories;
using CodeAnalyzer.Repositories.Internals;
using Microsoft.Extensions.DependencyInjection;

internal class Program
{
    private static void Main(string[] _)
    {
        ServiceProvider serviceProvider = new ServiceCollection()
            .AddTransient<IContactRepository, ContactRepository>()
            .BuildServiceProvider();

        IContactRepository contactRepository = serviceProvider.GetRequiredService<IContactRepository>();

        Analysis analyzer = new(contactRepository);

        analyzer.Invoke();
    }
}