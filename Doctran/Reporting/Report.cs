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
    using Helper;
    using Utilitys;

    public static class Report
    {
        public static ReportMode ReportMode { get; set; } = ReportMode.Debug;

        public static int Verbose { get; set; }

        public static void ContinueStatus(string message)
        {
            if (Verbose < 2)
            {
                return;
            }

            Console.Write(message);
        }

        public static void Error<TException>(Action<ConsolePublisher> publish, TException error)
            where TException : Exception
        {
            var publisher = new ConsolePublisher();
            publish(publisher);

            if (ReportMode == ReportMode.Release)
            {
                publisher.Publish(ReportSeverity.Error);
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
                    throw exception;
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
                    publisher.Publish(ReportSeverity.Error);
                }

                Environment.Exit(1);
            }

            ThrowExceptions(errorList);
        }

        public static void Message(string title, string text)
        {
            if (Verbose < 3)
            {
                return;
            }

            var ttb = new TitledTextBuilder();
            ttb.Append(title, text);
            OtherUtils.ConsoleGotoNewLine();
            Console.Write(ttb.ToString());
        }

        public static void MessageAndExit(string message)
        {
            OtherUtils.ConsoleGotoNewLine();
            Console.WriteLine(message);
            Environment.Exit(1);
        }

        public static void NewStatus(string message)
        {
            if (Verbose < 2)
            {
                return;
            }

            OtherUtils.ConsoleGotoNewLine();
            Console.Write(message);
        }

        public static void SetDebugProfile() => ReportMode = ReportMode.Debug;

        public static void SetReleaseProfile() => ReportMode = ReportMode.Release;

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
            if (Verbose < 2)
            {
                return;
            }

            var publisher = new ConsolePublisher();
            publish(publisher);
            publisher.Publish(ReportSeverity.Warning);

            if (ReportMode == ReportMode.Debug)
            {
                throw new ApplicationException("Exception from publisher.");
            }
        }

        public static void Warnings<TException>(Action<ConsolePublisher, TException> publish, IEnumerable<TException> warnings)
            where TException : Exception
        {
            var warningsList = warnings as IList<TException> ?? warnings.ToList();
            if (Verbose < 2 && !warningsList.Any())
            {
                return;
            }

            if (ReportMode == ReportMode.Release)
            {
                foreach (var warning in warningsList)
                {
                    var publisher = new ConsolePublisher();
                    publish(publisher, warning);
                    publisher.Publish(ReportSeverity.Warning);
                }
            }

            if (ReportMode == ReportMode.Debug)
            {
                ThrowExceptions(warningsList);
            }
        }

        private static void ApplicationExit()
        {
            Environment.Exit(-1);
        }

        private static void ThrowExceptions<TException>(ICollection<TException> errorList)
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