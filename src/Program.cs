// I hereby waive this project under the public domain - see UNLICENSE for details.

/// <summary>
/// Displays the scheduled article times in a formatted manner and provides
/// options to export the schedule or restart the scheduling process.
/// </summary>
void PrintTimes()
{
	var storeSchedule = new List<String>();
	var scheduledTimes = Generator.GenerateTimes();

	// Clear the screen on restart
	Console.Clear();

	Console.WriteLine("=== Stagger Post ===");
	foreach (var time in scheduledTimes)
	{
		var articleTime = $"{Generator.ConvertTo12Hour(time)}";
		// Correct format string to display time in 12-hour format with AM/PM
		Console.WriteLine(articleTime);
		// Store the schedule to memory for option export
		storeSchedule.Add(articleTime);
	}

	// Give the user an option to export the schedule
	if (Interactive.UserChoice("Retry?"))
		PrintTimes();

	// Give the user an option to export the schedule
	Export.ToJSON(storeSchedule, "config.toml");

	if (Interactive.UserChoice("Generate A New Batch?"))
		PrintTimes();
	else
	{
		Console.Clear();
		Environment.Exit(Environment.ExitCode);
	}
}

// Start the loop
PrintTimes();
