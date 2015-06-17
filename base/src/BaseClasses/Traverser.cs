//  Copyright © 2015 Christopher Pardi
//  This Source Code Form is subject to the terms of the Mozilla Public
//  License, v. 2.0. If a copy of the MPL was not distributed with this
//  file, You can obtain one at http://mozilla.org/MPL/2.0/.

using System;
using System.Collections.Generic;

using Doctran.BaseClasses;
using Doctran.Fbase.Common;

using Doctran.Fbase.Projects;
using Doctran.Fbase.Files;

namespace Doctran.BaseClasses
{
    public class Traverser
    {

        private List<PostAction> postActions;

        public Traverser(List<PostAction> postActions)
        {
            this.postActions = postActions;
        }

        public void AddAction(PostAction action)
        {
            this.postActions.Add(action);
        }

        public void AddActions(List<PostAction> actions)
        {
            this.postActions.AddRange(actions);
        }

        public void Go(Project project)
        {
            Navigate(project);

            //foreach (PostAction block in this.postActions)
            //    if (block.Is(project)) { block.PostObject(ref project); }

            //foreach (File file in project.SubObjectsOfType<File>())
            //{
            //    if (Settings.verbose >= 3) Console.WriteLine("Post processing: " + file.Name + file.Info.Extension);
            //    Navigate(file);
            //}
        }

        protected void Navigate(FortranObject obj)
        {
            if (Settings.verbose >= 3 && obj is File) Console.WriteLine("Post processing: " + obj.Name + (obj as File).Info.Extension);

            foreach (PostAction block in this.postActions)
                if (block.Is(obj)) { block.PostObject(ref obj); }

            for (int i = obj.SubObjects.Count - 1; i >= 0; i-- )
            {
                Navigate(obj.SubObjects[i]);
            }
        }
    }
}
