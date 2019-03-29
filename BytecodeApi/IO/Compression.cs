﻿using BytecodeApi.Extensions;
using System.IO;
using System.IO.Compression;

namespace BytecodeApi.IO
{
	/// <summary>
	/// Class to compress and decompress data using GZip.
	/// </summary>
	public static class Compression
	{
		/// <summary>
		/// Compresses the specified <see cref="byte" />[] using GZip and returns a new <see cref="byte" />[] that can be used with the <see cref="Decompress" /> method.
		/// </summary>
		/// <param name="data">A <see cref="byte" />[] that represents data to be compressed.</param>
		/// <returns>
		/// A new <see cref="byte" />[] that can be used with the <see cref="Decompress" /> method.
		/// </returns>
		public static byte[] Compress(byte[] data)
		{
			Check.ArgumentNull(data, nameof(data));

			using (MemoryStream memoryStream = new MemoryStream())
			{
				using (GZipStream gzipStream = new GZipStream(memoryStream, CompressionMode.Compress, true))
				{
					gzipStream.Write(data);
				}

				return memoryStream.ToArray();
			}
		}
		/// <summary>
		/// Decompresses the specified <see cref="byte" />[] using GZip and returns a new <see cref="byte" />[] that represents the uncompressed data.
		/// </summary>
		/// <param name="data">A <see cref="byte" />[] that represents data compressed by the <see cref="Compress" /> method.</param>
		/// <returns>
		/// A new <see cref="byte" />[] that represents the uncompressed data.
		/// </returns>
		public static byte[] Decompress(byte[] data)
		{
			Check.ArgumentNull(data, nameof(data));

			using (MemoryStream memoryStream = new MemoryStream(data))
			using (MemoryStream decompressedStream = new MemoryStream())
			{
				using (GZipStream gzipStream = new GZipStream(memoryStream, CompressionMode.Decompress))
				{
					gzipStream.CopyTo(decompressedStream);
					return decompressedStream.ToArray();
				}
			}
		}
	}
}