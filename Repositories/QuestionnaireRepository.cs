namespace FitnessApp.Repositories;

using System.Text.Json;
using FitnessApp.Models;

public class QuestionnaireRepository
{
    private string filePath = "data/questionnaires.json";
    private List<Questionnaire> questionnaires;

    public QuestionnaireRepository()
    {
        Load();
    }

    private void Load()
    {
        if (!File.Exists(filePath))
        {
            questionnaires = new List<Questionnaire>();
            return;
        }
        string json = File.ReadAllText(filePath);
        questionnaires = JsonSerializer.Deserialize<List<Questionnaire>>(json) ?? new List<Questionnaire>();
    }

    private void Save()
    {
        string json = JsonSerializer.Serialize(questionnaires, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(filePath, json);
    }

    public List<Questionnaire> GetAll()
    {
        return questionnaires;
    }

    public Questionnaire GetById(int id)
    {
        return questionnaires.FirstOrDefault(q => q.Id == id);
    }

    public void Add(Questionnaire questionnaire)
    {
        questionnaire.Id = questionnaires.Count > 0 ? questionnaires.Max(q => q.Id) + 1 : 1;
        questionnaires.Add(questionnaire);
        Save();
    }

    public void Update(Questionnaire questionnaire)
    {
        int index = questionnaires.FindIndex(q => q.Id == questionnaire.Id);
        if (index != -1)
        {
            questionnaires[index] = questionnaire;
            Save();
        }
    }

    public void Delete(int id)
    {
        questionnaires.RemoveAll(q => q.Id == id);
        Save();
    }
}
