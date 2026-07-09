namespace FitnessApp.Repositories;

using System.Text.Json;
using FitnessApp.Models;

public class PaymentRepository
{
    private string filePath = "data/payments.json";
    private List<Payment> payments;

    public PaymentRepository()
    {
        Load();
    }

    private void Load()
    {
        if (!File.Exists(filePath))
        {
            payments = new List<Payment>();
            return;
        }
        string json = File.ReadAllText(filePath);
        payments = JsonSerializer.Deserialize<List<Payment>>(json) ?? new List<Payment>();
    }

    private void Save()
    {
        string json = JsonSerializer.Serialize(payments, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(filePath, json);
    }

    public List<Payment> GetAll()
    {
        return payments;
    }

    public Payment GetById(int id)
    {
        return payments.FirstOrDefault(p => p.Id == id);
    }

    public void Add(Payment payment)
    {
        payment.Id = payments.Count > 0 ? payments.Max(p => p.Id) + 1 : 1;
        payments.Add(payment);
        Save();
    }

    public void Update(Payment payment)
    {
        int index = payments.FindIndex(p => p.Id == payment.Id);
        if (index != -1)
        {
            payments[index] = payment;
            Save();
        }
    }

    public void Delete(int id)
    {
        payments.RemoveAll(p => p.Id == id);
        Save();
    }
}
