// I hereby waive this project under the public domain - see UNLICENSE for details.
const string banner = "=== Publish Times ===";

var numberOfArticles = 5; // Define how many articles to schedule
var startTime = new TimeSpan(9, 0, 0); // Starting time at 9:00 AM
var rng = new Random();
var scheduledTimes = new List<TimeSpan>();
var storeSchedule = new List<String>();
// App directory is used for config file
var appDir = Directory.GetCurrentDirectory();
// File directory is used for file location set in config
var fileDir = Directory.GetCurrentDirectory();
var isRestart = false;
var communities = new[] { "Games", "Politics", "Research", "Technology" };
var scheduleFile = "schedule.txt";
var cfgFile = "config.toml";

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

/// <summary>
/// Converts a TimeSpan into a 12-hour AM/PM formatted time string.
/// </summary>
/// <param name="time">The TimeSpan representing the time of day.</param>
/// <returns>A formatted string representing the time in AM/PM format.</returns>
string TimeSpanToAMPM(TimeSpan time)
{
    var minutes = time.TotalMinutes;
    var hours12 = time.Hours % 12;

    if (hours12 == 0)
        hours12 = 1;

    var period = time.Hours >= 12 ? "PM" : "AM";
    return $"{hours12}:{time.Minutes:D2} {period}";
}

/// <summary>
/// Exports the scheduled articles to a file, allowing the user to modify
/// the directory, filename, and list of topics based on
/// a configuration file if available.
/// </summary>
void ExportSchedule()
{
    var cfgPath = Path.Combine(appDir, cfgFile);
    var filePath = Path.Combine(fileDir, scheduleFile);
    var appendSchedule = false;
    var topic = "";

    var chooseTopic = rng.Next(0, communities.Length);
    topic = communities[chooseTopic];

    // If the config file exists, read from that but don't assume anything is filled
    if (File.Exists(cfgPath))
    {
        var toml = File.ReadAllText(cfgPath);
        var model = Toml.ToModel<Config>(toml);
        var usrDir = model.Path;
        var usrFileName = model.File;
        var tomlList = string.Join(", ", model.Topics);
        var usrList = tomlList.Split(',');

        if (!string.IsNullOrEmpty(usrDir))
            fileDir = usrDir;

        if (!string.IsNullOrEmpty(usrFileName))
            scheduleFile = usrFileName;

        if (usrList.Length > 0)
        {
            var chooseUsrTopic = rng.Next(0, usrList.Length);
            topic = usrList[chooseUsrTopic];
        }


        // Set new file Path
        filePath = Path.Combine(fileDir, scheduleFile);
    }

    // If the file already exists, assume a previous schedule was written
    if (File.Exists(filePath))
    {
        Console.WriteLine($"{Environment.NewLine}Add another schedule? Y/N");
        if (Console.ReadKey().Key == ConsoleKey.Y)
            appendSchedule = true;

        // Write to file.
        using (var outputFile = new StreamWriter(filePath, appendSchedule))
        {
            outputFile.WriteLine($"        === {topic} ===");
            foreach (var line in storeSchedule)
                outputFile.WriteLine(line);
        }
    }

    // Clear list from memory before exit
    storeSchedule.Clear();

}

/// <summary>
/// Displays the scheduled article times in a formatted manner and provides
/// options to export the schedule or restart the scheduling process.
/// </summary>
void PrintSchedule()
{
    if (isRestart)
        Console.Clear();

    Console.WriteLine(banner);
    foreach (var time in scheduledTimes)
    {
        var articleTime = $"Article {scheduledTimes.IndexOf(time) + 1} Scheduled at: {TimeSpanToAMPM(time)}";
        // Correct format string to display time in 12-hour format with AM/PM
        Console.WriteLine(articleTime);
        // Store the schedule to memory for option export
        storeSchedule.Add(articleTime);
    }

    // Give the user an option to export the schedule
    Console.WriteLine($"{Environment.NewLine}Export? Y/N");
    if (Console.ReadKey().Key == ConsoleKey.Y)
        ExportSchedule();

    Console.WriteLine($"{Environment.NewLine}Start Over? Y/N");
    if (Console.ReadKey().Key == ConsoleKey.Y)
    {
        isRestart = true;
        PrintSchedule();
    }
    else
    {
        Console.Clear();
        Environment.Exit(Environment.ExitCode);
    }
}

// Start the loop
PrintSchedule();
