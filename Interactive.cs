// I hereby waive this project under the public domain - see UNLICENSE for details.
namespace StaggerPost;

internal static class Interactive
{
	/// <summary>
	/// Prompts the user with a yes/no question and returns their choice as a boolean value.
	/// </summary>
	/// <param name="choice">The message to display to the user.</param>
	/// <returns>True if the user selects 'Y' or presses Enter, otherwise false.</returns>
	public static bool UserChoice(string choice)
	{
		Console.WriteLine($"{Environment.NewLine}{choice} Y/N");
		var input = Console.ReadKey().Key;
		if (input == ConsoleKey.Y || input == ConsoleKey.Enter)
			return true;

		return false;
	}

	/// <summary>
	/// Prompts the user to select a topic from a given list
	/// and returns the chosen topic.
	/// </summary>
	/// <param name="communities">An array of available topics.</param>
	/// <returns>The selected topic as a string.</returns>
	public static string SelectTopics(List<string> communities)
	{
		var topicChoice = "";
		var topicNum = 0;
		var userChoices = new List<string>();
		var numOfTopics = 0;
		var topicDict = new Dictionary<int, string>();

		foreach (var community in communities)
		{
			numOfTopics++;
			var title = community.Trim();
			topicDict.Add(numOfTopics, title);
			userChoices.Add($"{Environment.NewLine}{numOfTopics} {title}");
		}

		var topicSelect = string.Join(", ", userChoices.ToArray());
		Console.WriteLine($"Choose a Topic{Environment.NewLine}{topicSelect}");
		var input = Console.ReadLine();

		// Attempt to parse a number.
		if (int.TryParse(input, out topicNum) == true)
			topicChoice = topicDict[topicNum];
		else
			SelectTopics(communities);

		return topicChoice;
	}

	/// <summary>
	/// Prompts the user to select a date (either today or tomorrow) and returns the selected date as a formatted string.
	/// </summary>
	/// <returns>A string representing the selected date in a short date format.</returns>
	public static string SelectDate()
	{
		var dtChoices = new[] { "Today", "Tomorrow" };
		var dtDict = new Dictionary<int, string>();
		var dtSelection = new List<string>();
		var dtChoice = 0;
		var dtNum = 0;

		foreach (var days in dtChoices)
		{
			dtNum++;
			var day = days.Trim();
			dtDict.Add(dtNum, day);
			dtSelection.Add($"{dtNum} {day}");
		}

		var topicSelect = string.Join(", ", dtSelection.ToArray());
		Console.WriteLine($"{Environment.NewLine}Choose a Date{Environment.NewLine}{topicSelect}");
		var input = Console.ReadLine();

		// Attempt to parse a number.
		if (int.TryParse(input, out dtNum) == true)
			dtChoice = dtNum;

		// Any choice above 2 tomorrow
		if (dtChoice >= 2)
		{
			var dt = DateTime.Now.AddDays(1);
			return dt.ToString("d");
		}

		return DateTime.Today.ToString("d");
	}
}
