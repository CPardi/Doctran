//  Copyright © 2015 Christopher Pardi
//  This Source Code Form is subject to the terms of the Mozilla Public
//  License, v. 2.0. If a copy of the MPL was not distributed with this
//  file, You can obtain one at http://mozilla.org/MPL/2.0/.

using System;

namespace Doctran.Fbase.Common
{
    public class UserInformer
    {

        private static void addNewLine()
        {
#if RELEASE
            if (Console.CursorLeft != 0) Console.WriteLine("");
#endif
        }

        public static void GiveWarning(string name, string value)
        {
            if (EnvVar.Verbose >= 2)
            {
                addNewLine();
                Console.WriteLine("WARNING: In " + name + ". " + value);
            }
        }

        public static void GiveWarning(string filename, int startLine, int endLine, string message)
        {
            if (EnvVar.Verbose >= 2)
            {
                addNewLine();
                Console.WriteLine("WARNING: In " + filename
                            + ". Error within lines '" + startLine
                            + "' to '" + endLine + ". "
                            + message);
            }
        }

        public static void GiveWarning(string name, Exception e)
        {
            if (EnvVar.Verbose >= 2)
            {
                addNewLine();
                Console.WriteLine("WARNING: In " + name + ". " + e.Message);
                throw new ApplicationException();
            }
        }

        public static void GiveError(string name, Exception e)
        {
#if DEBUG
            throw new ApplicationException(e.Message);
#endif

            addNewLine();
            Console.WriteLine("ERROR: In " + name + ". " + e.Message);
            Doctran.Fbase.Common.Helper.Stop();
        }

        public static void GiveError(string name, string value)
        {
#if DEBUG
            throw new ApplicationException(value);
#endif

            addNewLine();
            Console.WriteLine("ERROR: In " + name + ". " + value);
            Doctran.Fbase.Common.Helper.Stop();
        }

        public static void GiveError(string name, string value, Exception e)
        {
#if DEBUG
            throw new ApplicationException(e.Message);
#endif

            addNewLine();
            Console.WriteLine("ERROR: In " + name + ". " + value  + e.Message);
            Doctran.Fbase.Common.Helper.Stop();
        }
    }
}