namespace FitnessApp.Models;

using System;
using System.Collections.Generic;
using FitnessApp.Enums; 

public class Administrator : User
{
    public Trainer ReviewTrainerApplication(int trainerId) { return null; }
    public void ApproveTrainer(int trainerId) { }
    public void RejectTrainer(int trainerId) { }
    public void RemoveTrainer(int trainerId) { }
}