// <copyright file="Traverser.cs" company="Christopher Pardi">
//     Copyright © 2015 Christopher Pardi
//     This Source Code Form is subject to the terms of the Mozilla Public
//     License, v. 2.0. If a copy of the MPL was not distributed with this
//     file, You can obtain one at http://mozilla.org/MPL/2.0/.
// </copyright>

namespace Doctran.Parsing
{
    using System;
    using System.Linq;
    using Helper;
    using ParsingElements;
    using ParsingElements.FortranObjects;
    using Reporting;
    using Utilitys;

    public class Traverser
    {
        private readonly ILookup<Type, Action<object>> _actions;

        public Traverser(string name, params ITraverserAction[] actions)
        {
            this.Name = name;
            _actions = actions.ToLookup(a => a.ForType, a => a.Act);
        }

        public IErrorListener<TraverserException> ErrorListener { get; set; } = new StandardErrorListener<TraverserException>();

        public string Name { get; }

        public void Go(Project source)
        {
            var project = source;
            Report.Message("Post processing", $"Applying '{this.Name}' on '{project.ObjectName}'");
            this.Navigate(source);
        }

        public void Go(ISourceFile source)
        {
            var file = source;
            Report.Message("Post processing", $"Applying '{this.Name}' on '{file?.AbsolutePath}'");
            this.Navigate(source);
        }

        private void DoActions(IFortranObject obj, Type type)
        {
            var actionsForType = _actions[type].ToList();
            if (!actionsForType.Any())
            {
                return;
            }

            try
            {
                foreach (var act in actionsForType)
                {
                    act(obj);
                }
            }
            catch (TraverserException e)
            {
                this.ErrorListener.Error(e);
            }
        }

        private void Navigate(IFortranObject obj)
        {
            obj.GetType().ForTypeAndInterfaces(t => this.DoActions(obj, t));

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