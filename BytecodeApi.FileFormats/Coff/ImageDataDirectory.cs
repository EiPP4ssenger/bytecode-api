namespace BytecodeApi.FileFormats.Coff
{
	//FEATURE: Implement derived classes for specific data (e.g. import table, export table)
	/// <summary>
	/// Represents a data directory of a PE image file.
	/// </summary>
	public sealed class ImageDataDirectory
	{
		/// <summary>
		/// Gets the name of the data directory. This may not be a valid enum value of <see cref="ImageDataDirectoryName" />, if the image has more than 14 data directories.
		/// </summary>
		public ImageDataDirectoryName Name { get; private set; }
		/// <summary>
		/// Gets or sets the address of the first byte of a table or string that Windows uses.
		/// </summary>
		public uint VirtualAddress { get; set; }
		/// <summary>
		/// Gets or sets size of a table or string that Windows uses.
		/// </summary>
		public uint Size { get; set; }

		/// <summary>
		/// Initializes a new instance of the <see cref="ImageDataDirectory" /> class.
		/// </summary>
		public ImageDataDirectory(ImageDataDirectoryName name, uint virtualAddress, uint size)
		{
			Name = name;
			VirtualAddress = virtualAddress;
			Size = size;
		}

		/// <summary>
		/// Returns a <see cref="string" /> that represents this instance.
		/// </summary>
		/// <returns>
		/// A <see cref="string" /> that represents this instance.
		/// </returns>
		public override string ToString()
		{
			return "[" + (int)Name + ", " + Name + "]";
		}
	}
}