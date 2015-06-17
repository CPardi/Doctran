//  Copyright © 2015 Christopher Pardi
//  This Source Code Form is subject to the terms of the Mozilla Public
//  License, v. 2.0. If a copy of the MPL was not distributed with this
//  file, You can obtain one at http://mozilla.org/MPL/2.0/.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Doctran.Fbase.Common
{
    public class UserInformer
    {

        private static void addNewLine()
        {
            if (Console.CursorLeft != 0) Console.WriteLine("");
        }

        public static void GiveWarning(String name, String value)
        {
            if (Settings.verbose >= 2)
            {
                addNewLine();
                Console.WriteLine("WARNING: In " + name + ". " + value + ".");
            }
        }

        public static void GiveWarning(String filename, int startLine, int endLine, string message)
        {
            if (Settings.verbose >= 2)
            {
                addNewLine();
                Console.WriteLine("WARNING: In " + filename
                            + ". Error within lines '" + startLine
                            + "' to '" + endLine + ". "
                            + message
                            + ".");
            }
        }

        public static void GiveWarning(String name, Exception e)
        {
            if (Settings.verbose >= 2)
            {
                addNewLine();
                Console.WriteLine("WARNING: In " + name + ". " + e.Message + ".");
                throw new ApplicationException();
            }
        }

        public static void GiveError(String name, Exception e)
        {
            addNewLine();
            Console.WriteLine("ERROR: In " + name + ". " + e.Message + ".");
            Doctran.Fbase.Common.Helper.Stop();
        }

        public static void GiveError(String name, String value)
        {
            addNewLine();
            Console.WriteLine("ERROR: In " + name + ". " + value + ".");
            Doctran.Fbase.Common.Helper.Stop();
        }

        public static void GiveError(String name, String value, Exception e)
        {
            addNewLine();
            Console.WriteLine("ERROR: In " + name + ". " + value + ". " + e.Message);
            Doctran.Fbase.Common.Helper.Stop();
        }
    }
}