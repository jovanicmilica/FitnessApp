using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using FitnessApp.Models;

namespace FitnessApp.Repositories;

public class EquipmentRepository
{
    private string filePath = "data/equipments.json";
    private List<Equipment> equipmentList;

    public EquipmentRepository()
    {
        Load();
    }

    private void Load()
    {
        if (!File.Exists(filePath))
        {
            equipmentList = new List<Equipment>();
            return;
        }
        string json = File.ReadAllText(filePath);
        equipmentList = JsonSerializer.Deserialize<List<Equipment>>(json)
                        ?? new List<Equipment>();
    }

    private void Save()
    {
        string json = JsonSerializer.Serialize(equipmentList,
            new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(filePath, json);
    }

    public List<Equipment> GetAll() => equipmentList;

    public Equipment? GetById(int id) =>
        equipmentList.FirstOrDefault(e => e.Id == id);

    public void Add(Equipment equipment)
    {
        equipment.Id = equipmentList.Count > 0
            ? equipmentList.Max(e => e.Id) + 1
            : 1;
        equipmentList.Add(equipment);
        Save();
    }

    public void Update(Equipment equipment)
    {
        int index = equipmentList.FindIndex(e => e.Id == equipment.Id);
        if (index != -1)
        {
            equipmentList[index] = equipment;
            Save();
        }
    }

    public void Delete(int id)
    {
        equipmentList.RemoveAll(e => e.Id == id);
        Save();
    }
}