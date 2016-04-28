namespace Doctran.ParsingElements
{
    public abstract class IdentifierBase
    {
        protected IdentifierBase(string originalString)
        {
            this.OriginalString = originalString;
        }

        public string OriginalString { get; }

        public abstract override int GetHashCode();

        public abstract override bool Equals(object obj);

        public abstract override string ToString();

        public static bool operator ==(IdentifierBase obj1, IdentifierBase obj2)
        {
            return obj1?.Equals(obj2) ?? false;
        }

        public static bool operator !=(IdentifierBase obj1, IdentifierBase obj2)

        {
            return !obj1?.Equals(obj2) ?? false;
        }
    }
}