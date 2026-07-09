namespace FitnessApp.Models;

public class HealthRecord
{
    private int id;
    private List<string> healthConditions;
    private List<string> disabilities;

    public int Id { get => id; set => id = value; }
    public List<string> HealthConditions { get => healthConditions; set => healthConditions = value; }
    public List<string> Disabilities { get => disabilities; set => disabilities = value; }

    public void AddHealthCondition(string condition) { }
    public void RemoveHealthCondition(string condition) { }
    public void UpdateHealthConditions(List<string> conditions) { }
    public void AddDisability(string disability) { }
    public void RemoveDisability(string disability) { }
    public void UpdateDisabilities(List<string> disabilities) { }
}