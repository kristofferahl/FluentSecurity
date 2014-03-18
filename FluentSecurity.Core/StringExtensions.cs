using System;

namespace FluentSecurity
{
	public static class StringExtensions
	{
		/// <summary>
		/// Returns true if the value is null or empty
		/// </summary>
		/// <param name="value">The value</param>
		/// <returns>A boolean</returns>
		public static bool IsNullOrEmpty(this string value)
		{
			return String.IsNullOrEmpty(value);
		}

		/// <summary>
		/// Returns a formatted string
		/// </summary>
		/// <param name="format">The format</param>
		/// <param name="values">The values</param>
		/// <returns>A formatted string</returns>
		public static string FormatWith(this string format, params object[] values)
		{
			return String.Format(format, values);
		}
	}
}