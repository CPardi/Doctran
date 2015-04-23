//  Copyright © 2015 Christopher Pardi
//  This Source Code Form is subject to the terms of the Mozilla Public
//  License, v. 2.0. If a copy of the MPL was not distributed with this
//  file, You can obtain one at http://mozilla.org/MPL/2.0/.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Doctran
{
    public class UserInformer
    {
        private string nl = Environment.NewLine;

        private static void addNewLine()
        {
            if (Console.CursorLeft != 0) Console.WriteLine("");
        }

        public static void GiveWarning(String name, Exception e)
        {
            addNewLine();
            Console.WriteLine("ERROR: In " + name + ". " + e.Message);
        }

        public static void GiveError(String name, Exception e)
        {
            addNewLine();
            Console.WriteLine("ERROR: In " + name + ". " + e.Message);
            Doctran.Fbase.Common.Helper.Stop();
        }

        public static void GiveError(String name, String value, Exception e)
        {
            addNewLine();
            Console.WriteLine("ERROR: In " + name + " " + value + ". " + e.Message);
            Doctran.Fbase.Common.Helper.Stop();
        }
    }
}