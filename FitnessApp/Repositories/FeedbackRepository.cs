namespace FitnessApp.Repositories;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using FitnessApp.Models;

public class FeedbackRepository
{
    private string filePath = "data/feedbacks.json";
    private List<Feedback> feedbacks;

    public FeedbackRepository()
    {
        Load();
    }

    private void Load()
    {
        if (!File.Exists(filePath))
        {
            feedbacks = new List<Feedback>();
            return;
        }
        string json = File.ReadAllText(filePath);
        feedbacks = JsonSerializer.Deserialize<List<Feedback>>(json) ?? new List<Feedback>();
    }

    private void Save()
    {
        string json = JsonSerializer.Serialize(feedbacks, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(filePath, json);
    }

    public List<Feedback> GetAll()
    {
        return feedbacks;
    }

    public Feedback GetById(int id)
    {
        return feedbacks.FirstOrDefault(f => f.Id == id);
    }

    public void Add(Feedback feedback)
    {
        feedback.Id = feedbacks.Count > 0 ? feedbacks.Max(f => f.Id) + 1 : 1;
        feedbacks.Add(feedback);
        Save();
    }

    public void Update(Feedback feedback)
    {
        int index = feedbacks.FindIndex(f => f.Id == feedback.Id);
        if (index != -1)
        {
            feedbacks[index] = feedback;
            Save();
        }
    }

    public void Delete(int id)
    {
        feedbacks.RemoveAll(f => f.Id == id);
        Save();
    }
}
