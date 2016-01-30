// <copyright file="License.cs" company="Christopher Pardi">
//     Copyright (c) 2015 Christopher Pardi. All rights reserved.
// </copyright>

namespace Doctran.Licensing
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text.RegularExpressions;
    using global::Licensing;
    using Plugins;
    using Reporting;

    public static class LicenseManager
    {
        private static IEnumerable<LicensablePlugin> LicensablePlugins
            => from plugin in PluginManager.Plugins.Where(p => p is LicensablePlugin).Cast<LicensablePlugin>()
                select plugin;

        private static IEnumerable<LicensablePlugin> LicensedPlugins
            => from plugin in LicensablePlugins
                where plugin.Licensed
                select plugin;

        private static Dictionary<int, LicensablePlugin> UnlicensedPlugins
        {
            get
            {
                var dict = new Dictionary<int, LicensablePlugin>();
                var index = 1;
                foreach (var plugin in LicensablePlugins.Where(plugin => !plugin.Licensed))
                {
                    dict[index] = plugin;
                    index++;
                }

                return dict;
            }
        }

        public static void Show()
        {
            bool validInput;

            WritePlugins();
            if (!UnlicensedPlugins.Any())
            {
                return;
            }

            LicensablePlugin plugin = null;
            do
            {
                validInput = true;
                try
                {
                    plugin = GetPlugin();
                }
                catch (IndexOutOfRangeException)
                {
                    WritePlugins();
                    Report.Message("Error", "Please enter an integer value within the range above.");
                    validInput = false;
                }
                catch (FormatException)
                {
                    WritePlugins();
                    Report.Message("Error", "Please enter an integer value within the range of the list above.");
                    validInput = false;
                }
            }
            while (!validInput);

            var email = string.Empty;
            do
            {
                validInput = true;
                try
                {
                    email = GetEmail();
                }
                catch (FormatException)
                {
                    Report.Message("Error", "The email address entered is invalid.");
                    validInput = false;
                }
            }
            while (!validInput);

            var attempt = 0;
            do
            {
                validInput = GetActivation(email, plugin);
                attempt++;
            }
            while (!validInput && attempt < 3);

            if (attempt == 3)
            {
                Report.Error(
                    pub => pub.DescriptionReason(ReportGenre.Licensing, "Incorrect key entered 3 times. Exiting"), new Exception("Incorrect key entered 3 times."));
            }
            else
            {
                Console.WriteLine("License key validated. Thank you for your purchase.");
            }
        }

        private static bool GetActivation(string email, LicensablePlugin plugin)
        {
            Console.WriteLine("Enter your license key.   ");
            Console.Write("> ");
            var key = Console.ReadLine();
            if (LicenseVerifier.ValidKey(email, key, plugin.GetSecretKey()))
            {
                LicenseCreator.CreateLicense(plugin.GetLicensePath(), email, key, plugin.GetSecretKey());
                return true;
            }

            Console.WriteLine("License key is not valid.");
            return false;
        }

        private static string GetEmail()
        {
            Console.WriteLine("Enter the email address you used to purchase the license.   ");
            Console.Write("> ");
            var email = Console.ReadLine() ?? string.Empty;
            if (!Regex.IsMatch(email, @".+@.+\..+"))
            {
                throw new FormatException();
            }

            return email;
        }

        private static LicensablePlugin GetPlugin()
        {
            Console.WriteLine("Enter the number of the plugin you wish to license.   ");
            Console.Write("> ");
            var input = Console.ReadLine();
            int pluginNo = Convert.ToInt16(input);
            LicensablePlugin plugin;
            try
            {
                plugin = UnlicensedPlugins[pluginNo];
            }
            catch
            {
                throw new IndexOutOfRangeException();
            }

            return plugin;
        }

        private static void WritePlugins()
        {
            if (LicensedPlugins.Any())
            {
                Console.WriteLine(Environment.NewLine + "Licensed Plugins");
            }

            foreach (var plugin in LicensedPlugins)
            {
                Console.WriteLine(" - " + plugin.FriendlyName + ", Licensed to " + plugin.GetLicenseEmail());
            }

            if (UnlicensedPlugins.Any())
            {
                Console.WriteLine(Environment.NewLine + "Unlicensed Plugins");
            }

            foreach (var iPlugin in UnlicensedPlugins)
            {
                Console.WriteLine(" [" + iPlugin.Key + "] - " + iPlugin.Value.FriendlyName);
            }
        }
    }
}