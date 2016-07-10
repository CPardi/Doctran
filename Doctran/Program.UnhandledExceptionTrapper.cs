namespace Doctran
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Xml.Linq;
    using ParsingElements.FortranObjects;
    using Plugins;

    public partial class Program
    {
        private static void UnhandledExceptionTrapper(object sender, UnhandledExceptionEventArgs e)
        {
            try
            {
                var ex = (Exception)e.ExceptionObject;

                var logPath = Path.GetFullPath("errors.log");
                Console.WriteLine("\ninternal error");
                Console.WriteLine("Exception details written to " + logPath);
                using (var sq = new StreamWriter(logPath, false))
                {
                    sq.WriteLine("======Exception Type======");
                    sq.WriteLine(ex.GetType().Name);
                    sq.WriteLine(string.Empty);

                    sq.WriteLine("======Message======");
                    sq.WriteLine(ex.Message);
                    sq.WriteLine(string.Empty);

                    sq.WriteLine("======StackTrace======");
                    sq.WriteLine(ex.StackTrace);
                    sq.WriteLine(string.Empty);

                    sq.WriteLine("======Data======");
                    sq.WriteLine(ex.Data);
                    sq.WriteLine(string.Empty);

                    sq.WriteLine("======InnerException======");
                    sq.WriteLine(ex.InnerException);
                    sq.WriteLine(string.Empty);

                    sq.WriteLine("======Source======");
                    sq.WriteLine(ex.Source);
                    sq.WriteLine(string.Empty);
                }
            }
            finally
            {
                Environment.Exit(1);
            }
        }
    }
}