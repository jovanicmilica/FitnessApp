namespace FitnessApp.Repositories;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using FitnessApp.Models;

public class ClientRepository
{
    private string filePath = "data/clients.json";
    private List<Client> clients;

    public ClientRepository()
    {
        Load();
    }

    private void Load()
    {
        if (!File.Exists(filePath))
        {
            clients = new List<Client>();
            return;
        }
        string json = File.ReadAllText(filePath);
        clients = JsonSerializer.Deserialize<List<Client>>(json) ?? new List<Client>();
    }

    private void Save()
    {
        string json = JsonSerializer.Serialize(clients, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(filePath, json);
    }

    public List<Client> GetAll()
    {
        return clients;
    }

    public Client GetById(int id)
    {
        return clients.FirstOrDefault(c => c.Id == id);
    }

    public void Add(Client client)
    {
        client.Id = clients.Count > 0 ? clients.Max(c => c.Id) + 1 : 1;
        clients.Add(client);
        Save();
    }

    public void Update(Client client)
    {
        int index = clients.FindIndex(c => c.Id == client.Id);
        if (index != -1)
        {
            clients[index] = client;
            Save();
        }
    }

    public void Delete(int id)
    {
        clients.RemoveAll(c => c.Id == id);
        Save();
    }
    
    public Client GetByEmail(string email)
    {
        return clients.FirstOrDefault(c => c.Email == email);
    }
}