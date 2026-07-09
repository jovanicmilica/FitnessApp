namespace FitnessApp.Repositories;

using System.Text.Json;
using FitnessApp.Models;

public class HealthRecordRepository
{
    private string filePath = "data/healthrecords.json";
    private List<HealthRecord> healthRecords;

    public HealthRecordRepository()
    {
        Load();
    }

    private void Load()
    {
        if (!File.Exists(filePath))
        {
            healthRecords = new List<HealthRecord>();
            return;
        }
        string json = File.ReadAllText(filePath);
        healthRecords = JsonSerializer.Deserialize<List<HealthRecord>>(json) ?? new List<HealthRecord>();
    }

    private void Save()
    {
        string json = JsonSerializer.Serialize(healthRecords, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(filePath, json);
    }

    public List<HealthRecord> GetAll()
    {
        return healthRecords;
    }

    public HealthRecord GetById(int id)
    {
        return healthRecords.FirstOrDefault(h => h.Id == id);
    }

    public void Add(HealthRecord healthRecord)
    {
        healthRecord.Id = healthRecords.Count > 0 ? healthRecords.Max(h => h.Id) + 1 : 1;
        healthRecords.Add(healthRecord);
        Save();
    }

    public void Update(HealthRecord healthRecord)
    {
        int index = healthRecords.FindIndex(h => h.Id == healthRecord.Id);
        if (index != -1)
        {
            healthRecords[index] = healthRecord;
            Save();
        }
    }

    public void Delete(int id)
    {
        healthRecords.RemoveAll(h => h.Id == id);
        Save();
    }
}
