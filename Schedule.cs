
public class Schedule
{
    [JsonPropertyName("topic")]
    public string Topic { get; set; } = "";

    [JsonPropertyName("times")]
    public IList<string> Times { get; set; } = new List<string>();
}
