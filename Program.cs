// I hereby waive this project under the public domain - see UNLICENSE for details.

/// <summary>
/// Retrieves configuration settings from a TOML file if it exists; otherwise, returns a default configuration.
/// </summary>
/// <param name="file">The name of the configuration file (defaults to "config.toml").</param>
/// <returns>A Config object populated with values from the file, or a default Config instance if the file is not found.</returns>
Config GetConfig(string file = "config.toml")
{
    // App directory is used for config file
    var appDir = AppDomain.CurrentDomain.BaseDirectory;
    var cfgPath = Path.Combine(appDir, file);

    if (File.Exists(cfgPath))
    {
        var toml = File.ReadAllText(cfgPath);
        var model = Toml.ToModel<Config>(toml);

        return model;
    }

    return new Config();
}

/// <summary>
/// Prompts the user with a yes/no question and returns their choice as a boolean value.
/// </summary>
/// <param name="choice">The message to display to the user.</param>
/// <returns>True if the user selects 'Y' or presses Enter, otherwise false.</returns>
bool UserChoice(string choice)
{
    Console.WriteLine($"{Environment.NewLine}{choice} Y/N");
    var input = Console.ReadKey().Key;
    if (input == ConsoleKey.Y || input == ConsoleKey.Enter)
        return true;

    return false;
}

/// <summary>
/// Generates a schedule of article publishing times, ensuring a randomized
/// delay between each while avoiding time conflicts within a 30-minute window.
/// </summary>
/// <returns>A list of TimeSpan objects representing scheduled article times.</returns>
List<TimeSpan> GenerateSchedule()
{
    var numberOfArticles = 5; // Define how many articles to schedule
    var startTime = new TimeSpan(9, 0, 0); // Starting time at 9:00 AM
    var rng = new Random();
    var scheduledTimes = new List<TimeSpan>();

    for (int i = 0; i < numberOfArticles; i++)
    {
        var baseDelayHours = rng.Next(2, 4); // Randomly choose between 2-3 hours delay
        var minutesToAdd = rng.Next(0, 60); // Randomly choose minutes (0-59)

        // Calculate new time by adding base delay and random minutes
        var nextTime = startTime.Add(new TimeSpan(baseDelayHours, minutesToAdd, 0));

        // Check if the new time is within 30 minutes of any existing time
        while (scheduledTimes.Exists(previousTime => Math.Abs((nextTime - previousTime).TotalMinutes) < 30))
        {
            // If the new time is within 30 minutes of an existing time, adjust it
            nextTime = nextTime.Add(new TimeSpan(0, 30, 0));
        }

        scheduledTimes.Add(nextTime);
        startTime = nextTime; // Update start time for the next article
    }

    return scheduledTimes;
}

/// <summary>
/// Converts a TimeSpan into a 12-hour AM/PM formatted time string.
/// </summary>
/// <param name="time">The TimeSpan representing the time of day.</param>
/// <returns>A formatted string representing the time in AM/PM format.</returns>
string ConvertTo12Hour(TimeSpan time)
{
    var minutes = time.TotalMinutes;
    var hours12 = time.Hours % 12;

    if (hours12 == 0)
        hours12 = 1;

    var period = time.Hours >= 12 ? "PM" : "AM";
    return $"{hours12}:{time.Minutes:D2} {period}";
}

/// <summary>
/// Prompts the user to select a topic from a given list
/// and returns the chosen topic.
/// </summary>
/// <param name="topics">An array of available topics.</param>
/// <returns>The selected topic as a string.</returns>
string SelectTopics(List<string> topics)
{
    var topicChoice = "";
    var topicNum = 0;
    var userChoices = new List<string>();
    var numOfTopics = 0;
    var topicDict = new Dictionary<int,
      string>();

    foreach (var topic in topics)
    {
        numOfTopics++;
        var title = topic.Trim();
        topicDict.Add(numOfTopics, title);
        userChoices.Add($"{numOfTopics} {title}");
    }

    var selection = string.Join(", ", userChoices.ToArray());
    Console.WriteLine($"{Environment.NewLine}Select a Number{Environment.NewLine}{selection}");
    var input = Console.ReadLine();

    // Attempt to parse a number.
    if (int.TryParse(input, out topicNum) == true)
        topicChoice = topicDict[topicNum];
    else
        NewTopic(topics);

    return topicChoice;
}

/// <summary>
/// Allows the user to choose a new topic from a given list or default to placeholder if no selection is made.
/// </summary>
/// <param name="topics">A list of available topics.</param>
/// <returns>The selected topic or a default placeholder if none is chosen.</returns>
string NewTopic(List<string> topics)
{
    var newTopic = "";

    if (UserChoice("Choose a Topic?"))
        newTopic = SelectTopics(topics);
    else
        newTopic = "Any";

    return newTopic;
}

/// <summary>
/// Exports the scheduled articles to a file, allowing the user to modify
/// the directory, filename, and list of topics based on
/// a configuration file if available.
/// </summary>
void ExportSchedule(List<String> storeSchedule)
{
    // App directory is used for config file
    var appDir = AppDomain.CurrentDomain.BaseDirectory;
    // File directory is used for file location set in config
    var outputDir = Directory.GetCurrentDirectory();
    var cfgFile = "config.toml";

    var topics = new List<string>();
    var cfgPath = Path.Combine(appDir, cfgFile);
    var config = GetConfig(cfgPath);
    var outputFile = config.File;
    var filePath = Path.Combine(outputDir, outputFile!);
    var chosenTopic = "";


    // If the config file exists, read from that but don't assume anything is filled
    if (File.Exists(cfgPath))
    {
        var toml = File.ReadAllText(cfgPath);
        var usrDir = config.Path;
        var usrFileName = config.File;
        // Convert list into array
        var usrTopics = config.Topics;

        if (!string.IsNullOrEmpty(usrDir))
            outputDir = usrDir;

        if (!string.IsNullOrEmpty(usrFileName))
            outputFile = usrFileName;

        // If array is populated, apply config
        if (!usrTopics.Any())
        {
            foreach (var usrTopic in usrTopics)
                topics.Add(usrTopic);
        }

        // Set new file Path
        filePath = Path.Combine(outputDir, outputFile!);
    }

    // Set new topic
    topics = config.Topics.ToList();
    chosenTopic = NewTopic(topics);

    // Write to file.
    JsonWriterOptions options = new()
    {
        Indented = true
    };

    var jsonList = JsonSerializer.Deserialize<List<Schedule>>(filePath)
                          ?? new List<Schedule>();

    jsonList.Add(new Schedule()
    {
        Topic = chosenTopic,
        Times = storeSchedule,
    });
    var jsonString = JsonSerializer.Serialize(jsonList);
    Console.WriteLine(jsonString);
    // File.WriteAllText(filePath, jsonString);
    Console.WriteLine($"{Environment.NewLine}Written to: {filePath}");

    // Clear list from memory
    storeSchedule.Clear();
}

/// <summary>
/// Displays the scheduled article times in a formatted manner and provides
/// options to export the schedule or restart the scheduling process.
/// </summary>
void PrintSchedule(bool isRestart = false)
{
    var storeSchedule = new List<String>();
    var scheduledTimes = GenerateSchedule();

    // Clear the screen on restart
    if (isRestart)
        Console.Clear();

    Console.WriteLine("=== Publish Times ===");
    foreach (var time in scheduledTimes)
    {
        var articleTime = $"{ConvertTo12Hour(time)}";
        // Correct format string to display time in 12-hour format with AM/PM
        Console.WriteLine(articleTime);
        // Store the schedule to memory for option export
        storeSchedule.Add(articleTime);
    }

    // Give the user an option to export the schedule
    if (UserChoice("Retry?"))
        PrintSchedule(true);

    // Give the user an option to export the schedule
    if (UserChoice("Export?"))
        ExportSchedule(storeSchedule);

    if (UserChoice("Generate A New Batch?"))
        PrintSchedule(true);
    else
    {
        Console.Clear();
        Environment.Exit(Environment.ExitCode);
    }
}

// Start the loop
PrintSchedule();
