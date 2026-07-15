namespace FitnessApp.Models;

using System;
using System.Collections.Generic;
using FitnessApp.Enums; 
using FitnessApp.Repositories;

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

    public string ExerciseName
    {
        get
        {
            var repo = new ExerciseRepository();
            var exercise = repo.GetById(ExerciseId);
            return exercise?.Name ?? $"Exercise {ExerciseId}";
        }
    }

    public override string ToString()
    {
        return ExerciseName;
    }

    public WorkoutExercise(int exerciseId, int sets, int repetitions, int durationInSeconds)
    {
        this.exerciseId = exerciseId;
        this.sets = sets;
        this.repetitions = repetitions;
        this.durationInSeconds = durationInSeconds;
    }
}