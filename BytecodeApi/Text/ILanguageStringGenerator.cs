﻿namespace BytecodeApi.Text
{
	/// <summary>
	/// Defines base methods that are implemented in <see cref="WordGenerator" />, <see cref="SentenceGenerator" /> and <see cref="TextGenerator" />.
	/// </summary>
	public interface ILanguageStringGenerator
	{
		/// <summary>
		/// Generates a string in word generation classes, such as <see cref="WordGenerator" />, <see cref="SentenceGenerator" /> or <see cref="TextGenerator" />.
		/// </summary>
		/// <returns>
		/// The <see cref="string" /> generated by this instance.
		/// </returns>
		string Generate();
	}
}