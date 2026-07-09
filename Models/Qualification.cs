namespace FitnessApp.Models;

public class Qualification
{
    private int id;
    private string name;
    private string institution;
    private string certificateUrl;
    private DateTime dateIssued;
    private int trainerId;

    public int Id { get => id; set => id = value; }
    public string Name { get => name; set => name = value; }
    public string Institution { get => institution; set => institution = value; }
    public string CertificateUrl { get => certificateUrl; set => certificateUrl = value; }
    public DateTime DateIssued { get => dateIssued; set => dateIssued = value; }
    public int TrainerId { get => trainerId; set => trainerId = value; }

    public Qualification(string name, string institution, string certificateUrl, DateTime dateIssued) 
    {
        this.name = name;
        this.institution = institution;
        this.certificateUrl = certificateUrl;
        this.dateIssued = dateIssued;
    }
}