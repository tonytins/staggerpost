namespace PublishTimes;

/// <summary>
/// Represents the configuration settings for exporting a schedule,
/// including file name, directory path, and available topics.
/// </summary>
public class Config
{
    /// <summary>
    /// Gets or sets the name of the schedule file.
    /// </summary>
    public string? File { get; set; }

    /// <summary>
    /// Gets or sets the directory path where the schedule file is stored.
    /// </summary>
    public string? Path { get; set; }

    /// <summary>
    /// Gets or sets the list of available topics from the configuration file.
    /// </summary>
    public TomlArray? Topics { get; set; }
}
