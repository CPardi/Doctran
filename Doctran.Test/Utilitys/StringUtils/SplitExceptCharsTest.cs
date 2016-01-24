// <copyright file="SplitExceptCharsTest.cs" company="Christopher Pardi">
//     Copyright © 2015 Christopher Pardi
//     This Source Code Form is subject to the terms of the Mozilla Public
//     License, v. 2.0. If a copy of the MPL was not distributed with this
//     file, You can obtain one at http://mozilla.org/MPL/2.0/.
// </copyright>

namespace Doctran.Test.Utilitys.StringUtils
{
    using System;
    using System.Collections;
    using System.Linq;
    using Doctran.Utilitys;
    using NUnit.Framework;

    [TestFixture]
    public class SplitExceptCharsTest
    {
        public static IEnumerable Data
        {
            get
            {
                // Differing pairs
                yield return new TestCaseData("one,two", new[] { ',' }, new[] { new Tuple<char, char>('(', ')') }, new[] { "one", "two" })
                    .SetName("Single comma split, without brackets, differing pair");

                yield return new TestCaseData("one,two(two_a, two_b)", new[] { ',' }, new[] { new Tuple<char, char>('(', ')') }, new[] { "one", "two(two_a, two_b)" })
                    .SetName("Single comma split, with brackets, differing pair");

                yield return new TestCaseData("one,two(two_a, two_b),three", new[] { ',' }, new[] { new Tuple<char, char>('(', ')') }, new[] { "one", "two(two_a, two_b)", "three" })
                    .SetName("Double comma split, with brackets, differing pair");

                yield return new TestCaseData(@"one,two=(two_a, two_b).three", new[] { ',' }, new[] { new Tuple<char, char>('"', '"') }, new[] { "one", "two(two_a, two_b)", "three" })
                    .SetName("Double comma or full-stop split, with quotes, differing pair");

                // Matching Pairs
                yield return new TestCaseData(@"one,two", new[] { ',' }, new[] { new Tuple<char, char>('"', '"') }, new[] { "one", "two(two_a, two_b)" })
                    .SetName("Single comma split, without quotes, matching pair");

                yield return new TestCaseData(@"one,two=""two_a, two_b""", new[] { ',' }, new[] { new Tuple<char, char>('"', '"') }, new[] { "one", "two(two_a, two_b)" })
                    .SetName("Single comma split, with quotes, matching pair");

                yield return new TestCaseData(@"one,two=""two_a, two_b"",three", new[] { ',' }, new[] { new Tuple<char, char>('"', '"') }, new[] { "one", "two(two_a, two_b)", "three" })
                    .SetName("Double comma split, with quotes, matching pair");

                yield return new TestCaseData(@"one,two=""two_a, two_b"".three", new[] { ',', '.' }, new[] { new Tuple<char, char>('"', '"') }, new[] { "one", "two(two_a, two_b)", "three" })
                    .SetName("Double comma or full-stop split, with quotes, matching pair");

                // Two Matching Pairs
                yield return new TestCaseData(@"one,two", new[] { ',' }, new[] { new Tuple<char, char>('"', '"'), new Tuple<char, char>('\'', '\'') }, new[] { "one", "two" })
                    .SetName("Single comma split, without any pairs, matching pair");

                yield return new TestCaseData(@"one='one_a, one_b',two=""two_a, two_b""", new[] { ',' }, new[] { new Tuple<char, char>('"', '"'), new Tuple<char, char>('\'', '\'') }, new[] { "one='one_a, one_b'", @"two=""two_a, two_b""" })
                    .SetName("Single comma split, with two matching pairs, matching pair");

                yield return new TestCaseData(@"one='one_a, one_b',two=""two_a, two_b"",three", new[] { ',' }, new[] { new Tuple<char, char>('"', '"'), new Tuple<char, char>('\'', '\'') }, new[] { "one='one_a, one_b'", @"two=""two_a, two_b""", "three" })
                    .SetName("Double comma split, with two matching pairs, matching pair");

                yield return new TestCaseData(@"one='one_a, one_b',two=""two_a, two_b"".three", new[] { ',', '.' }, new[] { new Tuple<char, char>('"', '"'), new Tuple<char, char>('\'', '\'') }, new[] { "one='one_a, one_b'", @"two=""two_a, two_b""", "three" })
                    .SetName("Double comma or full-stop split, with two matching pairs, matching pair");

                // Two differing Pairs
                yield return new TestCaseData(@"one,two", new[] { ',' }, new[] { new Tuple<char, char>('(', ')'), new Tuple<char, char>('[', ']') }, new[] { "one", "two" })
                    .SetName("Single comma split, without any pairs, matching pair");

                yield return new TestCaseData(@"one(one_a, one_b),two[two_a, two_b]", new[] { ',' }, new[] { new Tuple<char, char>('(', ')'), new Tuple<char, char>('[', ']') }, new[] { "one(one_a, one_b)", @"two[two_a, two_b]" })
                    .SetName("Single comma split, with two matching pairs, matching pair");

                yield return new TestCaseData(@"one(one_a, one_b),two[two_a, two_b],three", new[] { ',' }, new[] { new Tuple<char, char>('(', ')'), new Tuple<char, char>('[', ']') }, new[] { "one(one_a, one_b)", @"two[two_a, two_b]", "three" })
                    .SetName("Double comma split, with two matching pairs, matching pair");

                yield return new TestCaseData(@"one(one_a, one_b),two[two_a, two_b].three", new[] { ',', '.' }, new[] { new Tuple<char, char>('(', ')'), new Tuple<char, char>('[', ']') }, new[] { "one(one_a, one_b)", @"two[two_a, two_b]", "three" })
                    .SetName("Double comma or full-stop split, with two matching pairs, matching pair");

                // Two mixed pairs
                yield return new TestCaseData(@"one,two", new[] { ',' }, new[] { new Tuple<char, char>('(', ')'), new Tuple<char, char>('\'', '\'') }, new[] { "one", "two" })
                    .SetName("Single comma split, without any pairs, matching pair");

                yield return new TestCaseData(@"one(one_a, one_b),two='two_a, two_b'", new[] { ',' }, new[] { new Tuple<char, char>('(', ')'), new Tuple<char, char>('\'', '\'') }, new[] { "one(one_a, one_b)", @"two='two_a, two_b'" })
                    .SetName("Single comma split, with two matching pairs, matching pair");

                yield return new TestCaseData(@"one(one_a, one_b),two='two_a, two_b',three", new[] { ',' }, new[] { new Tuple<char, char>('(', ')'), new Tuple<char, char>('\'', '\'') }, new[] { "one(one_a, one_b)", @"two='two_a, two_b'", "three" })
                    .SetName("Double comma split, with two matching pairs, matching pair");

                yield return new TestCaseData(@"one(one_a, one_b),two='two_a, two_b'.three", new[] { ',', '.' }, new[] { new Tuple<char, char>('(', ')'), new Tuple<char, char>('\'', '\'') }, new[] { "one(one_a, one_b)", @"two='two_a, two_b'", "three" })
                    .SetName("Double comma or full-stop split, with two matching pairs, matching pair");

                // Other tests
                yield return new TestCaseData(@"one="",sdf""", new[] { ',' }, new[] { new Tuple<char, char>('"', '"') }, new[] { @"one="",sdf""" })
                    .SetName("Delimiter at after first exception char.");

                yield return new TestCaseData(@"one=""sdf,""", new[] { ',' }, new[] { new Tuple<char, char>('"', '"') }, new[] { @"one=""sdf,""" })
                    .SetName("Delimiter at before last exception char.");
            }
        }

        [Test]
        [TestCaseSource(typeof(SplitExceptCharsTest), nameof(Data))]
        public void Test(string testString, char[] seperator, Tuple<char, char>[] pairs, string[] expected)
        {
            var actual = StringUtils.SplitExceptChars(testString, seperator, pairs);

            Assert.AreEqual(expected.Count(), actual.Count(), $"Actual:\n {actual.DelimiteredConcat(",\n")}");
            for (var i = 0; i < pairs.Length; i++)
            {
                Assert.AreEqual(actual[i], expected[i]);
            }
        }
    }
}