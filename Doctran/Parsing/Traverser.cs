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
        private readonly Dictionary<Type, Action<object>> _actions;

        public Traverser(params ITraverserAction[] actions)
        {
            _actions = actions.ToDictionary(a => a.ForType, a => a.Act);
        }

        public void Go(SourceFile sourceFile) => Navigate(sourceFile);

        protected void Navigate(FortranObject obj)
        {
            if (EnvVar.Verbose >= 3 && obj is SourceFile) Console.WriteLine("Post processing: " + obj.Name + ((SourceFile) obj).Info.Extension);

            Action<object> act;
            if (_actions.TryGetValue(obj.GetType(), out act))
            {
                act(obj);
            }

            for (var i = obj.SubObjects.Count - 1; i >= 0; i--)
            {
                Navigate(obj.SubObjects[i]);
            }
        }
    }
}
