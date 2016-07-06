using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NUnit.Framework;

namespace CodeJam.Utility
{
    [TestFixture]
    public class MaskTest
    {
        [Test]
        [TestCaseSource(nameof(TestData))]
        public void Test(string mask, Mask expected)
        {
            var m = new Mask(mask);
            Assert.That(m.Symbols, Is.EqualTo(expected.Symbols));
        }

        public static IEnumerable TestData()
        {
            yield return new object[]
            {
                "a\\b?c\\?d*e\\*f\\\\g\\",
                new Mask(
                    new Mask.Symbol('a'), Mask.Symbol.Backslash, new Mask.Symbol('b')
                    , Mask.Symbol.AnySingle, new Mask.Symbol('c'), Mask.Symbol.Question
                    , new Mask.Symbol('d'), Mask.Symbol.AnySequence, new Mask.Symbol('e')
                    , Mask.Symbol.Star, new Mask.Symbol('f'), Mask.Symbol.Backslash
                    , new Mask.Symbol('g'), Mask.Symbol.Backslash)
            };
            yield return new object[]
            {
                "*?", new Mask(Mask.Symbol.AnySingle, Mask.Symbol.AnySequence)
            };
            yield return new object[]
            {
                "**?*", new Mask(Mask.Symbol.AnySingle, Mask.Symbol.AnySequence)
            };
            yield return new object[]
            {
                "****a*?*?**?***b**?c?d*******e",
                new Mask(
                    Mask.Symbol.AnySequence, new Mask.Symbol('a'), Mask.Symbol.AnySingle
                    , Mask.Symbol.AnySingle, Mask.Symbol.AnySingle, Mask.Symbol.AnySequence
                    , new Mask.Symbol('b'), Mask.Symbol.AnySingle, Mask.Symbol.AnySequence
                    , new Mask.Symbol('c'), Mask.Symbol.AnySingle, new Mask.Symbol('d')
                    , Mask.Symbol.AnySequence, new Mask.Symbol('e'))
            };
        }
    }
}
