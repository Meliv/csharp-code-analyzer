using CodeAnalyzer.CustomAttributes;
using CodeAnalyzer.Models;

namespace CodeAnalyzer.Repositories
{
    [TestOnly]
    public interface IContactRepository
    {
        public Contact Get();
    }
}