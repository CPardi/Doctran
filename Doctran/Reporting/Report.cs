namespace Doctran.Reporting
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using CommandLine.Text;
    using Fbase.Common;

    public class Report
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

        public static void MessageThenExit(Action<ConsolePublisher> publish)
        {
            var publisher = new ConsolePublisher();
            publish(publisher);
            publisher.Publish();
            Environment.Exit(1);
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
        }

        public static void Warning(Action<ConsolePublisher> publish)
        {
            var publisher = new ConsolePublisher();
            publish(publisher);
            publisher.Publish();
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

        public void Error(Exception exception)
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

        private static void ApplicationExit()
        {
            Environment.Exit(-1);
        }

        private static void Rethrow(Exception exception)
        {
            throw exception;
        }
    }
}