using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

using CodeJam.PerfTests;
using CodeJam.Ranges;

using JetBrains.Annotations;

using NUnit.Framework;

using static CodeJam.AssemblyWideConfig;
using static CodeJam.PerfTests.CompetitionHelpers;

namespace CodeJam.RangesAlternatives
{
	/// <summary>
	/// Test to choose valid Range(of T) implementation.
	/// </summary>
	[TestFixture(Category = PerfTestCategory + ": Ranges")]
	[PublicAPI]
	[SuppressMessage("ReSharper", "PassStringInterpolation")]
	public class RangeAlternativesPerfTests
	{
		[Test]
		public void RunRangeAlternativesIntCase() =>
			Competition.Run<RangeAlternativesIntCase>(RunConfig);

		[PublicAPI]
		public class RangeAlternativesIntCase
		{
			private const int Count = DefaultCount;
			private readonly KeyValuePair<int, int>[] _data;
			private readonly RangeStub<int>[] _rangeData;
			private readonly RangeStub<int, string>[] _rangeKeyData;
			private readonly RangeStubCompact<int>[] _rangeCompactData;
			private readonly Range<int>[] _rangeDataImpl;
			private readonly Range<int, string>[] _rangeKeyDataImpl;

			public RangeAlternativesIntCase()
			{
				_data = new KeyValuePair<int, int>[Count];
				_rangeData = new RangeStub<int>[Count];
				_rangeKeyData = new RangeStub<int, string>[Count];
				_rangeCompactData = new RangeStubCompact<int>[Count];
				_rangeDataImpl = new Range<int>[Count];
				_rangeKeyDataImpl = new Range<int, string>[Count];

				for (var i = 0; i < _data.Length; i++)
				{
					_data[i] = new KeyValuePair<int, int>(i, i + 1);
					_rangeData[i] = new RangeStub<int>(i, i + 1);
					_rangeKeyData[i] = new RangeStub<int, string>(i, i + 1, i.ToString());
					_rangeCompactData[i] = new RangeStubCompact<int>(i, i + 1);
					_rangeDataImpl[i] = new Range<int>(i, i + 1);
					_rangeKeyDataImpl[i] = new Range<int, string>(i, i + 1, i.ToString());
				}
			}

			[CompetitionBaseline]
			public KeyValuePair<int, int> Test00DirectCompare()
			{
				var result = _data[0];
				for (var i = 1; i < _data.Length; i++)
				{
					result = new KeyValuePair<int, int>(
						result.Key < _data[i].Key ? result.Key : _data[i].Key,
						result.Value > _data[i].Value ? result.Value : _data[i].Value);
				}
				return result;
			}

			[CompetitionBenchmark(15.30, 18.78)]
			public RangeStub<int> Test01Range()
			{
				var result = _rangeData[0];
				for (var i = 1; i < _rangeData.Length; i++)
				{
					result = result.Union(_rangeData[i]);
				}
				return result;
			}

			[CompetitionBenchmark(16.06, 18.58)]
			public RangeStub<int, string> Test02KeyRange()
			{
				var result = _rangeKeyData[0];
				for (var i = 1; i < _rangeKeyData.Length; i++)
				{
					result = result.Union(_rangeKeyData[i]);
				}
				return result;
			}

			[CompetitionBenchmark(46.16, 52.12)]
			public RangeStubCompact<int> Test03CompactRange()
			{
				var result = _rangeCompactData[0];
				for (var i = 1; i < _rangeCompactData.Length; i++)
				{
					result = result.Union(_rangeCompactData[i]);
				}
				return result;
			}

			[CompetitionBenchmark(21.93, 24.79)]
			public Range<int> Test04RangeImpl()
			{
				var result = _rangeDataImpl[0];
				for (var i = 1; i < _rangeDataImpl.Length; i++)
				{
					result = result.Union(_rangeDataImpl[i]);
				}
				return result;
			}

			[CompetitionBenchmark(25.28, 28.06)]
			public Range<int, string> Test05RangeKeyImpl()
			{
				var result = _rangeKeyDataImpl[0];
				for (var i = 1; i < _rangeKeyDataImpl.Length; i++)
				{
					result = result.Union(_rangeKeyDataImpl[i]);
				}
				return result;
			}
		}

		[Test]
		public void RunRangeAlternativesNullableIntCase() =>
			Competition.Run<RangeAlternativesNullableIntCase>(RunConfig);

		[PublicAPI]
		public class RangeAlternativesNullableIntCase
		{
			private const int Count = DefaultCount;
			private readonly KeyValuePair<int?, int?>[] _data;
			private readonly RangeStub<int?>[] _rangeData;
			private readonly RangeStub<int?, string>[] _rangeKeyData;
			private readonly RangeStubCompact<int?>[] _rangeCompactData;
			private readonly Range<int?>[] _rangeDataImpl;
			private readonly Range<int?, string>[] _rangeKeyDataImpl;

			public RangeAlternativesNullableIntCase()
			{
				_data = new KeyValuePair<int?, int?>[Count];
				_rangeData = new RangeStub<int?>[Count];
				_rangeKeyData = new RangeStub<int?, string>[Count];
				_rangeCompactData = new RangeStubCompact<int?>[Count];
				_rangeDataImpl = new Range<int?>[Count];
				_rangeKeyDataImpl = new Range<int?, string>[Count];

				for (var i = 0; i < _data.Length; i++)
				{
					_data[i] = new KeyValuePair<int?, int?>(i, i + 1);
					_rangeData[i] = new RangeStub<int?>(i, i + 1);
					_rangeKeyData[i] = new RangeStub<int?, string>(i, i + 1, i.ToString());
					_rangeCompactData[i] = new RangeStubCompact<int?>(i, i + 1);
					_rangeDataImpl[i] = new Range<int?>(i, i + 1);
					_rangeKeyDataImpl[i] = new Range<int?, string>(i, i + 1, i.ToString());
				}
			}

			[CompetitionBaseline]
			public KeyValuePair<int?, int?> Test00DirectCompare()
			{
				var result = _data[0];
				for (var i = 1; i < _data.Length; i++)
				{
					result = new KeyValuePair<int?, int?>(
						result.Key < _data[i].Key ? result.Key : _data[i].Key,
						result.Value > _data[i].Value ? result.Value : _data[i].Value);
				}
				return result;
			}

			[CompetitionBenchmark(3.33, 3.66)]
			public RangeStub<int?> Test01Range()
			{
				var result = _rangeData[0];
				for (var i = 1; i < _rangeData.Length; i++)
				{
					result = result.Union(_rangeData[i]);
				}
				return result;
			}

			[CompetitionBenchmark(3.47, 5.99)]
			public RangeStub<int?, string> Test02KeyRange()
			{
				var result = _rangeKeyData[0];
				for (var i = 1; i < _rangeKeyData.Length; i++)
				{
					result = result.Union(_rangeKeyData[i]);
				}
				return result;
			}

			[CompetitionBenchmark(12.16, 12.91)]
			public RangeStubCompact<int?> Test03CompactRange()
			{
				var result = _rangeCompactData[0];
				for (var i = 1; i < _rangeCompactData.Length; i++)
				{
					result = result.Union(_rangeCompactData[i]);
				}
				return result;
			}

			[CompetitionBenchmark(5.49, 5.83)]
			public Range<int?> Test04RangeImpl()
			{
				var result = _rangeDataImpl[0];
				for (var i = 1; i < _rangeDataImpl.Length; i++)
				{
					result = result.Union(_rangeDataImpl[i]);
				}
				return result;
			}

			[CompetitionBenchmark(6.20, 6.80)]
			public Range<int?, string> Test05RangeKeyImpl()
			{
				var result = _rangeKeyDataImpl[0];
				for (var i = 1; i < _rangeKeyDataImpl.Length; i++)
				{
					result = result.Union(_rangeKeyDataImpl[i]);
				}
				return result;
			}
		}
	}
}