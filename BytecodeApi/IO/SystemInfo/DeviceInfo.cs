﻿using BytecodeApi.Extensions;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace BytecodeApi.IO.SystemInfo
{
	/// <summary>
	/// Represents device information, retrieved by <see cref="DeviceManager" />.
	/// </summary>
	public class DeviceInfo
	{
		/// <summary>
		/// Gets the collection of attributes associated with this device.
		/// </summary>
		public ReadOnlyDictionary<string, object> Attributes { get; private set; }
		/// <summary>
		/// Gets the name associated with this device.
		/// </summary>
		public string Name { get; private set; }
		/// <summary>
		/// Gets the description associated with this device.
		/// </summary>
		public string Description { get; private set; }

		internal DeviceInfo(IDictionary<string, object> attributes)
		{
			Attributes = attributes.ToReadOnlyDictionary();
			Name = Attributes.ValueOrDefault("Name") as string;
			Description = Attributes.ValueOrDefault("Description") as string;
		}

		/// <summary>
		/// Returns a <see cref="string" /> that represents this instance.
		/// </summary>
		/// <returns>
		/// A <see cref="string" /> that represents this instance.
		/// </returns>
		public override string ToString()
		{
			return "[" + Name + ", " + Description + ", Attributes: " + Attributes.Count + "]";
		}
	}
}