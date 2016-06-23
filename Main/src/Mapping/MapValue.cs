#if !FW35
using System;

using JetBrains.Annotations;

namespace CodeJam.Mapping
{
	/// <summary>
	/// Mapping value.
	/// </summary>
	public class MapValue
	{
		/// <summary>
		/// Creates <see cref="MapValue"/> instance.
		/// </summary>
		/// <param name="origValue">Original value.</param>
		/// <param name="mapValues">Mapping value.</param>
		public MapValue(object origValue, params MapValueAttribute[] mapValues)
		{
			Code.NotNullAndItemNotNull(mapValues, nameof(mapValues));

			OrigValue = origValue;
			MapValues = mapValues;
		}

		/// <summary>
		/// Original value.
		/// </summary>
		public object OrigValue { get; }

		/// <summary>
		/// Mapping value.
		/// </summary>
		[NotNull]
		[ItemNotNull]
		public MapValueAttribute[] MapValues { get; }
	}
}
#endif