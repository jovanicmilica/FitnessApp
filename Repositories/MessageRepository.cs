namespace FitnessApp.Repositories;

using System.Text.Json;
using FitnessApp.Models;

public class MessageRepository
{
    private string filePath = "data/messages.json";
    private List<Message> messages;

    public MessageRepository()
    {
        Load();
    }

    private void Load()
    {
        if (!File.Exists(filePath))
        {
            messages = new List<Message>();
            return;
        }
        string json = File.ReadAllText(filePath);
        messages = JsonSerializer.Deserialize<List<Message>>(json) ?? new List<Message>();
    }

    private void Save()
    {
        string json = JsonSerializer.Serialize(messages, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(filePath, json);
    }

    public List<Message> GetAll()
    {
        return messages;
    }

    public Message GetById(int id)
    {
        return messages.FirstOrDefault(m => m.Id == id);
    }

    public void Add(Message message)
    {
        message.Id = messages.Count > 0 ? messages.Max(m => m.Id) + 1 : 1;
        messages.Add(message);
        Save();
    }

    public void Update(Message message)
    {
        int index = messages.FindIndex(m => m.Id == message.Id);
        if (index != -1)
        {
            messages[index] = message;
            Save();
        }
    }

    public void Delete(int id)
    {
        messages.RemoveAll(m => m.Id == id);
        Save();
    }
}
