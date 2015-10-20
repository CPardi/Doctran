namespace Doctran.Parsing.PostActions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using FortranObjects;
    using Parsing;
    using Utilitys;

    public class ProjectPostAction : PostAction
    {
        private List<File> _files;

        public ProjectPostAction() : base(typeof(Project)) { }

        public override void PostObject(ref FortranObject obj)
        {
            LoadFiles(obj);
            CheckFilenames(0);            
        }

        private void CheckFilenames(int depth)
        {
            List<string> sameNames = new List<string>();
            sameNames.AddRange((from file in _files
                where _files.Count(f => f.Name.ToLower() == file.Name.ToLower()) > 1
                select file.Name.ToLower()).Distinct());
            if (!sameNames.Any()) return;
            
            foreach (var name in sameNames)
            {
                var list = _files.Where(f => f.Name.ToLower() == name);
                foreach (var file in list)
                {
                    System.IO.DirectoryInfo path = System.IO.Directory.GetParent(file.PathAndFilename);
                    for (int d = 0; d < depth; d++) { path = path.Parent; }

                    file.Name = path.Name.ToLower() + EnvVar.Slash + file.Name;

                    Console.WriteLine(file.Name);
                }
            }

            CheckFilenames(depth + 1);
        }

        private void LoadFiles(FortranObject obj)
        {
            if (_files == null) _files = obj.SubObjectsOfType<File>();
        }
    }
}