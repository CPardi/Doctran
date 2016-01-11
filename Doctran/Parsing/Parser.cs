// <copyright file="Parser.cs" company="Christopher Pardi">
//     Copyright © 2015 Christopher Pardi
//     This Source Code Form is subject to the terms of the Mozilla Public
//     License, v. 2.0. If a copy of the MPL was not distributed with this
//     file, You can obtain one at http://mozilla.org/MPL/2.0/.
// </copyright>

namespace Doctran.Parsing
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Helper;
    using ParsingElements;
    using ParsingElements.FortranBlocks;
    using ParsingElements.FortranObjects;
    using Utilitys;

    public class Parser
    {
        private readonly Dictionary<string, IFortranBlock> _blockParsers = new Dictionary<string, IFortranBlock>();

        private readonly string _language;

        public Parser(string language, IEnumerable<IFortranBlock> blockParsers)
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

        public ISourceFile Parse(string sourceName, string source, Func<string, IEnumerable<FileLine>> preprocessor)
        {
            // Set the current index to 0 and parse the lines set in the constructor.
            var currentIndex = 0;
            var blockStack = new Stack<IFortranBlock>();
            blockStack.Push(_blockParsers["Source"]);

            // Store the snippet and insert a blank line at the start to simplify the parsing algorithm.
            var linesForParse = new List<FileLine>
            {
                new FileLine(0, string.Empty)
            };

            var lines = preprocessor(source).ToList();
            linesForParse.AddRange(lines);

            var parserForLines = new ParserForLines(linesForParse, _blockParsers, this.ErrorListener);
            var parsingResult = (Source)parserForLines.SearchBlock(0, ref currentIndex, blockStack).SingleOrDefault();

            if (parsingResult != null)
            {
                return new SourceFile(_language, sourceName, parsingResult?.SubObjects, source, parsingResult.Lines.Skip(1).ToList());
            }

            this.ErrorListener.Warning(new ParserException(1, 1, "File could not be parsed and will appear empty."));
            return new SourceFile(_language, sourceName, CollectionUtils.Empty<IContained>(), source, linesForParse.Skip(1).ToList());
        }

        private class ParserForLines
        {
            private readonly Dictionary<string, IFortranBlock> _blockParsers;

            private readonly IErrorListener<ParserException> _errorListener;

            private readonly List<FileLine> _lines;

            public ParserForLines(List<FileLine> lines, Dictionary<string, IFortranBlock> blockParsers, IErrorListener<ParserException> errorListener)
            {
                _lines = lines;
                _blockParsers = blockParsers;
                _errorListener = errorListener;
            }

            public IEnumerable<IContained> SearchBlock(int startIndex, ref int currentIndex, Stack<IFortranBlock> blockStack)
            {
                var currentFactory = blockStack.Peek();

                // Objects defined by this block of code.
                var blockObjects = new List<IContained>();

                // Objects defined within this block of code.
                var blockSubObjects = new List<IContained>();

                // If this block is a one liner then create the objects and exit.
                if (this.EndBlock(startIndex, currentIndex, blockStack, currentFactory, blockObjects, blockSubObjects))
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
                        this.AddSubBlocks(ref incrementIndex, ref currentIndex, currentFactory, blockStack, blockSubObjects);
                    }

                    // If this block is at the end then create it, add its sub objects and exit.
                    if (this.EndBlock(startIndex, currentIndex, blockStack, currentFactory, blockObjects, blockSubObjects))
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
                IFortranBlock currentFactory,
                Stack<IFortranBlock> blockStack,
                List<IContained> blockSubObjects)
            {
                foreach (var block in _blockParsers.Values)
                {
                    // If this isn't the start of a new block then check the next block parser.
                    if (!block.BlockStart(blockStack, _lines, currentIndex))
                    {
                        continue;
                    }

                    blockStack.Push(block);

                    // The start index of this block is the current index.
                    var startIndex = currentIndex;

                    // Get any blockParsers that maybe defined within the current block, these will be added later.
                    blockSubObjects.AddRange(this.SearchBlock(startIndex, ref currentIndex, blockStack));

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
                Stack<IFortranBlock> blockStack,
                IFortranBlock currentFactory,
                List<IContained> blockObjects,
                IEnumerable<IContained> blockSubObjects)
            {
                // End the block if we are at the end of the source.
                if (!this.ValidLineIndex(currentIndex, blockStack))
                {
                    return true;
                }

                var blockSubObjectsList = blockSubObjects as List<IContained> ?? blockSubObjects.ToList();

                // If block has not ended yet then return.
                if (!currentFactory.BlockEnd(blockStack.Skip(1), _lines, currentIndex))
                {
                    return false;
                }

                // At this point we assumn the block has ended and create the object represented by it.
                var blockLines = _lines.GetRange(startIndex, currentIndex - startIndex + 1);
                IEnumerable<IContained> parsingResult = null;
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

                blockStack.Pop();
                return true;
            }

            private bool ValidLineIndex(int currentIndex, Stack<IFortranBlock> blockStack)
            {
                if (currentIndex <= _lines.Count - 1)
                {
                    return true;
                }

                var lineNum = _lines.Last().Number;
                var stackString = blockStack.Select(block => block.Name).DelimiteredConcat("->");
                _errorListener.Error(new ParserException(lineNum, lineNum, $"Could not find block closure before end of file. Block stack before error '{stackString}'"));
                return false;
            }
        }
    }
}