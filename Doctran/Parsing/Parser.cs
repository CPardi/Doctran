//  Copyright © 2015 Christopher Pardi
//  This Source Code Form is subject to the terms of the Mozilla Public
//  License, v. 2.0. If a copy of the MPL was not distributed with this
//  file, You can obtain one at http://mozilla.org/MPL/2.0/.

namespace Doctran.Parsing
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using FortranObjects;
    using Helper;
    using Output;
    using Reporting;
    using Utilitys;

    /// <summary>
    /// This class parses a code file.
    /// </summary>
    public class Parser
    {
        private Dictionary<string, FortranBlock> _blocks = new Dictionary<string,FortranBlock>();

        /// <summary>
        /// Creates an parser instance to extract the blocks from a code file.
        /// </summary>
        /// <param name="blocks">An array of blocks classes that defines the coding language.</param>
        public Parser(IEnumerable<FortranBlock> blocks)
        {
            foreach (var b in blocks)
                _blocks.Add(b.Name, b);
        }

        /// <summary>
        /// Parses a code file.
        /// </summary>
        /// <param name="project">The project that this file belongs to.</param>
        /// <param name="pathAndFileName">The full path to the code file.</param>
        /// <param name="lines">The lines of the code file to be parsed.</param>
        /// <returns></returns>
        public File ParseFile(string pathAndFileName, List<FileLine> lines, IEnumerable<ObjectGroup> objectGroups)
        {
            if (EnvVar.Verbose >= 3) Console.WriteLine("Analyzing: " + pathAndFileName);
            int currentIndex = 0;
            var file = SearchBlock(pathAndFileName, File.PreProcessFile(pathAndFileName, lines), true, 0, ref currentIndex, "File", "Project", objectGroups).Single() as File;
            file.OriginalLines = lines;
            return file;
        }

        private List<FortranObject> SearchBlock(string pathAndFileName, List<FileLine> lines, bool checkInternal, int startIndex, ref int currentIndex, string blockName, string parentBlockName, IEnumerable<ObjectGroup> objectGroups)
        {
            // Objects defined by this block of code.
            List<FortranObject> blockObjects = new List<FortranObject>();

            // Objects defined within this block of code.
            List<FortranObject> blockSubObjects = new List<FortranObject>();

            // If this block is a one liner then create the objects and exit.
            if (blockName != "File" && _blocks[blockName].BlockEnd(parentBlockName, lines, currentIndex))
            {
                blockObjects.AddRange(CreateObject(lines, currentIndex, startIndex, blockName, blockSubObjects));
                return blockObjects;
            }

            // If we not a new file, then the first line of this block has already been analysed, in the previous recusion level, so move to the next.
            if (blockName != "File") currentIndex++;

            // Iterate over lines until this block is terminated.
            do
            {
                var incrementIndex = true;

                // If this block of code contains sub-blocks, then search for them.
                if (checkInternal)
                {
                    // Does a block start on the current line.
                    SearchResults resultStart = IsBlockStart(lines, blockName, ref currentIndex);

                    // Search within this block for any sub-blocks.
                    if (resultStart.Found)
                    {
                        //Console.WriteLine(lines[result_start.Index].Number + "    begin " + result_start.BlockName);

                        // Get any blocks that maybe defined within the current block, these will be added later.
                        blockSubObjects.AddRange(SearchBlock(pathAndFileName, lines, _blocks[resultStart.BlockName].CheckInternal, resultStart.Index, ref currentIndex, resultStart.BlockName, blockName, objectGroups));

                        // If the block has a unique end, then increment the index here so that the same line isn't checked again below.
                        incrementIndex = blockName != "File" && !_blocks[blockName].ExplicitEnd;
                        if (!incrementIndex) currentIndex++;
                    }
                }

                // If this block is at the end then create it, add its sub objects and exit.
                if (blockName != "File" && _blocks[blockName].BlockEnd(parentBlockName, lines, currentIndex))
                {
                    blockObjects.AddRange(CreateObject(lines, currentIndex, startIndex, blockName, blockSubObjects));
                    break;
                }
                
                // Go to the next line if required.
                if (incrementIndex) currentIndex++;

                // If we are at the end of a file then create and return it.
                if (blockName == "File" && !(currentIndex < lines.Count))
                {
                    blockObjects.Add(CreateFile(lines, pathAndFileName, blockSubObjects, objectGroups));
                    return blockObjects;
                }

            } while(true);

            if (!blockObjects.Any())
            {
                Report.Error((pub, ex) =>
                {
                    pub.AddErrorDescription("Error parsing source file.");
                    pub.AddReason("The " + blockName.ToLower() + " beggining at line " + lines[startIndex].Number + " has not been closed");
                    pub.AddLocation(pathAndFileName);
                }, new Exception("Parser Exception."));
            }

            return blockObjects;
        }

        /// <summary>
        /// Checks the current line to see if it defines the start of a new block of code.
        /// </summary>
        /// <param name="lines">All lines within the code file.</param>
        /// <param name="parentBlockName">The name of the current block, ie the parent of the potential new block.</param>
        /// <param name="currentIndex">The current code line index.</param>
        /// <returns></returns>
        private SearchResults IsBlockStart(List<FileLine> lines, string parentBlockName, ref int currentIndex)
        {
            foreach (var block in _blocks.Values)
                if (block.BlockStart(parentBlockName, lines, currentIndex))
                    return new SearchResults(currentIndex, block.Name);
            return new SearchResults();
        }

        /// <summary>
        /// This should be called after the end of the current code block has been detected. The objects defined by the current code are returned.
        /// </summary>
        /// <param name="lines">All lines within the code file.</param>
        /// <param name="currentIndex">The current code line index.</param>
        /// <param name="startIndex">The code line index that the block began.</param>
        /// <param name="blockName">The current block's name.</param>
        /// <param name="blockSubObjects">The sub-objects defined by the code blocks contained within this code block.</param>
        /// <returns>The objects defined by the current code.</returns>
        private IEnumerable<FortranObject> CreateObject(List<FileLine> lines, int currentIndex, int startIndex, string blockName, IEnumerable<FortranObject> blockSubObjects)
        {
            //Console.WriteLine(lines[current_index].Number + "    end " + block_name + "    ");
            var thisBlock = _blocks[blockName].ReturnObject(blockSubObjects, lines.GetRange(startIndex, currentIndex - startIndex + 1)).ToList();
            thisBlock.ForEach(b => b.SubObjects.ForEach(sObj => sObj.parent = b));
            return thisBlock;
        }

        /// <summary>
        /// This should be called after all lines have been read. A file object is returned.
        /// </summary>
        /// <param name="lines">All lines within the code file.</param>
        /// <param name="pathAndFileName">The full path to the code file.</param>
        /// <param name="blockSubObjects">The objects defined by th code blocks found within the file's lines of code.</param>
        /// <returns>A file with its parsed contents.</returns>
        private File CreateFile(List<FileLine> lines, string pathAndFileName, IEnumerable<FortranObject> blockSubObjects, IEnumerable<ObjectGroup> objectGroups)
        {
            return new File(pathAndFileName, blockSubObjects, lines, objectGroups);
        }

    }

    /// <summary>
    /// A simple container for the results of analysing a line of code for starts of code blocks.
    /// </summary>
    internal class SearchResults
    {
        public bool Found { get; private set; }
        public int Index { get; private set; }
        public string BlockName { get; private set; }

        public SearchResults()
        {
            this.Found = false;
        }

        public SearchResults(int index, string blockName)
        {
            this.Found = true;
            this.Index = index;
            this.BlockName = blockName;
        }
    }
}