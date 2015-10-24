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
            int current_index = 0;
            var file = SearchBlock(pathAndFileName, File.PreProcessFile(pathAndFileName, lines), true, 0, ref current_index, "File", "Project", objectGroups).Single() as File;
            file.OriginalLines = lines;
            return file;
        }

        private List<FortranObject> SearchBlock(string pathAndFileName, List<FileLine> lines, bool check_internal, int start_index, ref int current_index, string block_name, string parent_block_name, IEnumerable<ObjectGroup> objectGroups)
        {
            // Objects defined by this block of code.
            List<FortranObject> blockObjects = new List<FortranObject>();

            // Objects defined within this block of code.
            List<FortranObject> blockSubObjects = new List<FortranObject>();

            // If this block is a one liner then create the objects and exit.
            if (block_name != "File" && _blocks[block_name].BlockEnd(parent_block_name, lines, current_index))
            {
                blockObjects.AddRange(CreateObject(lines, current_index, start_index, block_name, blockSubObjects));
                return blockObjects;
            }

            // If we not a new file, then the first line of this block has already been analysed, in the previous recusion level, so move to the next.
            if (block_name != "File") current_index++;

            // Iterate over lines until this block is terminated.
            do
            {
                bool incrementIndex = true;

                // If this block of code contains sub-blocks, then search for them.
                if (check_internal)
                {
                    // Does a block start on the current line.
                    SearchResults result_start = IsBlockStart(lines, block_name, ref current_index);

                    // Search within this block for any sub-blocks.
                    if (result_start.Found)
                    {
                        //Console.WriteLine(lines[result_start.Index].Number + "    begin " + result_start.BlockName);

                        // Get any blocks that maybe defined within the current block, these will be added later.
                        blockSubObjects.AddRange(SearchBlock(pathAndFileName, lines, _blocks[result_start.BlockName].CheckInternal, result_start.Index, ref current_index, result_start.BlockName, block_name, objectGroups));

                        // If the block has a unique end, then increment the index here so that the same line isn't checked again below.
                        incrementIndex = block_name != "File" && !_blocks[block_name].ExplicitEnd;
                        if (!incrementIndex) current_index++;
                    }
                }

                // If this block is at the end then create it, add its sub objects and exit.
                if (block_name != "File" && _blocks[block_name].BlockEnd(parent_block_name, lines, current_index))
                {
                    blockObjects.AddRange(CreateObject(lines, current_index, start_index, block_name, blockSubObjects));
                    break;
                }
                
                // Go to the next line if required.
                if (incrementIndex) current_index++;

                // If we are at the end of a file then create and return it.
                if (block_name == "File" && !(current_index < lines.Count))
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
                    pub.AddReason("The " + block_name.ToLower() + " beggining at line " + lines[start_index].Number + " has not been closed");
                    pub.AddLocation(pathAndFileName);
                }, new Exception("Parser Exception."));
            }

            return blockObjects;
        }

        /// <summary>
        /// Checks the current line to see if it defines the start of a new block of code.
        /// </summary>
        /// <param name="lines">All lines within the code file.</param>
        /// <param name="parent_block_name">The name of the current block, ie the parent of the potential new block.</param>
        /// <param name="current_index">The current code line index.</param>
        /// <returns></returns>
        private SearchResults IsBlockStart(List<FileLine> lines, string parent_block_name, ref int current_index)
        {
            foreach (var block in _blocks.Values)
                if (block.BlockStart(parent_block_name, lines, current_index))
                    return new SearchResults(current_index, block.Name);
            return new SearchResults();
        }

        /// <summary>
        /// This should be called after the end of the current code block has been detected. The objects defined by the current code are returned.
        /// </summary>
        /// <param name="lines">All lines within the code file.</param>
        /// <param name="current_index">The current code line index.</param>
        /// <param name="start_index">The code line index that the block began.</param>
        /// <param name="block_name">The current block's name.</param>
        /// <param name="block_sub_objects">The sub-objects defined by the code blocks contained within this code block.</param>
        /// <returns>The objects defined by the current code.</returns>
        private List<FortranObject> CreateObject(List<FileLine> lines, int current_index, int start_index, string block_name, List<FortranObject> block_sub_objects)
        {
            //Console.WriteLine(lines[current_index].Number + "    end " + block_name + "    ");
            var this_block = _blocks[block_name].ReturnObject(block_sub_objects, lines.GetRange(start_index, current_index - start_index + 1));
            this_block.ForEach(b => b.SubObjects.ForEach(sObj => sObj.parent = b));
            return this_block;
        }

        /// <summary>
        /// This should be called after all lines have been read. A file object is returned.
        /// </summary>
        /// <param name="lines">All lines within the code file.</param>
        /// <param name="pathAndFileName">The full path to the code file.</param>
        /// <param name="block_sub_objects">The objects defined by th code blocks found within the file's lines of code.</param>
        /// <returns>A file with its parsed contents.</returns>
        private File CreateFile(List<FileLine> lines, string pathAndFileName, List<FortranObject> block_sub_objects, IEnumerable<ObjectGroup> objectGroups)
        {
            return new File(pathAndFileName, block_sub_objects, lines, objectGroups);
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

        public SearchResults(int index, string block_name)
        {
            this.Found = true;
            this.Index = index;
            this.BlockName = block_name;
        }
    }
}