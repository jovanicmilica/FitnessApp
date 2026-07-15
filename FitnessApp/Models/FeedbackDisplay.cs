namespace FitnessApp.Models;

public class FeedbackDisplay
{
    public int Rating { get; set; }
    public string Comment { get; set; } = string.Empty;
    public string TargetType { get; set; } = string.Empty;
    public string TargetName { get; set; } = string.Empty;
}