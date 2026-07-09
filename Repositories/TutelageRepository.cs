namespace FitnessApp.Repositories;

using System.Text.Json;
using FitnessApp.Models;

public class TutelageRepository
{
    private string filePath = "data/tutelages.json";
    private List<Tutelage> tutelages;

    public TutelageRepository()
    {
        Load();
    }

    private void Load()
    {
        if (!File.Exists(filePath))
        {
            tutelages = new List<Tutelage>();
            return;
        }
        string json = File.ReadAllText(filePath);
        tutelages = JsonSerializer.Deserialize<List<Tutelage>>(json) ?? new List<Tutelage>();
    }

    private void Save()
    {
        string json = JsonSerializer.Serialize(tutelages, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(filePath, json);
    }

    public List<Tutelage> GetAll()
    {
        return tutelages;
    }

    public Tutelage GetById(int id)
    {
        return tutelages.FirstOrDefault(t => t.Id == id);
    }

    public void Add(Tutelage tutelage)
    {
        tutelage.Id = tutelages.Count > 0 ? tutelages.Max(t => t.Id) + 1 : 1;
        tutelages.Add(tutelage);
        Save();
    }

    public void Update(Tutelage tutelage)
    {
        int index = tutelages.FindIndex(t => t.Id == tutelage.Id);
        if (index != -1)
        {
            tutelages[index] = tutelage;
            Save();
        }
    }

    public void Delete(int id)
    {
        tutelages.RemoveAll(t => t.Id == id);
        Save();
    }
}
