namespace FitnessApp.Repositories;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using FitnessApp.Models;

public class UserRepository
{
    private string filePath = "data/users.json";
    private List<User> users;

    public UserRepository()
    {
        Load();
    }

    private void Load()
    {
        if (!File.Exists(filePath))
        {
            users = new List<User>();
            return;
        }
        string json = File.ReadAllText(filePath);
        users = JsonSerializer.Deserialize<List<User>>(json) ?? new List<User>();
    }

    private void Save()
    {
        string json = JsonSerializer.Serialize(users, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(filePath, json);
    }

    public List<User> GetAll()
    {
        return users;
    }

    public User GetById(int id)
    {
        return users.FirstOrDefault(u => u.Id == id);
    }

    public void Add(User user)
    {
        user.Id = users.Count > 0 ? users.Max(u => u.Id) + 1 : 1;
        users.Add(user);
        Save();
    }

    public void Update(User user)
    {
        int index = users.FindIndex(u => u.Id == user.Id);
        if (index != -1)
        {
            users[index] = user;
            Save();
        }
    }

    public void Delete(int id)
    {
        users.RemoveAll(u => u.Id == id);
        Save();
    }
}
