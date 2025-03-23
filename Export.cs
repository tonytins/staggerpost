// I hereby waive this project under the public domain - see UNLICENSE for details.
namespace StaggerPost;

internal static class Export
{
	/// <summary>
	/// Retrieves configuration settings from a TOML file if it exists; otherwise, returns a default configuration.
	/// </summary>
	/// <param name="file">The name of the configuration file (defaults to "config.toml").</param>
	/// <returns>A Config object populated with values from the file, or a default Config instance if the file is not found.</returns>
	static Config GetConfig(string file)
	{
		var cfgPath = Path.Combine(Tracer.AppDirectory, file);

		if (!File.Exists(cfgPath))
		{
			Tracer.LogLine("Config file not found. Switching to defaults.");
			var defaultList = new[]
			{
				"games@lemmy.world",
				"politics@lemmy.world",
				"science@lemmy.world",
				"technology@lemmy.world",
			};

			var config = new Config()
			{
				File = "schedule.json",
				Path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
				Communities = defaultList.ToList(),
			};

			return config;
		}

		Tracer.LogLine($"Discovered config file: {cfgPath}");
		var toml = File.ReadAllText(cfgPath);
		var model = Toml.ToModel<Config>(toml);

		return model;
	}

	/// <summary>
	/// Exports the scheduled articles to a file, allowing the user to modify
	/// the directory, filename, and list of topics based on
	/// a configuration file if available.
	/// </summary>
	public static void ToJSON(List<String> storeTimes, string cfgPath)
	{
		// File directory is used for file location set in config
		var topics = new List<string>();
		var config = GetConfig(cfgPath);
		var outputDir = Tracer.OutputDirectory(config.Path!);
		var outputFile = config.File;
		var filePath = Path.Combine(outputDir, outputFile!);
		var chosenTopic = "";
		var times = new List<string>();

		// If the config file exists, read from that but don't assume anything is filled
		if (File.Exists(cfgPath))
		{
			var toml = File.ReadAllText(cfgPath);
			var usrDir = config.Path;
			var usrFileName = config.File;
			// Convert list into array
			var list = config.Communities;
			var tomlList = string.Join(", ", list);
			var usrTopics = tomlList.Split(',');

			if (string.IsNullOrEmpty(usrDir))
				return;

			outputDir = usrDir;

			if (string.IsNullOrEmpty(usrFileName))
				return;

			outputFile = usrFileName;

			// If array is empty, return; otherwise, apply config
			if (usrTopics.Length < 0)
				return;

			foreach (var usrTopic in usrTopics)
				topics.Add(usrTopic);

			// Set new file Path
			filePath = Path.Combine(outputDir, outputFile!);
		}

		if (!File.Exists(filePath))
			File.WriteAllText(filePath, "[]");

		foreach (var time in storeTimes)
			times.Add(time.Trim());

		// Set new topic
		topics = config.Communities.ToList();
		Console.Clear();
		chosenTopic = Interactive.SelectTopics(topics);

		var date = Interactive.SelectDate();

		// Write to file.
		var jsonFile = File.ReadAllText(filePath);
		var jsonList = string.IsNullOrWhiteSpace(jsonFile)
			? new List<Schedule>()
			: JsonSerializer.Deserialize<List<Schedule>>(jsonFile) ?? new List<Schedule>();

		jsonList.Add(
			new Schedule()
			{
				Community = chosenTopic.Trim(),
				Date = date.Trim(),
				Times = times,
			}
		);

		var jsonOptions = new JsonSerializerOptions() { WriteIndented = true };

		var json = JsonSerializer.Serialize(jsonList, jsonOptions);
		File.WriteAllText(filePath, json);
		Tracer.LogLine($"{json}{Environment.NewLine}Written to: {filePath}");

		// Clear list from memory
		storeTimes.Clear();
	}
}
