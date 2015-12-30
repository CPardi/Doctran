﻿// <copyright file="Parser.cs" company="Christopher Pardi">
//     Copyright © 2015 Christopher Pardi
//     This Source Code Form is subject to the terms of the Mozilla Public
//     License, v. 2.0. If a copy of the MPL was not distributed with this
//     file, You can obtain one at http://mozilla.org/MPL/2.0/.
// </copyright>

namespace Doctran.Parsing
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using BuiltIn.FortranBlocks;
    using BuiltIn.FortranObjects;
    using Helper;

    public class Parser
    {
        private readonly Dictionary<string, FortranBlock> _blockParsers = new Dictionary<string, FortranBlock>();

        private readonly string _language;

        public Parser(string language, IEnumerable<FortranBlock> blockParsers)
        {
            _language = language;

            // Add the text factory for the passed text.
            var textFactory = new SourceBlock(_language);
            _blockParsers.Add(textFactory.Name, textFactory);

            foreach (var bp in blockParsers)
            {
                _blockParsers.Add(bp.Name, bp);
            }
        }

        public IErrorListener<ParserException> ErrorListener { get; set; } = new StandardErrorListener<ParserException>();

        public ISourceFile Parse(string sourceName, List<FileLine> lines)
        {
            // Set the current index to 0 and parse the lines set in the constructor.
            var currentIndex = 0;
            var blockNameStack = new Stack<FortranBlock>();
            blockNameStack.Push(_blockParsers["Source"]);

            // Store the snippet and insert a blank line at the start to simplify the parsing algorithm.
            var linesForParse = new List<FileLine>
            {
                new FileLine(0, string.Empty)
            };

            linesForParse.AddRange(lines);

            var parserForLines = new ParserForLines(linesForParse, _blockParsers, this.ErrorListener);
            var parsingResult = parserForLines.SearchBlock(0, ref currentIndex, blockNameStack).Single();
            return new SourceFile(_language, sourceName, parsingResult.SubObjects, lines, parsingResult.Lines.Skip(1).ToList());
        }

        private class ParserForLines
        {
            private readonly Dictionary<string, FortranBlock> _blockParsers;

            private readonly IErrorListener<ParserException> _errorListener;

            private readonly List<FileLine> _lines;

            public ParserForLines(List<FileLine> lines, Dictionary<string, FortranBlock> blockParsers, IErrorListener<ParserException> errorListener)
            {
                _lines = lines;
                _blockParsers = blockParsers;
                _errorListener = errorListener;
            }

            public IEnumerable<IFortranObject> SearchBlock(int startIndex, ref int currentIndex, Stack<FortranBlock> blockNameStack)
            {
                var currentFactory = blockNameStack.Peek();

                // Objects defined by this block of code.
                var blockObjects = new List<IFortranObject>();

                // Objects defined within this block of code.
                var blockSubObjects = new List<IFortranObject>();

                // If this block is a one liner then create the objects and exit.
                if (EndBlock(startIndex, currentIndex, blockNameStack, currentFactory, blockObjects, blockSubObjects))
                {
                    return blockObjects;
                }

                // The first line of this block has already been analysed, in the previous recursion level, so move to the next.
                currentIndex++;

                // Iterate over lines until this block is terminated.
                do
                {
                    // The line index is incremented by default. See AddSubBlocks for description of when this is false.
                    var incrementIndex = true;

                    // If this block of code contains sub-blockParsers, then search for them.
                    if (currentFactory.CheckInternal)
                    {
                        // Any internal blockParsers starting at this line will be added to the blockSubObjects list.
                        // The lineIndex and incrementIndex flag are set as follows:
                        // * Block has an explicit end - (LineIndex) the line after the last line of the last subblock. (incrementIndex) is set to false because AddSubBlocks has already incremented the index.
                        // * Block does not have an explicit end - (LineIndex) the last line of the last subblock. (incrementIndex) is set to true.
                        // This is to ensure that blockParsers without an explicit ending can be closed by the subsequent code.
                        AddSubBlocks(ref incrementIndex, ref currentIndex, currentFactory, blockNameStack, blockSubObjects);
                    }

                    // If this block is at the end then create it, add its sub objects and exit.
                    if (EndBlock(startIndex, currentIndex, blockNameStack, currentFactory, blockObjects, blockSubObjects))
                    {
                        break;
                    }

                    // Go to the next line if required.
                    if (incrementIndex)
                    {
                        currentIndex++;
                    }
                }
                while (true);

                return blockObjects;
            }

            private void AddSubBlocks(
                ref bool incrementIndex,
                ref int currentIndex,
                FortranBlock currentFactory,
                Stack<FortranBlock> blockNameStack,
                List<IFortranObject> blockSubObjects)
            {
                foreach (var block in _blockParsers.Values)
                {
                    // If this isn't the start of a new block then check the next block parser.
                    if (!block.BlockStart(blockNameStack, _lines, currentIndex))
                    {
                        continue;
                    }

                    blockNameStack.Push(block);

                    // The start index of this block is the current index.
                    var startIndex = currentIndex;

                    // Get any blockParsers that maybe defined within the current block, these will be added later.
                    blockSubObjects.AddRange(SearchBlock(startIndex, ref currentIndex, blockNameStack));

                    // If the block does not have an explicit end, then stay on the same line to check for a block end and go to next line later.
                    incrementIndex = !currentFactory.ExplicitEnd;

                    // If the block has a unique end, then increment the index here so that the same line isn't checked again below.
                    if (currentFactory.ExplicitEnd)
                    {
                        currentIndex++;
                    }

                    return;
                }
            }

            private bool EndBlock(
                int startIndex,
                int currentIndex,
                Stack<FortranBlock> blockNameStack,
                FortranBlock currentFactory,
                List<IFortranObject> blockObjects, 
                IEnumerable<IFortranObject> blockSubObjects)
            {
                var blockSubObjectsList = blockSubObjects as List<IFortranObject> ?? blockSubObjects.ToList();

                // If block has not ended yet then return.
                if (!currentFactory.BlockEnd(blockNameStack.Skip(1), _lines, currentIndex))
                {
                    return false;
                }

                // At this point we assumn the block has ended and create the object represented by it.
                var blockLines = _lines.GetRange(startIndex, currentIndex - startIndex + 1);
                IEnumerable<FortranObject> parsingResult = null;
                try
                {
                    parsingResult = currentFactory.ReturnObject(blockSubObjectsList, blockLines);
                }
                catch (BlockParserException e)
                {
                    _errorListener.Error(new ParserException(blockLines.First().Number, blockLines.Last().Number, e.Message));
                }

                // If we have created a valid block then add it to the list and return.
                if (parsingResult != null)
                {
                    blockObjects.AddRange(parsingResult);
                }

                blockNameStack.Pop();
                return true;
            }
        }
    }
}