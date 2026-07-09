namespace FitnessApp.Models;

using System;
using System.Collections.Generic;

using FitnessApp.Enums; 
public class Tutelage
{
    private int id;
    private TutelageStatus status;
    private DateTime startDate;
    private DateTime endDate;
    private int clientId;
    private int trainerId;
    private int questionnaireId;

    public int Id { get => id; set => id = value; }
    public TutelageStatus Status { get => status; set => status = value; }
    public DateTime StartDate { get => startDate; set => startDate = value; }
    public DateTime EndDate { get => endDate; set => endDate = value; }
    public int ClientId { get => clientId; set => clientId = value; }
    public int TrainerId { get => trainerId; set => trainerId = value; }
    public int QuestionnaireId { get => questionnaireId; set => questionnaireId = value; }

    public Tutelage(int clientId, int trainerId, int questionnaireId)
    {
        this.clientId = clientId;
        this.trainerId = trainerId;
        this.questionnaireId = questionnaireId;
        this.status = TutelageStatus.REQUESTED;
    }

    public void Approve() { }
    public void Reject() { }
    public void Complete() { }
    public bool CheckExpiration() { return false; }
}