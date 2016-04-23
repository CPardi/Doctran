namespace Doctran.ParsingElements.Scope
{
    public interface IScope
    {
        bool GetObjectFromIdentifier(string identifier, out IHasIdentifier obj);
    }
}