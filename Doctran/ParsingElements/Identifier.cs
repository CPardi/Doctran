namespace Doctran.ParsingElements
{
    public abstract class Identifier
    {
        protected Identifier(string originalString)
        {
            this.OriginalString = originalString;
        }

        public string OriginalString { get; }

        public static bool operator ==(Identifier obj1, Identifier obj2)
        {
            return obj1?.Equals(obj2) ?? false;
        }

        public static bool operator !=(Identifier obj1, Identifier obj2)

        {
            return !obj1?.Equals(obj2) ?? false;
        }

        public abstract override bool Equals(object obj);

        public abstract override int GetHashCode();

        public abstract override string ToString();
    }
}