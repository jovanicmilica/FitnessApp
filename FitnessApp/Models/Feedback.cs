namespace FitnessApp.Models;

using System;
using System.Collections.Generic;
using FitnessApp.Enums; 

public class Feedback
{
    private int id;
    private int rating;
    private string comment;
    private FeedbackTargetType targetType;
    private int targetId;
    private int clientId;

    public int Id { get => id; set => id = value; }
    public int Rating { get => rating; set => rating = value; }
    public string Comment { get => comment; set => comment = value; }
    public FeedbackTargetType TargetType { get => targetType; set => targetType = value; }
    public int TargetId { get => targetId; set => targetId = value; }
    public int ClientId { get => clientId; set => clientId = value; }

    public Feedback(int rating, string comment, FeedbackTargetType targetType, int targetId, int clientId)
    {
        this.rating = rating;
        this.comment = comment;
        this.targetType = targetType;
        this.targetId = targetId;
        this.clientId = clientId;
    }
}