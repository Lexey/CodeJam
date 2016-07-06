using System;
using System.Collections.Generic;

using CodeJam.Collections;

using JetBrains.Annotations;

namespace CodeJam.Utility
{
    /// <summary>Represents a mask of characters and * and ? wildcards</summary>
    public class Mask
    {
        /// <summary>Mask symbols</summary>
        [PublicAPI]
        public IReadOnlyList<Symbol> Symbols { get; }

        /// <summary>Creates a mask from the enumeration of symbols</summary>
        /// <param name="symbols">The mask symbols</param>
        [PublicAPI]
        public Mask([NotNull]IEnumerable<Symbol> symbols)
        {
            Code.NotNull(symbols, nameof(symbols));
            Symbols = Optimize(new List<Symbol>(symbols));
        }

        /// <summary>Creates a mask from symbol parameters</summary>
        /// <param name="symbols">The mask symbols</param>
        [PublicAPI]
        public Mask(params Symbol[] symbols) : this((IEnumerable<Symbol>)(symbols ?? Array<Symbol>.Empty))
        {
        }

        [PublicAPI]
        public Mask([NotNull]string mask)
        {
            Code.NotNull(mask, nameof(mask));
            var symbols = new List<Symbol>();
            var escaped = false;
            foreach (var c in mask)
            {
                switch (c)
                {
                    case '\\':
                        if (escaped)
                        {
                            escaped = false;
                            symbols.Add(Symbol.Backslash);
                        }
                        else
                        {
                            escaped = true;
                        }
                        break;
                    case '?':
                        if (escaped)
                        {
                            escaped = false;
                            symbols.Add(Symbol.Question);
                        }
                        else
                        {
                            symbols.Add(Symbol.AnySingle);
                        }
                        break;
                    case '*':
                        if (escaped)
                        {
                            escaped = false;
                            symbols.Add(Symbol.Star);
                        }
                        else
                        {
                            symbols.Add(Symbol.AnySequence);
                        }
                        break;
                    default:
                        if (escaped)
                        {
                            // treat previous backslash as a normal char
                            escaped = false;
                            symbols.Add(Symbol.Backslash);
                        }
                        symbols.Add(new Symbol(c));
                        break;
                }
            }
            if (escaped)
            {
                // the last symbol was the unpaired backslash
                // treat it as a normal char
                symbols.Add(Symbol.Backslash);
            }
            Symbols = Optimize(symbols);
        }

        /// <summary>Shows whether the mask matches any string or not</summary>
        public bool MatchesAnyString => Symbols.Count == 1 && Symbols[0].Kind == Symbol.SymbolKind.AnySequence;

        /// <summary>Optimizes the mask if possible</summary>
        private static List<Symbol> Optimize(List<Symbol> symbols)
        {
            DebugCode.AssertArgument(symbols != null, nameof(symbols), $"{nameof(symbols)} cannot be null");
            // First, change all *? combinations to ?* (move ? before *)
            for (var i = 1; i < symbols.Count; ++i)
            {
                var s = symbols[i];
                if (!s.IsAnySingle)
                {
                    continue;
                }
                var j = i - 1;
                for (; j >= 0; --j)
                {
                    if (!symbols[j].IsAnySequence)
                    {
                        break;
                    }
                }
                ++j;
                if (j != i)
                {
                    symbols[j] = Symbol.AnySingle;
                    symbols[i] = Symbol.AnySequence;
                }
            }
            // Second, replace all **...* with a single *
            for (var i = symbols.Count - 1; i > 0;)
            {
                if (!symbols[i].IsAnySequence)
                {
                    --i;
                    continue;
                }
                var j = i - 1;
                while (j >= 0 && symbols[j].IsAnySequence)
                {
                    --j;
                }
                var symbolsToRemove = i - j - 1;
                if (symbolsToRemove > 0)
                {
                    symbols.RemoveRange(j + 2, symbolsToRemove);
                }
                i = j - 1;
            }
            return symbols;
        }

        /// <summary>Mask symbol</summary>
        public struct Symbol : IEquatable<Symbol>
        {
            #region Equality members
            /// <summary>Indicates whether the current object is equal to another object of the same type.</summary>
            /// <returns>true if the current object is equal to the <paramref name="other" /> parameter; otherwise, false.</returns>
            /// <param name="other">An object to compare with this object.</param>
            public bool Equals(Symbol other) => _c == other._c && Kind == other.Kind;

            /// <summary>Indicates whether this instance and a specified object are equal.</summary>
            /// <returns>true if <paramref name="obj" /> and this instance are the same type and represent the same value; otherwise, false. </returns>
            /// <param name="obj">The object to compare with the current instance. </param>
            public override bool Equals(object obj)
            {
                if (ReferenceEquals(null, obj)) return false;
                return obj is Symbol && Equals((Symbol)obj);
            }

            /// <summary>Returns the hash code for this instance.</summary>
            /// <returns>A 32-bit signed integer that is the hash code for this instance.</returns>
            public override int GetHashCode()
            {
                unchecked
                {
                    return (_c.GetHashCode() * 397) ^ (int)Kind;
                }
            }

            public static bool operator ==(Symbol left, Symbol right) => left.Equals(right);

            public static bool operator !=(Symbol left, Symbol right) => !left.Equals(right);
            #endregion

            /// <summary>AnySequence symbol instance</summary>
            public static readonly Symbol AnySequence = new Symbol(SymbolKind.AnySequence);
            /// <summary>Any symbol instance</summary>
            public static readonly Symbol AnySingle = new Symbol(SymbolKind.AnySingle);
            /// <summary>Backslash symbol instance</summary>
            public static readonly Symbol Backslash = new Symbol('\\');
            /// <summary>Question mark symbol</summary>
            public static readonly Symbol Question = new Symbol('?');
            /// <summary>Plain star symbol</summary>
            public static readonly Symbol Star = new Symbol('*');

            private readonly char _c;

            /// <summary>
            /// The character value
            /// <remarks>Valid only for the Char kind symbol</remarks>
            /// </summary>
            public char Char
            {
                get
                {
                    if (Kind == SymbolKind.Char)
                    {
                        return _c;
                    }
                    throw new InvalidOperationException("The wildcard symbol does not have a character representation");
                }
            }
            /// <summary>The symbol kind</summary>
            public SymbolKind Kind { get; }

            /// <summary>Shows whether the symbol is the any character wildcard</summary>
            public bool IsAnySingle => Kind == SymbolKind.AnySingle;
            /// <summary>Shows whether the symbol is the any sequence wildcard</summary>
            public bool IsAnySequence => Kind == SymbolKind.AnySequence;
            /// <summary>Shows whether the symbol is the normal character</summary>
            public bool IsChar => Kind == SymbolKind.Char;

            /// <summary>Creates a symbol from the given character</summary>
            /// <param name="char">The character</param>
            public Symbol(char @char)
            {
                _c = @char;
                Kind = SymbolKind.Char;
            }

            private Symbol(SymbolKind kind)
            {
                _c = default(char);
                Kind = kind;
            }

            /// <summary>Symbol kind</summary>
            public enum SymbolKind : byte
            {
                /// <summary>A normal character</summary>
                Char,
                /// <summary>Any number of characters</summary>
                AnySequence,
                /// <summary>Any single character</summary>
                AnySingle
            }
        }
    }
}
