public class Schedule
{
	[JsonPropertyName("community")]
	public string Community { get; set; } = "";

	[JsonPropertyName("date")]
	public string Date { get; set; } = "";

	[JsonPropertyName("times")]
	public IList<string> Times { get; set; } = new List<string>();
}
