namespace FitnessApp.Repositories;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using FitnessApp.Models;

public class QualificationRepository
{
    private string filePath = "data/qualifications.json";
    private List<Qualification> qualifications;

    public QualificationRepository()
    {
        Load();
    }

    private void Load()
    {
        if (!File.Exists(filePath))
        {
            qualifications = new List<Qualification>();
            return;
        }
        string json = File.ReadAllText(filePath);
        qualifications = JsonSerializer.Deserialize<List<Qualification>>(json) ?? new List<Qualification>();
    }

    private void Save()
    {
        string json = JsonSerializer.Serialize(qualifications, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(filePath, json);
    }

    public List<Qualification> GetAll()
    {
        return qualifications;
    }

    public Qualification GetById(int id)
    {
        return qualifications.FirstOrDefault(q => q.Id == id);
    }

    public void Add(Qualification qualification)
    {
        qualification.Id = qualifications.Count > 0 ? qualifications.Max(q => q.Id) + 1 : 1;
        qualifications.Add(qualification);
        Save();
    }

    public void Update(Qualification qualification)
    {
        int index = qualifications.FindIndex(q => q.Id == qualification.Id);
        if (index != -1)
        {
            qualifications[index] = qualification;
            Save();
        }
    }

    public void Delete(int id)
    {
        qualifications.RemoveAll(q => q.Id == id);
        Save();
    }
}
