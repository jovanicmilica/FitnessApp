namespace FitnessApp.Models;

public class Payment
{
    private int id;
    private decimal amount;
    private DateTime date;
    private PaymentType type;
    private int trainerId;
    private int tutelageId;

    public int Id { get => id; set => id = value; }
    public decimal Amount { get => amount; set => amount = value; }
    public DateTime Date { get => date; set => date = value; }
    public PaymentType Type { get => type; set => type = value; }
    public int TrainerId { get => trainerId; set => trainerId = value; }
    public int TutelageId { get => tutelageId; set => tutelageId = value; }

    public Payment(decimal amount, PaymentType type)
    {
        this.amount = amount;
        this.type = type;
        this.date = DateTime.Now;
    }
}