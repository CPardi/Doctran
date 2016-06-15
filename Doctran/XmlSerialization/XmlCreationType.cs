namespace Doctran.XmlSerialization
{
    public enum XmlCreationType
    {
        /// <summary>
        /// Specifies that all XML information should be outputted, including from the object, interfaces and sub-objects.
        /// </summary>
        All,

        /// <summary>
        /// Specifies that only the XML value for the object should be outputted. This means information from interfaces and sub-objects will be omitted.
        /// </summary>
        ObjectOnly
    }
}