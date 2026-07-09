namespace FitnessApp.Models;

using System;
using System.Collections.Generic;
using FitnessApp.Enums; 

public class Exercise
{
    private int id;
    private string name;
    private string description;
    private string videoUrl;
    private int duration;

    public int Id { get => id; set => id = value; }
    public string Name { get => name; set => name = value; }
    public string Description { get => description; set => description = value; }
    public string VideoUrl { get => videoUrl; set => videoUrl = value; }
    public int Duration { get => duration; set => duration = value; }

    public Exercise(string name, string description, string videoUrl, int duration)
    {
        this.name = name;
        this.description = description;
        this.videoUrl = videoUrl;
        this.duration = duration;
    }
}