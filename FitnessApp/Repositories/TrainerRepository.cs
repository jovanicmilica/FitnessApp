namespace FitnessApp.Repositories;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using FitnessApp.Models;

public class TrainerRepository
{
    private string filePath = "data/trainers.json";
    private List<Trainer> trainers;

    public TrainerRepository()
    {
        Load();
    }

    private void Load()
    {
        if (!File.Exists(filePath))
        {
            trainers = new List<Trainer>();
            return;
        }
        string json = File.ReadAllText(filePath);
        trainers = JsonSerializer.Deserialize<List<Trainer>>(json) ?? new List<Trainer>();
    }

    private void Save()
    {
        string json = JsonSerializer.Serialize(trainers, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(filePath, json);
    }

    public List<Trainer> GetAll()
    {
        return trainers;
    }

    public Trainer GetById(int id)
    {
        return trainers.FirstOrDefault(t => t.Id == id);
    }

    public void Add(Trainer trainer)
    {
        trainer.Id = trainers.Count > 0 ? trainers.Max(t => t.Id) + 1 : 1;
        trainers.Add(trainer);
        Save();
    }

    public void Update(Trainer trainer)
    {
        int index = trainers.FindIndex(t => t.Id == trainer.Id);
        if (index != -1)
        {
            trainers[index] = trainer;
            Save();
        }
    }

    public void Reload()
    {
        Load(); 
    }

    public void Delete(int id)
    {
        trainers.RemoveAll(t => t.Id == id);
        Save();
    }
    
    public Trainer GetByEmail(string email)
    {
        return trainers.FirstOrDefault(t => t.Email == email);
    }
}
