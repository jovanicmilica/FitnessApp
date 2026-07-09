namespace FitnessApp.Models;

using System;
using System.Collections.Generic;
using FitnessApp.Enums; 

public class Questionnaire
{
    private int id;
    private List<string> goals;
    private string locationPreference;
    private string timePreference;
    private string healthIssues;
    private int tutelageId;

    public int Id { get => id; set => id = value; }
    public List<string> Goals { get => goals; set => goals = value; }
    public string LocationPreference { get => locationPreference; set => locationPreference = value; }
    public string TimePreference { get => timePreference; set => timePreference = value; }
    public string HealthIssues { get => healthIssues; set => healthIssues = value; }
    public int TutelageId { get => tutelageId; set => tutelageId = value; }

    public Questionnaire(List<string> goals, string locationPreference, string timePreference, string healthIssues)
    {
        this.goals = goals;
        this.locationPreference = locationPreference;
        this.timePreference = timePreference;
        this.healthIssues = healthIssues;
    }
}