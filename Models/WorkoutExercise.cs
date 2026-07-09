namespace FitnessApp.Models;

public class WorkoutExercise
{
    private int id;
    private int sets;
    private int repetitions;
    private int durationInSeconds;
    private int workoutId;
    private int exerciseId;

    public int Id { get => id; set => id = value; }
    public int Sets { get => sets; set => sets = value; }
    public int Repetitions { get => repetitions; set => repetitions = value; }
    public int DurationInSeconds { get => durationInSeconds; set => durationInSeconds = value; }
    public int WorkoutId { get => workoutId; set => workoutId = value; }
    public int ExerciseId { get => exerciseId; set => exerciseId = value; }

    public WorkoutExercise(int exerciseId, int sets, int repetitions, int durationInSeconds)
    {
        this.exerciseId = exerciseId;
        this.sets = sets;
        this.repetitions = repetitions;
        this.durationInSeconds = durationInSeconds;
    }
}