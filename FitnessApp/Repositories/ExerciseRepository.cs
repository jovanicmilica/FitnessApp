namespace FitnessApp.Repositories;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using FitnessApp.Models;

public class ExerciseRepository
{
    private string filePath = "data/exercises.json";
    private List<Exercise> exercises;

    public ExerciseRepository()
    {
        Load();
    }

    private void Load()
    {
        if (!File.Exists(filePath))
        {
            exercises = new List<Exercise>();
            return;
        }
        string json = File.ReadAllText(filePath);
        exercises = JsonSerializer.Deserialize<List<Exercise>>(json) ?? new List<Exercise>();
    }

    private void Save()
    {
        string json = JsonSerializer.Serialize(exercises, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(filePath, json);
    }

    public List<Exercise> GetAll()
    {
        return exercises;
    }

    public Exercise GetById(int id)
    {
        return exercises.FirstOrDefault(e => e.Id == id);
    }

    public void Add(Exercise exercise)
    {
        exercise.Id = exercises.Count > 0 ? exercises.Max(e => e.Id) + 1 : 1;
        exercises.Add(exercise);
        Save();
    }

    public void Update(Exercise exercise)
    {
        int index = exercises.FindIndex(e => e.Id == exercise.Id);
        if (index != -1)
        {
            exercises[index] = exercise;
            Save();
        }
    }

    public void Delete(int id)
    {
        exercises.RemoveAll(e => e.Id == id);
        Save();
    }
}
