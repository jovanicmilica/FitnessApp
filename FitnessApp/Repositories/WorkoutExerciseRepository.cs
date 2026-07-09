namespace FitnessApp.Repositories;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using FitnessApp.Models;

public class WorkoutExerciseRepository
{
    private string filePath = "data/workoutexercises.json";
    private List<WorkoutExercise> workoutExercises;

    public WorkoutExerciseRepository()
    {
        Load();
    }

    private void Load()
    {
        if (!File.Exists(filePath))
        {
            workoutExercises = new List<WorkoutExercise>();
            return;
        }
        string json = File.ReadAllText(filePath);
        workoutExercises = JsonSerializer.Deserialize<List<WorkoutExercise>>(json) ?? new List<WorkoutExercise>();
    }

    private void Save()
    {
        string json = JsonSerializer.Serialize(workoutExercises, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(filePath, json);
    }

    public List<WorkoutExercise> GetAll()
    {
        return workoutExercises;
    }

    public WorkoutExercise GetById(int id)
    {
        return workoutExercises.FirstOrDefault(w => w.Id == id);
    }

    public void Add(WorkoutExercise workoutExercise)
    {
        workoutExercise.Id = workoutExercises.Count > 0 ? workoutExercises.Max(w => w.Id) + 1 : 1;
        workoutExercises.Add(workoutExercise);
        Save();
    }

    public void Update(WorkoutExercise workoutExercise)
    {
        int index = workoutExercises.FindIndex(w => w.Id == workoutExercise.Id);
        if (index != -1)
        {
            workoutExercises[index] = workoutExercise;
            Save();
        }
    }

    public void Delete(int id)
    {
        workoutExercises.RemoveAll(w => w.Id == id);
        Save();
    }
}
