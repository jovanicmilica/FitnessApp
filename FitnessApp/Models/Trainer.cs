namespace FitnessApp.Models;

using System;
using System.Collections.Generic;
using FitnessApp.Enums; 

public class Trainer : User
{
    private decimal pricePerMonth;
    private TrainerStatus status;
    private double rating;

    public decimal PricePerMonth { get => pricePerMonth; set => pricePerMonth = value; }
    public TrainerStatus Status { get => status; set => status = value; }
    public double Rating { get => rating; set => rating = value; }

    public void SubmitRegistration(List<int> qualificationIds) { }
    public Questionnaire ReviewTutelageRequest(int tutelageId) { return null; }
    public void RespondToTutelageRequest(int tutelageId, bool approve) { }
    public List<Exercise> BrowseExercises() { return null; }
    public Workout CreateWorkout(int clientId, List<int> exerciseIds) { return null; }
    public void PaySubscription(decimal amount) { }
    public void SendMessage(string content, int tutelageId) { }
    public double CalculateRating() { return 0; }
    public double GetDebt() { return 0; }
}