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
    using BuiltIn.FortranObjects;
    using Helper;
    using Reporting;

    public class Traverser
    {
        private readonly Dictionary<Type, Action<object>> _actions;

        public Traverser(string name, params ITraverserAction[] actions)
        {
            this.Name = name;
            _actions = actions.ToDictionary(a => a.ForType, a => a.Act);
        }

        public IErrorListener<TraverserException> ErrorListener { get; set; } = new StandardErrorListener<TraverserException>();

        public string Name { get; }

        public void Go(ISource source)
        {
            var file = source as ISourceFile;
            Report.Message("Post processing", $"Applying '{this.Name}' on '{file?.AbsolutePath}'");
            Navigate(source);
        }

        private void DoActions(IFortranObject obj, Type type)
        {
            Action<object> act;
            if (!_actions.TryGetValue(type, out act))
            {
                return;
            }

            try
            {
                act(obj);
            }
            catch (TraverserException e)
            {
                this.ErrorListener.Error(e);
            }
        }

        private void Navigate(IFortranObject obj)
        {
            this.DoActions(obj, obj.GetType());
            foreach (var inter in obj.GetType().GetInterfaces())
            {
                this.DoActions(obj, inter);
            }

            for (var i = obj.SubObjects.Count - 1; i >= 0; i--)
            {
                Navigate(obj.SubObjects[i]);
            }
        }
    }

    public class TraverserException : ApplicationException
    {
        public TraverserException(IFortranObject fortranObject, string message)
            : base(message)
        {
            this.FortranObject = fortranObject;
        }

        public IFortranObject FortranObject { get; }
    }
}