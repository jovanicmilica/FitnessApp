namespace FitnessApp.Repositories;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using FitnessApp.Models;

public class AdminRepository
{
    private string filePath = "data/admins.json";
    private List<Administrator> admins;

    public AdminRepository()
    {
        Load();
    }

    private void Load()
    {
        if (!File.Exists(filePath))
        {
            admins = new List<Administrator>();
            return;
        }
        string json = File.ReadAllText(filePath);
        admins = JsonSerializer.Deserialize<List<Administrator>>(json) ?? new List<Administrator>();
    }

    private void Save()
    {
        string json = JsonSerializer.Serialize(admins, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(filePath, json);
    }

    public List<Administrator> GetAll()
    {
        return admins;
    }

    public Administrator GetById(int id)
    {
        return admins.FirstOrDefault(a => a.Id == id);
    }

    public void Add(Administrator admin)
    {
        admin.Id = admins.Count > 0 ? admins.Max(a => a.Id) + 1 : 1;
        admins.Add(admin);
        Save();
    }

    public void Update(Administrator admin)
    {
        int index = admins.FindIndex(a => a.Id == admin.Id);
        if (index != -1)
        {
            admins[index] = admin;
            Save();
        }
    }

    public void Delete(int id)
    {
        admins.RemoveAll(a => a.Id == id);
        Save();
    }
}
