namespace Doctran.ParsingElements.Scope
{
    public interface IScope
    {
        bool GetObjectFromName(string name, out IHasIdentifier obj);
    }
}