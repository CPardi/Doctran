namespace Doctran.Parsing
{
    using System.Collections.Generic;

    /// <summary>
    ///     Exposes members that do not appear in <see cref="IContainer.SubObjects" />, but are generated dynamically. Used to
    ///     allow generic searching of project structure , such as in
    ///     <see cref="Traversal.Find{T}(ParsingElements.FortranObjects.Project, ParsingElements.Identifier)" />.
    /// </summary>
    public interface IQuasiContainer : IFortranObject
    {
        /// <summary>
        ///     Gets objects that do not appear in <see cref="IContainer.SubObjects" />, to allow access in a generic way.
        /// </summary>
        IEnumerable<IFortranObject> QuasiObjects { get; }
    }
}