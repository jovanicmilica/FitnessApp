using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using FitnessApp.Models;

namespace FitnessApp.Repositories;

public class PropRepository
{
    private string filePath = "data/props.json";
    private List<Prop> props;

    public PropRepository()
    {
        Load();
    }

    private void Load()
    {
        if (!File.Exists(filePath))
        {
            props = new List<Prop>();
            return;
        }
        string json = File.ReadAllText(filePath);
        props = JsonSerializer.Deserialize<List<Prop>>(json)
                ?? new List<Prop>();
    }

    private void Save()
    {
        string json = JsonSerializer.Serialize(props,
            new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(filePath, json);
    }

    public List<Prop> GetAll() => props;

    public Prop? GetById(int id) =>
        props.FirstOrDefault(p => p.Id == id);

    public void Add(Prop prop)
    {
        prop.Id = props.Count > 0
            ? props.Max(p => p.Id) + 1
            : 1;
        props.Add(prop);
        Save();
    }

    public void Update(Prop prop)
    {
        int index = props.FindIndex(p => p.Id == prop.Id);
        if (index != -1)
        {
            props[index] = prop;
            Save();
        }
    }

    public void Delete(int id)
    {
        props.RemoveAll(p => p.Id == id);
        Save();
    }
}