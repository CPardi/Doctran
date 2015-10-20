namespace Doctran.Input.OptionFile
{
    using System.Collections.Generic;
    using Comments;
    using Helper;
    using Parsing;

    class OptionFactory : IInformationFactory
    {
        public string Name { get; private set; }

        public OptionFactory(string name)
        {
            this.Name = name;
        }

        public IEnumerable<IInformation> Create(int depth, string value, IEnumerable<FortranObject> subObjects, List<FileLine> lines)
        {
            yield return new Option(depth, this.Name, value, lines);
        }
    }

    class Option : FortranObject, IInformation
    {
        public string Value { get; private set; }
        public int Depth { get; private set; }

        public Option(int depth, string name, string value, List<FileLine> lines)
            : base(name,lines)
        {
            this.Value = value;
            this.Depth = depth;
        }

        protected override string GetIdentifier()
        {
            return "Option";
        }
    }
}
