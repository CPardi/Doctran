namespace Doctran.Input.OptionFile
{
    using System.Collections.Generic;
    using Comments;
    using Helper;
    using Parsing;

    internal class OptionFactory : IInformationFactory
    {
        public OptionFactory(string name)
        {
            this.Name = name;
        }

        public string Name { get; }

        public IEnumerable<IInformation> Create(int depth, string value, IEnumerable<IFortranObject> subObjects, List<FileLine> lines)
        {
            yield return new Option(depth, this.Name, value, lines);
        }
    }

    internal class Option : FortranObject, IInformation
    {
        public Option(int depth, string name, string value, List<FileLine> lines)
            : base(lines)
        {
            this.Name = name;
            this.Value = value;
            this.Depth = depth;
        }

        public int Depth { get; }
        public string Name { get; }
        public string Value { get; }

        protected override string GetIdentifier()
        {
            return "Option";
        }
    }
}