using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FitnessApp.Models;
using FitnessApp.Repositories;
using FitnessApp.Enums;

namespace FitnessApp.ViewModels;

public partial class ClientMainViewModel : ObservableObject
{
    // Trenutno ulogovani klijent
    private readonly Client currentClient;

    // Repozitorijumi
    private readonly TrainerRepository trainerRepository;
    private readonly TutelageRepository tutelageRepository;
    private readonly QuestionnaireRepository questionnaireRepository;
    private readonly FeedbackRepository feedbackRepository;
    private readonly PaymentRepository paymentRepository;
    private readonly WorkoutRepository workoutRepository;
    private readonly WorkoutExerciseRepository workoutExerciseRepository;
    private readonly ExerciseRepository exerciseRepository;

    // Pretraga trenera
    [ObservableProperty]
    private ObservableCollection<Trainer> availableTrainers = new();

    [ObservableProperty]
    private Trainer? selectedTrainer;

    [ObservableProperty]
    private string searchCriteria = string.Empty;

    // Mentorstva
    [ObservableProperty]
    private ObservableCollection<Tutelage> myTutelages = new();

    [ObservableProperty]
    private Tutelage? selectedTutelage;

    // Upitnik
    [ObservableProperty]
    private string goals = string.Empty;

    [ObservableProperty]
    private string locationPreference = string.Empty;

    [ObservableProperty]
    private string timePreference = string.Empty;

    [ObservableProperty]
    private string healthIssues = string.Empty;

    // Feedback
    [ObservableProperty]
    private int feedbackRating = 5;

    [ObservableProperty]
    private string feedbackComment = string.Empty;

    // Status poruke
    [ObservableProperty]
    private string statusMessage = string.Empty;

    [ObservableProperty]
    private string errorMessage = string.Empty;
    
    // Profil korisnika
    [ObservableProperty]
    private ClientProfileViewModel? _profileViewModel;

    // Workout
    [ObservableProperty]
    private ObservableCollection<Workout> myWorkouts = new();

    [ObservableProperty]
    private Workout? selectedWorkout;

    [ObservableProperty]
    private ObservableCollection<WorkoutExercise> selectedWorkoutExercises = new();

    [ObservableProperty]
    private string selectedWorkoutDetails = string.Empty;

    public ClientMainViewModel(Client client)
    {
        currentClient = client;
        trainerRepository = new TrainerRepository();
        tutelageRepository = new TutelageRepository();
        questionnaireRepository = new QuestionnaireRepository();
        feedbackRepository = new FeedbackRepository();
        paymentRepository = new PaymentRepository();
        workoutRepository = new WorkoutRepository();
        workoutExerciseRepository = new WorkoutExerciseRepository();
        exerciseRepository = new ExerciseRepository();
        ProfileViewModel = new ClientProfileViewModel(client);

        LoadTrainers();
        LoadMyTutelages();
        LoadMyWorkouts();
    }

    private void LoadTrainers()
    {
        AvailableTrainers.Clear();
        foreach (var trainer in trainerRepository.GetAll())
        {
            if (trainer.Status == TrainerStatus.ACTIVE)
                AvailableTrainers.Add(trainer);
        }
    }

    private void LoadMyTutelages()
    {
        MyTutelages.Clear();
        foreach (var tutelage in tutelageRepository.GetAll())
        {
            if (tutelage.ClientId == currentClient.Id)
                MyTutelages.Add(tutelage);
        }
    }

    private void LoadMyWorkouts()
    {
        MyWorkouts.Clear();
        
        var clientTutelages = tutelageRepository.GetAll()
            .Where(t => t.ClientId == currentClient.Id)
            .Select(t => t.Id)
            .ToList();
        
        var allWorkouts = workoutRepository.GetAll();
        var clientWorkouts = allWorkouts
            .Where(w => clientTutelages.Contains(w.TutelageId))
            .OrderByDescending(w => w.DateCreated)
            .ToList();
        
        foreach (var workout in clientWorkouts)
        {
            MyWorkouts.Add(workout);
        }
    }

    [RelayCommand]
    private void SearchTrainers()
    {
        AvailableTrainers.Clear();
        var all = trainerRepository.GetAll();

        foreach (var trainer in all)
        {
            if (trainer.Status != TrainerStatus.ACTIVE)
                continue;

            if (!string.IsNullOrEmpty(SearchCriteria))
            {
                bool matchesName = trainer.FirstName.Contains(SearchCriteria, StringComparison.OrdinalIgnoreCase) ||
                                   trainer.LastName.Contains(SearchCriteria, StringComparison.OrdinalIgnoreCase);
                if (!matchesName) continue;
            }

            AvailableTrainers.Add(trainer);
        }

        StatusMessage = $"Found {AvailableTrainers.Count} trainer(s).";
    }

    [RelayCommand]
    private void SendTutelageRequest()
    {
        ErrorMessage = string.Empty;
        StatusMessage = string.Empty;

        if (SelectedTrainer == null)
        {
            ErrorMessage = "Please select a trainer.";
            return;
        }

        if (string.IsNullOrEmpty(Goals) || string.IsNullOrEmpty(LocationPreference) ||
            string.IsNullOrEmpty(TimePreference))
        {
            ErrorMessage = "Please fill in all questionnaire fields.";
            return;
        }

        bool alreadyExists = tutelageRepository.GetAll()
            .Any(t => t.ClientId == currentClient.Id &&
                      t.TrainerId == SelectedTrainer.Id &&
                      (t.Status == TutelageStatus.REQUESTED || t.Status == TutelageStatus.ACTIVE));

        if (alreadyExists)
        {
            ErrorMessage = "You already have an active or pending request with this trainer.";
            return;
        }

        Questionnaire questionnaire = new Questionnaire(
            new List<string> { Goals },
            LocationPreference,
            TimePreference,
            HealthIssues
        );
        questionnaireRepository.Add(questionnaire);

        Tutelage tutelage = new Tutelage(
            currentClient.Id,
            SelectedTrainer.Id,
            questionnaire.Id
        );
        tutelageRepository.Add(tutelage);

        StatusMessage = $"Request sent to {SelectedTrainer.FirstName} {SelectedTrainer.LastName}!";

        Goals = string.Empty;
        LocationPreference = string.Empty;
        TimePreference = string.Empty;
        HealthIssues = string.Empty;
        SelectedTrainer = null;

        LoadMyTutelages();
    }

    [RelayCommand]
    private void PayForTutelage()
    {
        if (SelectedTutelage == null)
        {
            ErrorMessage = "Please select a tutelage to pay for.";
            return;
        }

        if (SelectedTutelage.Status != TutelageStatus.ACTIVE)
        {
            ErrorMessage = "You can only pay for active tutelages.";
            return;
        }

        Payment payment = new Payment(
            trainerRepository.GetById(SelectedTutelage.TrainerId)?.PricePerMonth ?? 0,
            PaymentType.TUTELAGE_PAYMENT
        )
        {
            TrainerId = SelectedTutelage.TrainerId,
            TutelageId = SelectedTutelage.Id
        };

        paymentRepository.Add(payment);

        SelectedTutelage.EndDate = DateTime.Now.AddMonths(1);
        tutelageRepository.Update(SelectedTutelage);

        StatusMessage = "Payment successful! Tutelage extended by one month.";
        LoadMyTutelages();
    }

    [RelayCommand]
    private void LeaveFeedback()
    {
        if (SelectedTutelage == null)
        {
            ErrorMessage = "Please select a tutelage first.";
            return;
        }

        if (string.IsNullOrEmpty(FeedbackComment))
        {
            ErrorMessage = "Please enter a comment.";
            return;
        }

        if (FeedbackRating < 1 || FeedbackRating > 5)
        {
            ErrorMessage = "Rating must be between 1 and 5.";
            return;
        }

        Feedback feedback = new Feedback(
            FeedbackRating,
            FeedbackComment,
            FeedbackTargetType.TRAINER,
            SelectedTutelage.TrainerId,
            currentClient.Id
        );

        feedbackRepository.Add(feedback);

        StatusMessage = "Feedback submitted successfully!";
        FeedbackComment = string.Empty;
        FeedbackRating = 5;
    }

    [RelayCommand]
    private void ViewWorkoutDetails()
    {
        if (SelectedWorkout == null)
        {
            ErrorMessage = "Please select a workout.";
            return;
        }
        
        SelectedWorkoutExercises.Clear();
        var exercises = workoutExerciseRepository.GetAll()
            .Where(we => we.WorkoutId == SelectedWorkout.Id)
            .ToList();
        
        foreach (var exercise in exercises)
        {
            SelectedWorkoutExercises.Add(exercise);
        }
        
        var exerciseNames = string.Join(", ", SelectedWorkoutExercises.Select(e => 
        {
            var ex = exerciseRepository.GetById(e.ExerciseId);
            return ex?.Name ?? "Unknown";
        }));
        
        var statusText = SelectedWorkout.Status == WorkoutStatus.COMPLETED ? "Completed" : "Active";
        SelectedWorkoutDetails = $"Status: {statusText}\nCreated: {SelectedWorkout.DateCreated:dd/MM/yyyy}\nExercises: {exerciseNames}";
    }

    [RelayCommand]
    private void Logout()
    {
        LogoutRequested?.Invoke();
    }

    public event Action? CloseRequested;
    public event Action? LogoutRequested;
}