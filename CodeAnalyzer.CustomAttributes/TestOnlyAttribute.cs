namespace CodeAnalyzer.CustomAttributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface)]
    public class TestOnlyAttribute : Attribute { }
}