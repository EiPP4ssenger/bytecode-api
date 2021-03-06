﻿using System;
using System.Threading;
using System.Windows;
using System.Windows.Interop;

namespace BytecodeApi.UI
{
	/// <summary>
	/// Class for managing single instance UI applications. A second instance can detect an already running instance and optionally trigger the first instance to be notified.
	/// </summary>
	public class SingleInstanceManager : IDisposable
	{
		private readonly Mutex Mutex;
		private readonly uint ActivationMessage;
		private bool ActivationSent;
		private HwndSource HwndSource;
		/// <summary>
		/// Occurs when <see cref="SendActivationMessage" /> is called by another running instance.
		/// </summary>
		public event EventHandler Activated;

		/// <summary>
		/// Initializes a new instance of the <see cref="SingleInstanceManager" /> class and registers a <see cref="System.Threading.Mutex" /> as well as a WindowMessage using the specified identifier.
		/// </summary>
		/// <param name="mutexName">A <see cref="string" /> representing the name of the <see cref="System.Threading.Mutex" /> and the WindowMessage.</param>
		public SingleInstanceManager(string mutexName)
		{
			Check.ArgumentNull(mutexName, nameof(mutexName));
			Check.ArgumentEx.StringNotEmpty(mutexName, nameof(mutexName));

			Mutex = new Mutex(false, mutexName);
			ActivationMessage = Native.RegisterWindowMessage("ACTIVATE_SINGLE_INSTANCE_" + mutexName);
		}
		/// <summary>
		/// Releases all resources used by the current instance of the <see cref="SingleInstanceManager" /> class.
		/// </summary>
		public void Dispose()
		{
			Mutex?.Dispose();
			HwndSource?.Dispose();
		}

		/// <summary>
		/// Registers a <see cref="Window" /> object that identifies as the main application window and creates a WndProc message pump.
		/// </summary>
		/// <param name="window">The <see cref="Window" /> object identifying as the main application window.</param>
		public void RegisterMainWindow(Window window)
		{
			Check.ArgumentNull(window, nameof(window));

			if (HwndSource == null)
			{
				HwndSource = HwndSource.FromHwnd(new WindowInteropHelper(window).EnsureHandle());
				HwndSource.AddHook(WndProc);
			}
			else
			{
				throw Throw.InvalidOperation("Main window was already registered.");
			}
		}
		/// <summary>
		/// Checks whether an instance is already running by querying the <see cref="System.Threading.Mutex" />.
		/// </summary>
		/// <returns>
		/// <see langword="true" />, if an instance is already running;
		/// otherwise, <see langword="false" />.
		/// </returns>
		public bool CheckInstanceRunning()
		{
			return !Mutex.WaitOne(0, false);
		}
		/// <summary>
		/// Sends a notification to other running instances. The <see cref="Activated" /> event will be triggered in all instances, except the current.
		/// </summary>
		public void SendActivationMessage()
		{
			ActivationSent = true;
			Native.SendNotifyMessage((IntPtr)(-1), ActivationMessage, 0, 0);
		}
		private IntPtr WndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
		{
			if (msg == ActivationMessage)
			{
				if (ActivationSent) ActivationSent = false;
				else OnActivated(EventArgs.Empty);
			}
			return IntPtr.Zero;
		}

		/// <summary>
		/// Raises the <see cref="Activated" /> event.
		/// </summary>
		/// <param name="e">The event data for the <see cref="Activated" /> event.</param>
		protected void OnActivated(EventArgs e)
		{
			Activated?.Invoke(this, e);
		}
	}
}