// I hereby waive this project under the public domain - see UNLICENSE for details.
namespace StaggerPost;

/// <summary>
/// Provides debug-only console output methods.
/// These methods are only executed when the application is compiled in DEBUG mode.
/// </summary>
internal static class Tracer
{
	const string LOG = "[LOG]:";

	/// <summary>
	/// Writes a line of text to the console, but only when in DEBUG mode.
	/// </summary>
	/// <param name="content">The text to write to the console.</param>
	[Conditional("DEBUG")]
	internal static void LogLine(string content) => Console.WriteLine($"{LOG} {content}");

	/// <summary>
	/// Writes text to the console without a newline, but only when in DEBUG mode.
	/// </summary>
	/// <param name="content">The text to write to the console.</param>
	[Conditional("DEBUG")]
	internal static void Log(string content) => Console.Write($"{LOG} {content}");

	/// <summary>
	/// Writes multiple lines of text to the console, but only when in DEBUG mode.
	/// </summary>
	/// <param name="contents">A collection of text lines to write to the console.</param>
	[Conditional("DEBUG")]
	internal static void LogLine(IEnumerable<string> contents)
	{
		foreach (var content in contents)
		{
			Console.WriteLine($"{LOG} {content}");
		}
	}

	/// <summary>
	/// Writes multiple text entries to the console without newlines, but only when in DEBUG mode.
	/// </summary>
	/// <param name="contents">A collection of text entries to write to the console.</param>
	[Conditional("DEBUG")]
	internal static void Log(IEnumerable<string> contents)
	{
		foreach (var content in contents)
		{
			Console.Write($"{LOG} {content}");
		}
	}

	/// <summary>
	/// Gets the current working directory in DEBUG mode or the application's base directory in release mode.
	/// </summary>
	internal static string AppDirectory
	{
		get
		{
#if DEBUG
			return Directory.GetCurrentDirectory();
#else
			return AppDomain.CurrentDomain.BaseDirectory;
#endif
		}
	}

	/// <summary>
	/// Determines the appropriate output directory based on the given directory path.
	/// In DEBUG mode, it always returns the current working directory.
	/// In release mode, it returns the provided directory unless it contains a "/.", in which case it defaults to the current directory.
	/// </summary>
	/// <param name="dir">The directory path to evaluate.</param>
	/// <returns>The resolved output directory as a string.</returns>
	internal static string OutputDirectory(string dir)
	{
		var curDir = Directory.GetCurrentDirectory();

#if DEBUG
		return curDir;
#else
		if (dir.Contains("/."))
			return curDir;

		return dir;
#endif
	}
}
