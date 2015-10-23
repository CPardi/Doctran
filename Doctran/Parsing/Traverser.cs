//  Copyright © 2015 Christopher Pardi
//  This Source Code Form is subject to the terms of the Mozilla Public
//  License, v. 2.0. If a copy of the MPL was not distributed with this
//  file, You can obtain one at http://mozilla.org/MPL/2.0/.

namespace Doctran.Parsing
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using FortranObjects;
    using Utilitys;

    public class Traverser
    {

        private List<PostAction> postActions;

        public Traverser(params PostAction[] postActions)
        {
            this.postActions = postActions.ToList();
        }

        public void AddAction(PostAction action)
        {
            this.postActions.Add(action);
        }

        public void AddActions(List<PostAction> actions)
        {
            this.postActions.AddRange(actions);
        }

        public void Go(File file)
        {
            Navigate(file);
        }

        protected void Navigate(FortranObject obj)
        {
            if (EnvVar.Verbose >= 3 && obj is File) Console.WriteLine("Post processing: " + obj.Name + ((File) obj).Info.Extension);

            foreach (PostAction block in this.postActions)
                if (block.Is(obj)) { block.PostObject(ref obj); }

            for (int i = obj.SubObjects.Count - 1; i >= 0; i-- )
            {
                Navigate(obj.SubObjects[i]);
            }
        }
    }
}
