﻿using System.Collections;
using System.Collections.Generic;

namespace BytecodeApi.Comparers
{
	/// <summary>
	/// Represents a comparison operation that compares the <see cref="string.Length" /> property of <see cref="string" /> objects, where trailing and leading whitespaces are ignored.
	/// </summary>
	public sealed class StringLengthTrimmedComparer : IComparer, IComparer<string>
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="StringLengthTrimmedComparer" /> class.
		/// </summary>
		public StringLengthTrimmedComparer()
		{
		}

		/// <summary>
		/// Compares the <see cref="string.Length" /> property of two <see cref="string" /> objects, ignoring trailing and leading whitespaces, and returns an indication of their relative sort order. Specified <see cref="object" /> parameters that are not <see cref="string" /> objects are treated as <see langword="null" />.
		/// </summary>
		/// <param name="x">A <see cref="string" /> to compare its <see cref="string.Length" /> property to <paramref name="y" />.</param>
		/// <param name="y">A <see cref="string" /> to compare its <see cref="string.Length" /> property to <paramref name="x" />.</param>
		/// <returns>
		/// A signed integer that indicates the relative values of the <see cref="string.Length" /> property of <paramref name="x" /> and <paramref name="y" />.
		/// </returns>
		public int Compare(object x, object y)
		{
			return Compare(x as string, y as string);
		}
		/// <summary>
		/// Compares the <see cref="string.Length" /> property of two <see cref="string" /> objects, ignoring trailing and leading whitespaces, and returns an indication of their relative sort order.
		/// </summary>
		/// <param name="x">A <see cref="string" /> to compare its <see cref="string.Length" /> property to <paramref name="y" />.</param>
		/// <param name="y">A <see cref="string" /> to compare its <see cref="string.Length" /> property to <paramref name="x" />.</param>
		/// <returns>
		/// A signed integer that indicates the relative values of the <see cref="string.Length" /> property of <paramref name="x" /> and <paramref name="y" />.
		/// </returns>
		public int Compare(string x, string y)
		{
			if (x == null && y == null) return 0;
			else if (x == null) return -1;
			else if (y == null) return 1;
			else return x.Trim().Length.CompareTo(y.Trim().Length);
		}
	}
}