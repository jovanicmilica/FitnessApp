namespace FitnessApp.Models;

public class Client : User
{
    private int healthRecordId;

    public int HealthRecordId { get => healthRecordId; set => healthRecordId = value; }

    public void Register() { }
    public void EditHealthRecord() { }
    public List<Trainer> SearchTrainers(string criteria) { return null; }
    public Trainer ViewTrainerProfile(int trainerId) { return null; }
    public void SendTutelageRequest(int trainerId, int questionnaireId) { }
    public void LeaveFeedback(int workoutId, int rating, string comment) { }
    public void PayForTutelage(int tutelageId, decimal amount) { }
    public void SendMessage(string content, int tutelageId) { }
}