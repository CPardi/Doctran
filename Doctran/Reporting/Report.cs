// <copyright file="Report.cs" company="Christopher Pardi">
//     Copyright © 2015 Christopher Pardi
//     This Source Code Form is subject to the terms of the Mozilla Public
//     License, v. 2.0. If a copy of the MPL was not distributed with this
//     file, You can obtain one at http://mozilla.org/MPL/2.0/.
// </copyright>

namespace Doctran.Reporting
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using CommandLine.Text;
    using Utilitys;

    public static class Report
    {
        public static ReportMode ReportMode { get; set; } = ReportMode.Debug;

        public static void Error<TException>(Action<ConsolePublisher, TException> publish, TException error)
            where TException : Exception
        {
            var publisher = new ConsolePublisher();
            publish(publisher, error);
            if (ReportMode == ReportMode.Release)
            {
                publisher.Publish();
                Environment.Exit(1);
            }

            if (ReportMode == ReportMode.Debug)
            {
                throw error;
            }
        }

        public static void Error(Exception exception)
        {
            switch (ReportMode)
            {
                case ReportMode.Debug:
                    Rethrow(exception);
                    break;
                case ReportMode.Release:
                {
                    ApplicationExit();
                    break;
                }
            }
        }

        public static void Errors<TException>(Action<ConsolePublisher, TException> publish, IEnumerable<TException> errors, bool terminate = true)
            where TException : Exception
        {
            var errorList = errors as IList<TException> ?? errors.ToList();
            if (!errorList.Any())
            {
                return;
            }

            if (ReportMode == ReportMode.Release)
            {
                foreach (var error in errorList)
                {
                    var publisher = new ConsolePublisher();
                    publish(publisher, error);
                    publisher.Publish();
                }

                Environment.Exit(1);
            }

            ThrowExceptions(errorList);
        }

        public static void MessageThenExit(Action<ConsolePublisher> publish)
        {
            var publisher = new ConsolePublisher();
            publish(publisher);
            publisher.Publish();
            Environment.Exit(1);
        }

        public static void SetDebugProfile()
        {
            ReportMode = ReportMode.Debug;
        }

        public static void SetReleaseProfile()
        {
            ReportMode = ReportMode.Release;
        }

        public static string UsageString<TOptions>(TOptions options)
        {
            var assemblyTitle = typeof(Program).Assembly.GetAssemblyAttribute<AssemblyTitleAttribute>().Title;
            var assemblyVersion = typeof(Program).Assembly.GetName().Version.ToString();
            var assemblyCompany = typeof(Program).Assembly.GetAssemblyAttribute<AssemblyCompanyAttribute>().Company;

            var help = new HelpText
            {
                Heading = new HeadingInfo(assemblyTitle, assemblyVersion),
                Copyright = new CopyrightInfo(assemblyCompany, 2015),
                AdditionalNewLineAfterOption = true,
                AddDashesToOption = true
            };

            help.AddPreOptionsLine($"{Environment.NewLine}Usage: doctran [options] [source_files]");
            help.AddOptions(options);
            return help;
        }

        public static void Warning(Action<ConsolePublisher> publish)
        {
            var publisher = new ConsolePublisher();
            publish(publisher);
            publisher.Publish();

            if (ReportMode == ReportMode.Debug)
            {
                throw new ApplicationException("Exception from publisher.");
            }
        }

        public static void Warnings<TException>(Action<ConsolePublisher, TException> publish, IEnumerable<TException> warnings)
            where TException : Exception
        {
            var warningsList = warnings as IList<TException> ?? warnings.ToList();
            if (!warningsList.Any())
            {
                return;
            }

            foreach (var warning in warningsList)
            {
                var publisher = new ConsolePublisher();
                publish(publisher, warning);
                publisher.Publish();
            }

            ThrowExceptions(warningsList);
        }

        private static void ApplicationExit()
        {
            Environment.Exit(-1);
        }

        private static void Rethrow(Exception exception)
        {
            throw exception;
        }

        private static void ThrowExceptions<TException>(IList<TException> errorList) 
            where TException : Exception
        {
            if (ReportMode != ReportMode.Debug)
            {
                return;
            }

            if (errorList.Count == 1)
            {
                throw errorList.Single();
            }

            throw new AggregateException(errorList);
        }
    }
}