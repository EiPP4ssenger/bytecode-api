﻿using BytecodeApi.Extensions;
using BytecodeApi.Threading;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading;

namespace BytecodeApi.IO
{
	/// <summary>
	/// Proides <see langword="static" /> methods for interoperability with executable files, typically EXE and DLL files.
	/// </summary>
	public static class Executable
	{
		/// <summary>
		/// Creates a <see cref="Process" /> with the specified commandline and the specified <see cref="ProcessIntegrityLevel" />. If process creation fails, a <see cref="Win32Exception" /> is thrown. This is typically used to create processes with lower integrity.
		/// </summary>
		/// <param name="commandLine">A <see cref="string" /> specifying the commandline to create the <see cref="Process" /> with.</param>
		/// <param name="integrityLevel">The <see cref="ProcessIntegrityLevel" /> to create the <see cref="Process" /> with. This is usually lower than the <see cref="ProcessIntegrityLevel" /> of the current <see cref="Process" />.</param>
		/// <returns>
		/// The <see cref="Process" /> this method creates.
		/// </returns>
		public static Process CreateProcessWithIntegrity(string commandLine, ProcessIntegrityLevel integrityLevel)
		{
			Check.ArgumentNull(commandLine, nameof(commandLine));

			IntPtr token = IntPtr.Zero;
			IntPtr newToken = IntPtr.Zero;
			IntPtr integritySid = IntPtr.Zero;
			IntPtr tokenInfoPtr = IntPtr.Zero;
			Native.StartupInfo startupInfo = new Native.StartupInfo();
			Native.ProcessInformation processInformation = new Native.ProcessInformation();

			try
			{
				using (Process process = Process.GetCurrentProcess())
				{
					token = process.OpenToken(0x8b);
				}

				if (token != IntPtr.Zero)
				{
					if (Native.DuplicateTokenEx(token, 0, IntPtr.Zero, 2, 1, out newToken))
					{
						Native.SidIdentifierAuthority securityMandatoryLabelAuthority = new Native.SidIdentifierAuthority { Value = new byte[] { 0, 0, 0, 0, 0, 16 } };
						if (Native.AllocateAndInitializeSid(ref securityMandatoryLabelAuthority, 1, (int)integrityLevel, 0, 0, 0, 0, 0, 0, 0, out integritySid))
						{
							Native.TokenMandatoryLabel mandatoryTokenLabel;
							mandatoryTokenLabel.Label.Attributes = 0x20;
							mandatoryTokenLabel.Label.Sid = integritySid;

							int tokenInfo = Marshal.SizeOf(mandatoryTokenLabel);
							tokenInfoPtr = Marshal.AllocHGlobal(tokenInfo);
							Marshal.StructureToPtr(mandatoryTokenLabel, tokenInfoPtr, false);
							if (Native.SetTokenInformation(newToken, 25, tokenInfoPtr, tokenInfo + Native.GetLengthSid(integritySid)))
							{
								startupInfo.StructSize = Marshal.SizeOf(startupInfo);
								if (Native.CreateProcessAsUser(newToken, null, commandLine, IntPtr.Zero, IntPtr.Zero, false, 0, IntPtr.Zero, null, ref startupInfo, out processInformation))
								{
									return Process.GetProcessById(processInformation.ProcessId);
								}
							}
						}
					}
				}
			}
			finally
			{
				if (token != IntPtr.Zero) Native.CloseHandle(token);
				if (newToken != IntPtr.Zero) Native.CloseHandle(newToken);
				if (integritySid != IntPtr.Zero) Native.FreeSid(integritySid);
				if (tokenInfoPtr != IntPtr.Zero) Marshal.FreeHGlobal(tokenInfoPtr);
				if (processInformation.Process != IntPtr.Zero) Native.CloseHandle(processInformation.Process);
				if (processInformation.Thread != IntPtr.Zero) Native.CloseHandle(processInformation.Thread);
			}

			throw Throw.Win32("Process could not be created.");
		}
		/// <summary>
		/// Executes a .NET executable from a <see cref="byte" />[] by invoking the main entry point. The Main method must either have no parameters or one <see cref="string" />[] parameter. If it has a parameter, <see langword="new" /> <see cref="string" />[0] is passed.
		/// </summary>
		/// <param name="executable">A <see cref="byte" />[] that represents a .NET executable.</param>
		public static void ExecuteDotNetAssembly(byte[] executable)
		{
			ExecuteDotNetAssembly(executable, null);
		}
		/// <summary>
		/// Executes a .NET executable from a <see cref="byte" />[] by invoking the main entry point. The Main method must either have no parameters or one <see cref="string" />[] parameter. If it has a parameter, <paramref name="args" /> is passed, otherwise <paramref name="args" /> is ignored.
		/// </summary>
		/// <param name="executable">A <see cref="byte" />[] that represents a .NET executable.</param>
		/// <param name="args">A <see cref="string" />[] representing the arguments that is passed to the main entry point, if the Main method has a <see cref="string" />[] parameter.</param>
		public static void ExecuteDotNetAssembly(byte[] executable, params string[] args)
		{

			ExecuteDotNetAssembly(executable, args, false);
		}
		/// <summary>
		/// Executes a .NET executable from a <see cref="byte" />[] by invoking the main entry point. The Main method must either have no parameters or one <see cref="string" />[] parameter. If it has a parameter, <paramref name="args" /> is passed, otherwise <paramref name="args" /> is ignored.
		/// </summary>
		/// <param name="executable">A <see cref="byte" />[] that represents a .NET executable.</param>
		/// <param name="args">A <see cref="string" />[] representing the arguments that is passed to the main entry point, if the Main method has a <see cref="string" />[] parameter.</param>
		/// <param name="thread"><see langword="true" /> to invoke the main entry point in a new thread.</param>
		public static void ExecuteDotNetAssembly(byte[] executable, string[] args, bool thread)
		{
			Check.ArgumentNull(executable, nameof(executable));

			MethodInfo method = Assembly.Load(executable).EntryPoint;
			ParameterInfo[] parameters = method.GetParameters();

			Action invoke;

			if (parameters.Length == 0) invoke = () => method.Invoke();
			else if (parameters.Length == 1 && parameters.First().ParameterType == typeof(string[])) invoke = () => method.Invoke(null, Singleton.Array(args ?? new string[0]));
			else throw Throw.InvalidOperation("Executable does not contain a static 'main' method suitable for an entry point.");

			if (thread) ThreadFactory.StartThread(invoke);
			else invoke();
		}
		/// <summary>
		/// Detects one or multiple process analysers, such as sandboxes, virtual environments, or specific debuggers or profilers.
		/// </summary>
		/// <param name="processAnalysers">The <see cref="ProcessAnalysers" /> flags specifying which process analysers to test.</param>
		/// <returns>
		/// <see langword="true" />, if the specified process analyser has been detected;
		/// otherwise, <see langword="false" />.
		/// </returns>
		public static bool DetectProcessAnalyser(ProcessAnalysers processAnalysers)
		{
			//FEATURE: Sandboxes, virtual machines, CheatEngine (https://www.aspfree.com/c/a/braindump/virtualization-and-sandbox-detection/)		
			if (processAnalysers.HasFlag(ProcessAnalysers.Sandboxie))
			{
				return Native.GetModuleHandle("SbieDll") != IntPtr.Zero;
			}
			else if (processAnalysers.HasFlag(ProcessAnalysers.Wireshark))
			{
				return Process.GetProcessesByName("Wireshark").Any() || Process.GetProcesses().Any(process => CSharp.Try(() => process?.MainWindowTitle).CompareTo("The Wireshark Network Analyzer", SpecialStringComparisons.NullAndEmptyEqual | SpecialStringComparisons.IgnoreCase) == 0);
			}
			else if (processAnalysers.HasFlag(ProcessAnalysers.ProcessMonitor))
			{
				return Process.GetProcesses().Any(process => CSharp.Try(() => process?.MainWindowTitle).Contains("Process Monitor -", SpecialStringComparisons.IgnoreCase));
			}
			else if (processAnalysers.HasFlag(ProcessAnalysers.Emulator))
			{
				int start = Environment.TickCount;
				Stopwatch stopwatch = ThreadFactory.StartStopwatch();
				Thread.Sleep(500);
				int stop = Environment.TickCount;
				return stop - start < 450 || stopwatch.Elapsed < TimeSpan.FromMilliseconds(450);
			}
			else
			{
				throw Throw.InvalidEnumArgument(nameof(processAnalysers));
			}
		}
		/// <summary>
		/// If the current <see cref="Process" /> is running with elevated privileges, a blue screen is triggered and the operating system is terminated; otherwise, an exception is thrown.
		/// </summary>
		public static void BlueScreen()
		{
			int processInformation = 1;
			if (Native.NtSetInformationProcess(Process.GetCurrentProcess().Handle, 29, ref processInformation, 4) == 0)
			{
				Environment.Exit(0);
			}

			throw Throw.InvalidOperation("Could not trigger a blue screen.");
		}
	}
}