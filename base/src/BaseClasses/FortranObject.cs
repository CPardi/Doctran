//  Copyright © 2015 Christopher Pardi
//  This Source Code Form is subject to the terms of the Mozilla Public
//  License, v. 2.0. If a copy of the MPL was not distributed with this
//  file, You can obtain one at http://mozilla.org/MPL/2.0/.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml.Linq;

using Doctran.Fbase.Common;
using Doctran.BaseClasses;

using Doctran.Fbase.Projects;

namespace Doctran.BaseClasses
{
    public abstract class FortranObject
    {
        public FortranObject parent;
        public String Name { get; set; }
        public List<FortranObject> SubObjects = new List<FortranObject>();
        public List<FileLine> lines = new List<FileLine>();

        protected FortranObject() { }

        protected FortranObject(FortranObject parent, List<FileLine> lines, bool ContainsBlocks)
        {
            this.parent = parent;
            this.lines = lines;
            if (ContainsBlocks) { this.Search(); }
        }

        protected FortranObject(FortranObject parent, String name,List<FileLine> lines, bool ContainsBlocks)
        {
            if (Settings.verbose >= 3) Console.WriteLine("Analysing: " + name);

            this.parent = parent;
            this.Name = name;
            this.lines = lines;
            if (ContainsBlocks) 
            {
                try
                {
                    this.Search();
                }
                catch (UnclosedBlockException e)
                {
                    UserInformer.GiveError(this.Name, e.ToString());
                    Helper.Stop();
                }
            }
        }

        public String Identifier
        {
            get
            {
                return this.GetIdentifier().ToLower();
            }
        }

        protected abstract String GetIdentifier();

        public void Search()
        {
            // StartLine and EndLine store the line numbers at the beginning and end of the blocks.
            int StartLine, lineIndex = 1;

            FortranObject current = this;
            List<FortranBlock> Blocks = PluginManager.FortranBlocks; 
                       
            // Loop through the file lines from lineIndex till the last line.
            while (lineIndex < current.lines.Count)
            {
                //Ignor whitespace and non-doctran comments and go to the next coding line or if at end of file exit loop.
                if (Helper.GotoNextUsefulLine(current.lines, ref lineIndex)) break;
                bool BlockFound = false; int i = 0;
                do
                {
                    bool BlockStart = Blocks[i].BlockStart(this.GetType(), current.lines, lineIndex);

                    if (BlockStart)
                    {
                        StartLine = lineIndex;
						var internalBlocks = new List<FortranBlock>();internalBlocks.Clear();
                        var removalBlocks = new List<FortranBlock>();
                        do
                        {
                            if (lineIndex >= lines.Count)
                                throw new UnclosedBlockException(Blocks[i]);

                            if (Blocks[i].CheckInternal)
                            {
                                foreach (FortranBlock bk in Blocks)
                                    if (bk.BlockStart(this.GetType(), lines, lineIndex))
                                    {
                                        internalBlocks.Add(bk);
                                        break;
                                    }
										
                                removalBlocks.Clear();

                                foreach (FortranBlock bk in Blocks)
                                    if (bk.BlockEnd(this.GetType(), lines, lineIndex))
                                        removalBlocks.Add(bk);
                                    

                                foreach (FortranBlock bk in removalBlocks)
                                    internalBlocks.Remove(bk);
							}

                            if (!internalBlocks.Any() && Blocks[i].BlockEnd(this.GetType(), current.lines, lineIndex)) break;
                            lineIndex++;

                        } while (true);
                        current.SubObjects.AddRange(Blocks[i].ReturnObject(current, current.lines.GetRange(StartLine, lineIndex - StartLine + 1)));
                        BlockFound = true;
                        break;
                    }
                    i++;
                } while (!BlockFound & i < Blocks.Count());
                lineIndex++;
                Helper.GotoNextUsefulLine(current.lines, ref lineIndex);
            }
        }
    
        public T GoUpTillType<T>() where T : FortranObject
        {
            FortranObject obj = this;
            while (!(obj is T))
                obj = obj.parent;
            return obj as T;
        }

        public List<R> SubObjectsOfType<R>() 
            where R : FortranObject
        {
            IEnumerable<R> a = from obj in this.SubObjects
                   where obj is R
                   select (R)obj;
            return a.ToList<R>();
        }

        public List<FortranObject> SubObjectsNotOfType<R>()
            where R : FortranObject
        {
            IEnumerable<FortranObject> a = from obj in this.SubObjects
                               where !(obj is R)
                               select obj;
            return a.ToList();
        }
    }

    public class UnclosedBlockException : ApplicationException
    {
        public FortranBlock block;

        public UnclosedBlockException(FortranBlock block)
            : base()
        {
            this.block = block;
        }

        public override string ToString()
        {
            return "A " + this.block.GetType().Name + " has not been closed";
        }
    }
}
