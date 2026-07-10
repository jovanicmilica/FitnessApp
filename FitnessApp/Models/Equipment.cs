namespace FitnessApp.Models;

public class Equipment
{
    private int id;
    private string name;
    private string description;
    private int quantity;

    public int Id { get => id; set => id = value; }
    public string Name { get => name; set => name = value; }
    public string Description { get => description; set => description = value; }
    public int Quantity { get => quantity; set => quantity = value; }
}