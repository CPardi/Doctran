// <copyright file="Traverser.cs" company="Christopher Pardi">
//     Copyright © 2015 Christopher Pardi
//     This Source Code Form is subject to the terms of the Mozilla Public
//     License, v. 2.0. If a copy of the MPL was not distributed with this
//     file, You can obtain one at http://mozilla.org/MPL/2.0/.
// </copyright>

namespace Doctran.Parsing
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Helper;
    using ParsingElements;
    using ParsingElements.FortranObjects;
    using ParsingElements.Scope;
    using Reporting;
    using Utilitys;

    public class Traverser
    {
        private readonly ILookup<Type, ITraverserAction> _actions;

        public Traverser(string name, params ITraverserAction[] actions)
        {
            this.Name = name;
            _actions = actions.ToLookup(a => a.ForType, new CompareRootTypes(actions.Select(a => a.ForType)));
        }

        public IErrorListener<TraverserException> ErrorListener { get; set; } = new StandardErrorListener<TraverserException>();

        public string Name { get; }

        public IEnumerable<ITraverserAction> TraverserActions => _actions.SelectMany(a => a);

        public void Go(IContainer source)
        {
            Report.Message("Post processing", $"Applying '{this.Name}' on '{source.ObjectName}'");
            this.Navigate(source);
        }

        public void Go(ISourceFile source)
        {
            Report.Message("Post processing", $"Applying '{this.Name}' on '{source.AbsolutePath}'");
            this.Navigate(source);
        }

        private void DoActions(IFortranObject obj, Type type)
        {
            var actionsForType = _actions[type].ToList();
            if (!actionsForType.Any())
            {
                return;
            }

            var errorListener = new ErrorListener<TraverserException>(w => this.ErrorListener.Warning(w), e => this.ErrorListener.Error(e));
            foreach (var act in actionsForType)
            {
                act.Act(obj, errorListener);
            }
        }

        private void Navigate(IFortranObject obj)
        {
            this.DoActions(obj, obj.GetType());

            var asContainer = (obj as IContainer)?.SubObjects;
            if (asContainer == null)
            {
                return;
            }

            for (var i = asContainer.Count - 1; i >= 0; i--)
            {
                this.Navigate(asContainer[i]);
            }
        }
    }
}