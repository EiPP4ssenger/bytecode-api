﻿using BytecodeApi.Threading;
using System;
using System.Windows.Threading;

namespace BytecodeApi.UI.Mathematics
{
	/// <summary>
	/// Provoides a wrapper for <see cref="Action" /> objects to be deferred.
	/// <para>Example: A search TextBox, where the search is performed 0.5 seconds after typing activity stopped.</para>
	/// </summary>
	public class Deferrer : IDisposable
	{
		private readonly DispatcherTimer UpdateTimer;
		private DateTime LastInvocation;
		private Action LastAction;
		/// <summary>
		/// Gets or sets the amount of time that needs to pass between consecutive calls to <see cref="Invoke(Action)" />, before the speficied <see cref="Action" /> is invoked.
		/// </summary>
		public TimeSpan Delay { get; set; }
		/// <summary>
		/// Gets or sets a value indicating whether this <see cref="Deferrer" /> is enabled. If set to <see langword="false" />, invocations are not handled.
		/// </summary>
		public bool IsEnabled { get; set; }
		/// <summary>
		/// Occurs when an <see cref="Action" /> was invoked. This event will only be invoked, if a call to <see cref="Invoke(Action)" /> was not deferred.
		/// </summary>
		public event EventHandler Invoked;

		/// <summary>
		/// Initializes a new instance of the <see cref="Deferrer" /> class with the specified delay.
		/// </summary>
		/// <param name="delay">The amount of time that needs to pass between a consecutive call to <see cref="Invoke(Action)" />, before the speficied <see cref="Action" /> is invoked.</param>
		public Deferrer(TimeSpan delay)
		{
			Delay = delay;
			IsEnabled = true;

			UpdateTimer = ThreadFactory.StartDispatcherTimer(() =>
			{
				if (IsEnabled && DateTime.Now - LastInvocation > Delay)
				{
					LastInvocation = DateTime.MaxValue;
					LastAction?.Invoke();
					OnInvoked(EventArgs.Empty);
				}
			}, TimeSpan.FromMilliseconds(10));
		}
		/// <summary>
		/// Initializes a new instance of the <see cref="Deferrer" /> class with the specified delay.
		/// </summary>
		/// <param name="delay">The amount of time in milliseconds that needs to pass between a consecutive call to <see cref="Invoke(Action)" />, before the speficied <see cref="Action" /> is invoked.</param>
		public Deferrer(int delay) : this(TimeSpan.FromMilliseconds(delay))
		{
		}
		/// <summary>
		/// Releases all resources used by the current instance of the <see cref="Deferrer" /> class.
		/// </summary>
		public void Dispose()
		{
			UpdateTimer.Stop();
		}

		/// <summary>
		/// Invokes <paramref name="action" />, if the last call to <see cref="Invoke(Action)" /> was longer ago than <see cref="Delay" />.
		/// </summary>
		/// <param name="action">The <see cref="Action" /> to be invoked.</param>
		public void Invoke(Action action)
		{
			Invoke(action, false);
		}
		/// <summary>
		/// Invokes <paramref name="action" />, if the last call to <see cref="Invoke(Action)" /> was longer ago than <see cref="Delay" />.
		/// </summary>
		/// <param name="action">The <see cref="Action" /> to be invoked.</param>
		/// <param name="now"><see langword="true" /> to always invoke <paramref name="action" />, regardless of <see cref="Delay" />.</param>
		public void Invoke(Action action, bool now)
		{
			Check.ArgumentNull(action, nameof(action));

			LastAction = action;
			LastInvocation = now ? DateTime.MinValue : DateTime.Now;
		}

		/// <summary>
		/// Raises the <see cref="Invoked" /> event.
		/// </summary>
		/// <param name="e">The event data for the <see cref="Invoked" /> event.</param>
		protected void OnInvoked(EventArgs e)
		{
			Invoked?.Invoke(this, e);
		}
	}
}