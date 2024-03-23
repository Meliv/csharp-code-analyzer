using CodeAnalyzer;
using CodeAnalyzer.Repositories;
using CodeAnalyzer.Repositories.Internals;
using Microsoft.Extensions.DependencyInjection;

internal class Program
{
    private static void Main(string[] args)
    {
        ServiceProvider serviceProvider = new ServiceCollection()
            .AddTransient<IContactRepository, ContactRepository>()
            .BuildServiceProvider();

        IContactRepository contactRepository = serviceProvider.GetRequiredService<IContactRepository>();

        Analyzer analyzer = new Analyzer(contactRepository);

        analyzer.Invoke();
    }
}