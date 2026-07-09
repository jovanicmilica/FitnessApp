namespace FitnessApp.Models;

public class Workout
{
    private int id;
    private string name;
    private DateTime dateCreated;
    private WorkoutStatus status;
    private int trainerId;
    private int tutelageId;

    public int Id { get => id; set => id = value; }
    public string Name { get => name; set => name = value; }
    public DateTime DateCreated { get => dateCreated; set => dateCreated = value; }
    public WorkoutStatus Status { get => status; set => status = value; }
    public int TrainerId { get => trainerId; set => trainerId = value; }
    public int TutelageId { get => tutelageId; set => tutelageId = value; }

    public void AddExercise(int exerciseId, int sets, int repetitions) { }
    public void RemoveExercise(int exerciseId) { }
    public void MarkAsCompleted() { }
}