﻿using System;
using System.Collections;
using System.Collections.Generic;

namespace BytecodeApi.FileFormats.Csv
{
	/// <summary>
	/// Represents a collection of <see cref="CsvRow" /> objects.
	/// </summary>
	public sealed class CsvRowCollection : ICollection<CsvRow>
	{
		private readonly List<CsvRow> Rows;
		/// <summary>
		/// Gets the <see cref="CsvRow" /> at the specified index.
		/// </summary>
		/// <param name="index">The index at which to retrieve the <see cref="CsvRow" />.</param>
		public CsvRow this[int index]
		{
			get
			{
				Check.IndexOutOfRange(index, Count);
				return Rows[index];
			}
		}
		/// <summary>
		/// Gets the number of rows in this <see cref="CsvRowCollection" />.
		/// </summary>
		public int Count => Rows.Count;
		/// <summary>
		/// Gets a value indicating whether the <see cref="CsvRowCollection" /> is read-only.
		/// </summary>
		public bool IsReadOnly => false;

		/// <summary>
		/// Initializes a new instance of the <see cref="CsvRowCollection" /> class.
		/// </summary>
		public CsvRowCollection()
		{
			Rows = new List<CsvRow>();
		}

		/// <summary>
		/// Adds an <see cref="CsvRow" /> to the end of the <see cref="CsvRowCollection" />.
		/// </summary>
		/// <param name="item">The <see cref="CsvRow" /> to be added to the end of the <see cref="CsvRowCollection" />.</param>
		public void Add(CsvRow item)
		{
			Check.ArgumentNull(item, nameof(item));

			Rows.Add(item);
		}
		/// <summary>
		/// Removes the first occurrence of a specific <see cref="CsvRow" /> from the <see cref="CsvRowCollection" />.
		/// </summary>
		/// <param name="item">The <see cref="CsvRow" /> to remove from the <see cref="CsvRowCollection" />.</param>
		/// <returns>
		/// <see langword="true" />, if <paramref name="item" /> is successfully removed;
		/// otherwise, <see langword="false" />.
		/// This method also returns <see langword="false" />, if <paramref name="item" /> was not found in the <see cref="CsvRowCollection" />.</returns>
		public bool Remove(CsvRow item)
		{
			return Rows.Remove(item);
		}
		/// <summary>
		/// Removes all elements from the <see cref="CsvRowCollection" />.
		/// </summary>
		public void Clear()
		{
			Rows.Clear();
		}
		/// <summary>
		/// Determines whether an element is in the <see cref="CsvRowCollection" />.
		/// </summary>
		/// <param name="item">The <see cref="CsvRow" /> to locate in the <see cref="CsvRowCollection" />.</param>
		/// <returns>
		/// <see langword="true" />, if <paramref name="item" /> is found in the <see cref="CsvRowCollection" />;
		/// otherwise, <see langword="false" />.
		/// </returns>
		public bool Contains(CsvRow item)
		{
			return Rows.Contains(item);
		}
		/// <summary>
		/// Copies the elements of the <see cref="CsvRowCollection" /> to an <see cref="Array" />, starting at a particular <see cref="Array" /> index.
		/// </summary>
		/// <param name="array">The one-dimensional <see cref="Array" /> that is the destination of the elements copied from <see cref="CsvRowCollection" />.</param>
		/// <param name="arrayIndex">The zero-based index in <paramref name="array" /> at which copying begins.</param>
		public void CopyTo(CsvRow[] array, int arrayIndex)
		{
			Check.ArgumentNull(array, nameof(array));
			Check.IndexOutOfRange(arrayIndex, array.Length - Count + 1);

			Rows.CopyTo(array, arrayIndex);
		}

		/// <summary>
		/// Returns an enumerator that iterates through the collection.
		/// </summary>
		/// <returns>
		/// An enumerator that can be used to iterate through the collection.
		/// </returns>
		public IEnumerator<CsvRow> GetEnumerator()
		{
			return Rows.GetEnumerator();
		}
		IEnumerator IEnumerable.GetEnumerator()
		{
			return Rows.GetEnumerator();
		}
	}
}