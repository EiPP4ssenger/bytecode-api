﻿using BytecodeApi.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BytecodeApi.Text
{
	/// <summary>
	/// Class that performs string manipulation on <see cref="string" /> objects containing text.
	/// </summary>
	public static class Wording
	{
		/// <summary>
		/// Trims the specified <see cref="string" /> by the specified length. If the <see cref="string" /> is longer than the value of <paramref name="length" />, it will be truncated by a leading "..." to match the <paramref name="length" /> parameter, including the length of the "..." appendix (3 characters).
		/// </summary>
		/// <param name="str">The <see cref="string" /> to be trimmed.</param>
		/// <param name="length">A <see cref="int" /> value specifying the maximum length of the returned <see cref="string" />.</param>
		/// <returns>
		/// The original value of <paramref name="str" />, if the length of the <see cref="string" /> is less than or equal to the <paramref name="length" /> parameter;
		/// otherwise, the truncated <see cref="string" /> and a leading "..." that matches the <paramref name="length" /> parameter, including the length of the "..." appendix (3 characters).
		/// </returns>
		public static string TrimText(string str, int length)
		{
			return TrimText(str, length, "...");
		}
		/// <summary>
		/// Trims the specified <see cref="string" /> by the specified length. If the <see cref="string" /> is longer than the value of <paramref name="length" />, it will be truncated and the value of <paramref name="trailingText" /> will be appended to match the <paramref name="length" /> parameter, including the length of the <paramref name="trailingText" /> paramter.
		/// </summary>
		/// <param name="str">The <see cref="string" /> to be trimmed.</param>
		/// <param name="length">A <see cref="int" /> value specifying the maximum length of the returned <see cref="string" />.</param>
		/// <param name="trailingText">The trailing text.</param>
		/// <returns>
		/// The original value of <paramref name="str" />, if the length of the <see cref="string" /> is less than or equal to the <paramref name="length" /> parameter and the length of the <paramref name="trailingText" /> parameter;
		/// otherwise, the truncated <see cref="string" /> and the value of <paramref name="trailingText" /> that matches the <paramref name="length" /> parameter, including the length of the <paramref name="trailingText" /> paramter.
		/// </returns>
		public static string TrimText(string str, int length, string trailingText)
		{
			Check.ArgumentNull(str, nameof(str));
			Check.Argument(length >= trailingText.Length, nameof(length), "Length must be at least the length of the trailing text string.");
			Check.ArgumentNull(trailingText, nameof(trailingText));

			return str.Length > length ? str.Left(length - trailingText.Length) + trailingText : str;
		}
		/// <summary>
		/// Concatenates all values in the specified <see cref="string" /> collection, where <paramref name="lastSeparator" /> is used for the last separator.
		/// <para>Example with 1 value: A</para>
		/// <para>Example with 2 values: A and B</para>
		/// <para>Example with 3 values: A, B and C</para>
		/// <para>Example with 4 values: A, B, C and D</para>
		/// </summary>
		/// <param name="separator">A <see cref="string" /> value specifying the separator between each <see cref="string" /> value.</param>
		/// <param name="lastSeparator">A <see cref="string" /> value specifying the separator between the last two <see cref="string" /> values.</param>
		/// <param name="strings">The <see cref="string" />[] that is concatenated.</param>
		/// <returns>
		/// A <see cref="string" />, where the values of <paramref name="strings" /> is concatenated by the specified separators, or <see cref="string.Empty" />, if the collection is empty.
		/// </returns>
		public static string JoinStrings(string separator, string lastSeparator, params string[] strings)
		{
			Check.ArgumentNull(separator, nameof(separator));
			Check.ArgumentNull(lastSeparator, nameof(lastSeparator));

			return strings.IsNullOrEmpty() ? "" : strings.Take(strings.Length - 1).AsString(separator) + (strings.Length > 1 ? lastSeparator : "") + strings[strings.Length - 1];
		}
		/// <summary>
		/// Formats a <see cref="long" /> value representing a byte size in the <see cref="ByteSizeFormat.ByteSizeTwoDigits" /> format.
		/// <para>Example: 12345 is converted to "12,06 KB"</para>
		/// </summary>
		/// <param name="size">A <see cref="long" /> value representing a byte size.</param>
		/// <returns>
		/// An equivalent <see cref="string" /> value representing the byte size value in the <see cref="ByteSizeFormat.ByteSizeTwoDigits" /> format.
		/// </returns>
		public static string FormatByteSizeString(long size)
		{
			return FormatByteSizeString(size, 2);
		}
		/// <summary>
		/// Formats a <see cref="long" /> value representing a byte size with the specified number of decimal places.
		/// <para>Example: 12345 is converted to "12,06 KB" (given, that <paramref name="decimalPlaces" /> = 2)</para>
		/// </summary>
		/// <param name="size">A <see cref="long" /> value representing a byte size.</param>
		/// <param name="decimalPlaces">The number of decimal places to round the result to.</param>
		/// <returns>
		/// An equivalent <see cref="string" /> value representing the formatted byte size value.
		/// </returns>
		public static string FormatByteSizeString(long size, int decimalPlaces)
		{
			Check.ArgumentOutOfRangeEx.GreaterEqual0(size, nameof(size));
			Check.ArgumentOutOfRangeEx.GreaterEqual0(decimalPlaces, nameof(decimalPlaces));

			double calculatedSize = size;
			int unit = 0;

			while (calculatedSize >= 1024)
			{
				calculatedSize /= 1024;
				unit++;
			}

			if (unit == 0)
			{
				return ((long)calculatedSize).ToStringInvariant() + " bytes";
			}
			else
			{
				return Math
					.Round(calculatedSize, decimalPlaces)
					.ToStringInvariant("0." + '0'.Repeat(decimalPlaces))
					.Replace('.', ',') + " " + new[] { "KB", "MB", "GB", "TB", "PB", "EB" }[unit - 1];
			}
		}
		/// <summary>
		/// Formats a <see cref="long" /> value representing a byte size in the specified <see cref="ByteSizeFormat" /> and returns its equivalent <see cref="string" /> representation.
		/// </summary>
		/// <param name="size">A <see cref="long" /> value representing a byte size.</param>
		/// <param name="format">The <see cref="ByteSizeFormat" /> to use to format <paramref name="size" />.</param>
		/// <returns>
		/// An equivalent <see cref="string" /> value representing the byte size value in the specified <see cref="ByteSizeFormat" />.
		/// </returns>
		public static string FormatByteSizeString(long size, ByteSizeFormat format)
		{
			Check.ArgumentOutOfRangeEx.GreaterEqual0(size, nameof(size));

			switch (format)
			{
				case ByteSizeFormat.ByteSize:
					return FormatByteSizeString(size, 0);
				case ByteSizeFormat.ByteSizeTwoDigits:
					return FormatByteSizeString(size, 2);
				case ByteSizeFormat.Bytes:
					return FormatInt64(size) + " bytes";
				case ByteSizeFormat.KiloBytes:
					return FormatInt64((long)Math.Ceiling(size / 1024.0)) + " KB";
				case ByteSizeFormat.KiloBytesTwoDigits:
					return FormatDouble(Math.Round(size / 1024.0, 2)) + " KB";
				default:
					throw Throw.InvalidEnumArgument(nameof(format));
			}

			string FormatInt64(long number) => number.ToStringInvariant("#,0").Replace(',', '.');
			string FormatDouble(double number) => number.ToStringInvariant("#,0.##").Swap(',', '.');
		}
		/// <summary>
		/// Converts the value of the specified <see cref="TimeSpan" /> to a human readable <see cref="string" /> representation by displaying the two most significant elements of either days, hours, minutes or seconds that are greater than zero, separated by a comma.
		/// <para>Example: "12:00:03" is converted to "12h, 3s"</para>
		/// </summary>
		/// <param name="timeSpan">The <see cref="TimeSpan" /> value to convert.</param>
		/// <returns>
		/// The value of the specified <see cref="TimeSpan" /> as a human readable <see cref="string" /> representation.
		/// </returns>
		public static string FormatTimeSpanString(TimeSpan timeSpan)
		{
			return FormatTimeSpanString(timeSpan, 2);
		}
		/// <summary>
		/// Converts the value of the specified <see cref="TimeSpan" /> to a human readable <see cref="string" /> representation by displaying a specified number of most significant elements of either days, hours, minutes or seconds that are greater than zero, separated by a comma.
		/// <para>Example: "5.03:30:15" is converted to "5d, 3h", if <paramref name="maxElements" /> is 2 and "5d, 3h, 30m", if <paramref name="maxElements" /> is 3.</para>
		/// </summary>
		/// <param name="timeSpan">The <see cref="TimeSpan" /> value to convert.</param>
		/// <param name="maxElements">A <see cref="int" /> value specifying the number of elements of either days, hours, minutes or seconds to display.</param>
		/// <returns>
		/// The value of the specified <see cref="TimeSpan" /> as a human readable <see cref="string" /> representation.
		/// </returns>
		public static string FormatTimeSpanString(TimeSpan timeSpan, int maxElements)
		{
			return FormatTimeSpanString(timeSpan, maxElements, ", ");
		}
		/// <summary>
		/// Converts the value of the specified <see cref="TimeSpan" /> to a human readable <see cref="string" /> representation by displaying a specified number of most significant elements of either days, hours, minutes or seconds that are greater than zero, separated by the specified separator.
		/// <para>Example: "5.03:30:15" is converted to "5d, 3h", if <paramref name="maxElements" /> is 2 and "5d, 3h, 30m", if <paramref name="maxElements" /> is 3.</para>
		/// </summary>
		/// <param name="timeSpan">The <see cref="TimeSpan" /> value to convert.</param>
		/// <param name="maxElements">A <see cref="int" /> value specifying the number of elements of either days, hours, minutes or seconds to display.</param>
		/// <param name="separator">A <see cref="string" /> specifying the separator to use between each element, including whitespaces, if needed.</param>
		/// <returns>
		/// The value of the specified <see cref="TimeSpan" /> as a human readable <see cref="string" /> representation.
		/// </returns>
		public static string FormatTimeSpanString(TimeSpan timeSpan, int maxElements, string separator)
		{
			Check.ArgumentOutOfRangeEx.Greater0(maxElements, nameof(maxElements));
			Check.ArgumentNull(separator, nameof(separator));

			if (timeSpan == TimeSpan.Zero)
			{
				return "";
			}
			else
			{
				bool sign = timeSpan < TimeSpan.Zero;
				if (sign) timeSpan = -timeSpan;

				List<string> elements = new List<string>();
				if (timeSpan.Days > 0) elements.Add(timeSpan.Days + "d");
				if (timeSpan.Hours > 0) elements.Add(timeSpan.Hours + "h");
				if (timeSpan.Minutes > 0) elements.Add(timeSpan.Minutes + "m");
				if (timeSpan.Seconds > 0) elements.Add(timeSpan.Seconds + "s");

				return (sign ? "-" : null) + elements.Take(maxElements).AsString(separator);
			}
		}
		/// <summary>
		/// Wraps the specified text by splitting it up by whitespace characters and wrapping it by a specified maximum column width. This is typically used for console windows or in conjunction with monospace fonts.
		/// </summary>
		/// <param name="text">A <see cref="string" /> specifying the text to wrap.</param>
		/// <param name="width">A <see cref="int" /> specifying the maximum number of characters per line.</param>
		/// <param name="overflow"><see langword="true" /> to allow words that are longer than <paramref name="width" /> to overflow; <see langword="false" /> to split up long words.</param>
		/// <returns>
		/// A multiline <see cref="string" /> with the wrapped text. Each line does not exceed an amount of characters equal to <paramref name="width" />, unless <paramref name="overflow" /> is set t <see langword="true" />.
		/// </returns>
		public static string WrapText(string text, int width, bool overflow)
		{
			Check.ArgumentNull(text, nameof(text));
			Check.ArgumentOutOfRangeEx.Greater0(width, nameof(width));

			StringBuilder result = new StringBuilder();

			int index = 0;
			int column = 0;

			while (index < text.Length)
			{
				int spaceIndex = text.IndexOfAny(new[] { ' ', '\t', '\r', '\n' }, index);

				if (spaceIndex == -1)
				{
					break;
				}
				else if (spaceIndex == index)
				{
					index++;
				}
				else
				{
					AddWord(text.Substring(index, spaceIndex - index));
					index = spaceIndex + 1;
				}
			}

			if (index < text.Length) AddWord(text.Substring(index));

			void AddWord(string word)
			{
				if (!overflow && word.Length > width)
				{
					int wordIndex = 0;
					while (wordIndex < word.Length)
					{
						string subWord = word.Substring(wordIndex, Math.Min(width, word.Length - wordIndex));
						AddWord(subWord);
						wordIndex += subWord.Length;
					}
				}
				else
				{
					if (column + word.Length >= width)
					{
						if (column > 0)
						{
							result.AppendLine();
							column = 0;
						}
					}
					else if (column > 0)
					{
						result.Append(" ");
						column++;
					}

					result.Append(word);
					column += word.Length;
				}
			}

			return result.ToString();
		}
		/// <summary>
		/// Creates a binary view for the specified <see cref="byte" />[].
		/// <para>Example: 00000000h: 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 ; ................</para>
		/// </summary>
		/// <param name="bytes">The <see cref="byte" />[] to create the binary view from.</param>
		/// <returns>
		/// A new <see cref="string" /> representing the specified <see cref="byte" />[] as a binary view.
		/// </returns>
		public static string FormatBinary(byte[] bytes)
		{
			return FormatBinary(bytes, 0, bytes.Length);
		}
		/// <summary>
		/// Creates a binary view for the specified <see cref="byte" />[], starting from the specified offset, including the specified number of bytes.
		/// <para>Example: 00000000h: 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 ; ................</para>
		/// </summary>
		/// <param name="bytes">The <see cref="byte" />[] to create the binary view from.</param>
		/// <param name="offset">The starting point in the buffer at which to begin.</param>
		/// <param name="count">The number of bytes to take.</param>
		/// <returns>
		/// A new <see cref="string" /> representing the specified <see cref="byte" />[] as a binary view.
		/// </returns>
		public static string FormatBinary(byte[] bytes, int offset, int count)
		{
			Check.ArgumentNull(bytes, nameof(bytes));
			Check.ArgumentEx.OffsetAndLengthOutOfBounds(offset, count, bytes.Length);

			StringBuilder stringBuilder = new StringBuilder();

			for (int i = offset, position = 0; i < offset + count; i += 16, position += 16)
			{
				int length = Math.Min(bytes.Length - i, 16);
				string line1 = position.ToStringInvariant("x8") + "h: ";
				string line2 = null;

				for (int j = 0; j < length; j++)
				{
					line1 += bytes[i + j].ToStringInvariant("x2") + " ";
					line2 += bytes[i + j] >= 32 && bytes[i + j] <= 126 ? (char)bytes[i + j] : '.';
				}

				if (length < 16)
				{
					line1 += ' '.Repeat((16 - length) * 3);
					line2 += ' '.Repeat(16 - length);
				}

				stringBuilder.AppendLine(line1 + "; " + line2);
			}

			return stringBuilder.ToString();
		}
		/// <summary>
		/// Encodes a <see cref="string" /> to its equivalent ROT13 representation. This function either encodes a <see cref="string" /> or decodes an already encoded <see cref="string" />.
		/// <para>Example:"http://example.com/" is encoded to "uggc://rknzcyr.pbz/".</para>
		/// </summary>
		/// <param name="str">The <see cref="string" /> to be encoded or decoded.</param>
		/// <returns>
		/// The encoded or decoded representation of <paramref name="str" />.
		/// </returns>
		public static string Rot13(string str)
		{
			Check.ArgumentNull(str, nameof(str));

			char[] newString = str.ToCharArray();
			for (int i = 0; i < str.Length; i++)
			{
				char c = newString[i];
				if (c >= 'a' && c <= 'z') newString[i] = (char)(c + 13 > 'z' ? c - 13 : c + 13);
				else if (c >= 'A' && c <= 'Z') newString[i] = (char)(c + 13 > 'Z' ? c - 13 : c + 13);
			}

			return newString.AsString();
		}
		/// <summary>
		/// Escapes a SendKeys sequence. The characters +^%~(){}[] are encapsulated by braces. The returned <see cref="string" /> can be then passed to SendKeys.
		/// </summary>
		/// <param name="str">The <see cref="string" /> value to be escaped.</param>
		/// <returns>
		/// A <see cref="string" /> with the escaped representation of <paramref name="str" />.
		/// </returns>
		public static string EscapeSendKeys(string str)
		{
			Check.ArgumentNull(str, nameof(str));

			return str.Select(key => CSharp.EqualsAny(key, "+^%~(){}[]") ? "{" + key + "}" : key.ToString()).AsString();
		}
	}
}