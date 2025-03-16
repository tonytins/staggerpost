// I hereby waive this project under the public domain - see UNLICENSE for details.
namespace PublishTimes;

/// <summary>
/// Provides debug-only console output methods.
/// These methods are only executed when the application is compiled in DEBUG mode.
/// </summary>
internal static class Tracer
{
    /// <summary>
    /// Writes a line of text to the console, but only when in DEBUG mode.
    /// </summary>
    /// <param name="content">The text to write to the console.</param>
    [Conditional("DEBUG")]
    internal static void WriteLine(string content) =>
        Console.WriteLine(content);

    /// <summary>
    /// Writes text to the console without a newline, but only when in DEBUG mode.
    /// </summary>
    /// <param name="content">The text to write to the console.</param>
    [Conditional("DEBUG")]
    internal static void Write(string content) =>
        Console.Write(content);

    /// <summary>
    /// Writes multiple lines of text to the console, but only when in DEBUG mode.
    /// </summary>
    /// <param name="contents">A collection of text lines to write to the console.</param>
    [Conditional("DEBUG")]
    internal static void WriteLine(IEnumerable<string> contents)
    {
        foreach (var content in contents)
        {
            Console.WriteLine(content);
        }
    }

    /// <summary>
    /// Writes multiple text entries to the console without newlines, but only when in DEBUG mode.
    /// </summary>
    /// <param name="contents">A collection of text entries to write to the console.</param>
    [Conditional("DEBUG")]
    internal static void Write(IEnumerable<string> contents)
    {
        foreach (var content in contents)
        {
            Console.Write(content);
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
  return  AppDomain.CurrentDomain.BaseDirectory;
#endif
        }
    }
}
