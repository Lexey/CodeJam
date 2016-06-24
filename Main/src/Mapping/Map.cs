﻿#if !FW35
using System;

using JetBrains.Annotations;

namespace CodeJam.Mapping
{
	/// <summary>
	/// Mapper helper class.
	/// </summary>
	/// <include file='Examples.xml' path='doc/Example[@name="MapTests"]/*'/>
	[PublicAPI]
	public static class Map
	{
		/// <summary>
		/// Returns a mapper to map an object of <i>TFrom</i> type to an object of <i>TTo</i> type.
		/// </summary>
		/// <typeparam name="TFrom">Type to map from.</typeparam>
		/// <typeparam name="TTo">Type to map to.</typeparam>
		/// <returns>Mapping expression.</returns>
		[Pure]
		public static Mapper<TFrom,TTo> GetMapper<TFrom,TTo>()
			=> new Mapper<TFrom,TTo>(new MapperBuilder<TFrom,TTo>());

		/// <summary>
		/// Returns a mapper to map an object of <i>TFrom</i> type to an object of <i>TTo</i> type.
		/// </summary>
		/// <typeparam name="TFrom">Type to map from.</typeparam>
		/// <typeparam name="TTo">Type to map to.</typeparam>
		/// <returns>Mapping expression.</returns>
		[Pure]
		public static Mapper<TFrom,TTo> GetMapper<TFrom,TTo>(
			[NotNull] Func<MapperBuilder<TFrom,TTo>,MapperBuilder<TFrom,TTo>> setter)
		{
			if (setter == null) throw new ArgumentNullException(nameof(setter));
			return new Mapper<TFrom, TTo>(setter(new MapperBuilder<TFrom, TTo>()));
		}
	}
}
#endif