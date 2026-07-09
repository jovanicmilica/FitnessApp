namespace FitnessApp.Repositories;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using FitnessApp.Models;

public class WorkoutRepository
{
    private string filePath = "data/workouts.json";
    private List<Workout> workouts;

    public WorkoutRepository()
    {
        Load();
    }

    private void Load()
    {
        if (!File.Exists(filePath))
        {
            workouts = new List<Workout>();
            return;
        }
        string json = File.ReadAllText(filePath);
        workouts = JsonSerializer.Deserialize<List<Workout>>(json) ?? new List<Workout>();
    }

    private void Save()
    {
        string json = JsonSerializer.Serialize(workouts, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(filePath, json);
    }

    public List<Workout> GetAll()
    {
        return workouts;
    }

    public Workout GetById(int id)
    {
        return workouts.FirstOrDefault(w => w.Id == id);
    }

    public void Add(Workout workout)
    {
        workout.Id = workouts.Count > 0 ? workouts.Max(w => w.Id) + 1 : 1;
        workouts.Add(workout);
        Save();
    }

    public void Update(Workout workout)
    {
        int index = workouts.FindIndex(w => w.Id == workout.Id);
        if (index != -1)
        {
            workouts[index] = workout;
            Save();
        }
    }

    public void Delete(int id)
    {
        workouts.RemoveAll(w => w.Id == id);
        Save();
    }
}
