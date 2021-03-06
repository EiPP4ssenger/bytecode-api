﻿using BytecodeApi.Extensions;
using System.IO;

namespace BytecodeApi.IO.FileSystem
{
	//FEATURE: CompareContents - compare file&directory names and/or file contents, recursive and non-recursive
	/// <summary>
	/// Provides constants and <see langword="static" /> methods that extend the <see cref="Directory" /> class.
	/// </summary>
	public static class DirectoryEx
	{
		/// <summary>
		/// Sends this directory and all of its contents to recycle bin.
		/// </summary>
		/// <param name="path">A <see cref="string" /> representing the path to a directory.</param>
		public static void SendToRecycleBin(string path)
		{
			Check.ArgumentNull(path, nameof(path));
			Check.DirectoryNotFound(path);

			new DirectoryInfo(path).SendToRecycleBin();
		}
		/// <summary>
		/// Gets the name of this directory with character casing according to the original and existing directory.
		/// </summary>
		/// <param name="path">A <see cref="string" /> representing the path to a directory.</param>
		/// <returns>
		/// A <see cref="string" /> that contains the actual name of this directory.
		/// </returns>
		public static string GetCaseSensitiveName(string path)
		{
			Check.ArgumentNull(path, nameof(path));
			Check.DirectoryNotFound(path);

			return new DirectoryInfo(path).GetCaseSensitiveName();
		}
		/// <summary>
		/// Gets the full path of this directory with character casing according to the original and existing directory.
		/// </summary>
		/// <param name="path">A <see cref="string" /> representing the path to a directory.</param>
		/// <returns>
		/// A <see cref="string" /> that contains the actual full path of this directory.
		/// </returns>
		public static string GetCaseSensitiveFullName(string path)
		{
			Check.ArgumentNull(path, nameof(path));
			Check.DirectoryNotFound(path);

			return new DirectoryInfo(path).GetCaseSensitiveFullName();
		}
		/// <summary>
		/// Gets the UNC path of this directory. If the path cannot be converted to a UNC path, the original path is returned.
		/// </summary>
		/// <param name="path">A <see cref="string" /> representing the path to a directory.</param>
		/// <returns>
		/// The UNC path of this directory, if conversion is possible;
		/// otherwise, the original path.
		/// </returns>
		public static string ToUncPath(string path)
		{
			Check.ArgumentNull(path, nameof(path));
			Check.DirectoryNotFound(path);

			return new DirectoryInfo(path).ToUncPath();
		}
		/// <summary>
		/// Computes the size of this directory including all of its subdirectories.
		/// </summary>
		/// <param name="path">A <see cref="string" /> representing the path to a directory.</param>
		/// <returns>
		/// The total size in bytes of this directory.
		/// </returns>
		public static long ComputeDirectorySize(string path)
		{
			Check.ArgumentNull(path, nameof(path));
			Check.DirectoryNotFound(path);

			return new DirectoryInfo(path).ComputeDirectorySize();
		}
		/// <summary>
		/// Copies this directory to a new location including all files and subdirectories recursively.
		/// </summary>
		/// <param name="path">A <see cref="string" /> representing the path to a directory.</param>
		/// <param name="destDirectoryName">The path of the new location including the top directory name.</param>
		public static void CopyTo(string path, string destDirectoryName)
		{
			CopyTo(path, destDirectoryName, false);
		}
		/// <summary>
		/// Copies this directory to a new location including all files and subdirectories recursively.
		/// </summary>
		/// <param name="path">A <see cref="string" /> representing the path to a directory.</param>
		/// <param name="destDirectoryName">The path of the new location including the top directory name.</param>
		/// <param name="overwrite"><see langword="true" /> to overwrite contents. Existing files in the destination directory that do not exist in the source directory are not deleted.</param>
		public static void CopyTo(string path, string destDirectoryName, bool overwrite)
		{
			Check.ArgumentNull(path, nameof(path));
			Check.ArgumentNull(destDirectoryName, nameof(destDirectoryName));

			new DirectoryInfo(path).CopyTo(destDirectoryName, overwrite);
		}
		/// <summary>
		/// Deletes all files and directories from this directory, if it exists.
		/// </summary>
		/// <param name="path">A <see cref="string" /> representing the path to a directory.</param>
		public static void DeleteContents(string path)
		{
			DeleteContents(path, false);
		}
		/// <summary>
		/// Deletes all files and directories from this directory, if it exists. If <paramref name="create" /> is set to <see langword="true" />, an empty directory will be created.
		/// </summary>
		/// <param name="path">A <see cref="string" /> representing the path to a directory.</param>
		/// <param name="create"><see langword="true" /> to create the directory, if it does not exist.</param>
		public static void DeleteContents(string path, bool create)
		{
			Check.ArgumentNull(path, nameof(path));

			new DirectoryInfo(path).DeleteContents(create);
		}
		/// <summary>
		/// Shows the properties dialog for the directory.
		/// </summary>
		/// <param name="path">A <see cref="string" /> representing the path to a directory.</param>
		public static void ShowPropertiesDialog(string path)
		{
			Check.ArgumentNull(path, nameof(path));
			Check.DirectoryNotFound(path);

			new DirectoryInfo(path).ShowPropertiesDialog();
		}
	}
}