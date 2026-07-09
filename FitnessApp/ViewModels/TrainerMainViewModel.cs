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

public partial class TrainerMainViewModel : ObservableObject
{
    private readonly Trainer currentTrainer;

    private readonly TutelageRepository tutelageRepository;
    private readonly QuestionnaireRepository questionnaireRepository;
    private readonly WorkoutRepository workoutRepository;
    private readonly WorkoutExerciseRepository workoutExerciseRepository;
    private readonly ExerciseRepository exerciseRepository;
    private readonly PaymentRepository paymentRepository;
    private readonly ClientRepository clientRepository;
    private readonly FeedbackRepository feedbackRepository;

    // Pending zahtjevi
    [ObservableProperty]
    private ObservableCollection<Tutelage> pendingRequests = new();

    [ObservableProperty]
    private Tutelage? selectedPendingRequest;

    [ObservableProperty]
    private Questionnaire? selectedQuestionnaire;

    // Aktivna mentorstva
    [ObservableProperty]
    private ObservableCollection<Tutelage> activeTutelages = new();

    [ObservableProperty]
    private Tutelage? selectedActiveTutelage;

    // Kreiranje treninga
    [ObservableProperty]
    private ObservableCollection<Exercise> availableExercises = new();

    [ObservableProperty]
    private ObservableCollection<Exercise> selectedExercises = new();

    [ObservableProperty]
    private Exercise? selectedAvailableExercise;

    [ObservableProperty]
    private string workoutName = string.Empty;

    // Feedback pregled
    [ObservableProperty]
    private ObservableCollection<Feedback> myFeedbacks = new();

    // Plaćanje pretplate
    [ObservableProperty]
    private string subscriptionStatus = string.Empty;

    [ObservableProperty]
    private bool isSubscriptionPaid = false;

    // Status poruke
    [ObservableProperty]
    private string statusMessage = string.Empty;

    [ObservableProperty]
    private string errorMessage = string.Empty;

    public TrainerMainViewModel(Trainer trainer)
    {
        currentTrainer = trainer;
        tutelageRepository = new TutelageRepository();
        questionnaireRepository = new QuestionnaireRepository();
        workoutRepository = new WorkoutRepository();
        workoutExerciseRepository = new WorkoutExerciseRepository();
        exerciseRepository = new ExerciseRepository();
        paymentRepository = new PaymentRepository();
        clientRepository = new ClientRepository();
        feedbackRepository = new FeedbackRepository();

        LoadPendingRequests();
        LoadActiveTutelages();
        LoadExercises();
        LoadFeedbacks();
        UpdateSubscriptionStatus();
    }

    private void LoadPendingRequests()
    {
        PendingRequests.Clear();
        foreach (var tutelage in tutelageRepository.GetAll())
        {
            if (tutelage.TrainerId == currentTrainer.Id &&
                tutelage.Status == TutelageStatus.REQUESTED)
                PendingRequests.Add(tutelage);
        }
    }

    private void LoadActiveTutelages()
    {
        ActiveTutelages.Clear();
        foreach (var tutelage in tutelageRepository.GetAll())
        {
            if (tutelage.TrainerId == currentTrainer.Id &&
                tutelage.Status == TutelageStatus.ACTIVE)
                ActiveTutelages.Add(tutelage);
        }
    }

    private void LoadExercises()
    {
        AvailableExercises.Clear();
        foreach (var exercise in exerciseRepository.GetAll())
            AvailableExercises.Add(exercise);
    }

    private void LoadFeedbacks()
    {
        MyFeedbacks.Clear();
        foreach (var feedback in feedbackRepository.GetAll())
        {
            if (feedback.TargetType == FeedbackTargetType.TRAINER &&
                feedback.TargetId == currentTrainer.Id)
                MyFeedbacks.Add(feedback);
        }
    }

    private void UpdateSubscriptionStatus()
    {
        var latestPayment = paymentRepository.GetAll()
            .Where(p => p.TrainerId == currentTrainer.Id &&
                        p.Type == PaymentType.TRAINER_SUBSCRIPTION)
            .OrderByDescending(p => p.Date)
            .FirstOrDefault();

        if (latestPayment == null)
        {
            SubscriptionStatus = "No subscription payment found. Please pay to keep your account active.";
            IsSubscriptionPaid = false;
        }
        else
        {
            // Proveri da li je plaćanje već urađeno danasnje
            if (latestPayment.Date.Date == DateTime.Now.Date)
            {
                SubscriptionStatus = $"✓ Paid today ({latestPayment.Date:dd/MM/yyyy HH:mm})";
                IsSubscriptionPaid = true;
            }
            else
            {
                var nextDue = latestPayment.Date.AddMonths(1);
                SubscriptionStatus = $"Last payment: {latestPayment.Date:dd/MM/yyyy} | Next due: {nextDue:dd/MM/yyyy}";
                IsSubscriptionPaid = false;
            }
        }
    }

    // Pregled upitnika za odabrani zahtjev
    partial void OnSelectedPendingRequestChanged(Tutelage? value)
    {
        if (value == null)
        {
            SelectedQuestionnaire = null;
            return;
        }
        SelectedQuestionnaire = questionnaireRepository.GetById(value.QuestionnaireId);
    }

    [RelayCommand]
    private void ApproveRequest()
    {
        ErrorMessage = string.Empty;

        if (SelectedPendingRequest == null)
        {
            ErrorMessage = "Please select a request to approve.";
            return;
        }

        SelectedPendingRequest.Status = TutelageStatus.ACTIVE;
        SelectedPendingRequest.StartDate = DateTime.Now;
        SelectedPendingRequest.EndDate = DateTime.Now.AddMonths(1);
        tutelageRepository.Update(SelectedPendingRequest);

        StatusMessage = "Request approved successfully!";
        LoadPendingRequests();
        LoadActiveTutelages();
    }

    [RelayCommand]
    private void RejectRequest()
    {
        ErrorMessage = string.Empty;

        if (SelectedPendingRequest == null)
        {
            ErrorMessage = "Please select a request to reject.";
            return;
        }

        SelectedPendingRequest.Status = TutelageStatus.REJECTED;
        tutelageRepository.Update(SelectedPendingRequest);

        StatusMessage = "Request rejected.";
        LoadPendingRequests();
    }

    [RelayCommand]
    private void AddExerciseToWorkout()
    {
        if (SelectedAvailableExercise == null)
        {
            ErrorMessage = "Please select an exercise to add.";
            return;
        }

        if (SelectedExercises.Contains(SelectedAvailableExercise))
        {
            ErrorMessage = "This exercise is already added.";
            return;
        }

        SelectedExercises.Add(SelectedAvailableExercise);
        ErrorMessage = string.Empty;
    }

    [RelayCommand]
    private void RemoveExerciseFromWorkout()
    {
        if (SelectedAvailableExercise == null) return;
        SelectedExercises.Remove(SelectedAvailableExercise);
    }

    [RelayCommand]
    private void CreateWorkout()
    {
        ErrorMessage = string.Empty;

        if (SelectedActiveTutelage == null)
        {
            ErrorMessage = "Please select an active tutelage to create a workout for.";
            return;
        }

        if (string.IsNullOrEmpty(WorkoutName))
        {
            ErrorMessage = "Please enter a workout name.";
            return;
        }

        if (SelectedExercises.Count == 0)
        {
            ErrorMessage = "Please add at least one exercise.";
            return;
        }

        // Kreiraj workout
        Workout workout = new Workout
        {
            Name = WorkoutName,
            DateCreated = DateTime.Now,
            Status = WorkoutStatus.ASSIGNED,
            TrainerId = currentTrainer.Id,
            TutelageId = SelectedActiveTutelage.Id
        };
        workoutRepository.Add(workout);

        // Dodaj vježbe
        foreach (var exercise in SelectedExercises)
        {
            WorkoutExercise workoutExercise = new WorkoutExercise(
                exercise.Id,
                sets: 3,
                repetitions: 10,
                durationInSeconds: exercise.Duration
            )
            {
                WorkoutId = workout.Id
            };
            workoutExerciseRepository.Add(workoutExercise);
        }

        StatusMessage = $"Workout '{WorkoutName}' created successfully!";

        // Očisti formu
        WorkoutName = string.Empty;
        SelectedExercises.Clear();
        SelectedActiveTutelage = null;
    }

    [RelayCommand]
    private void PaySubscription()
    {
        if (IsSubscriptionPaid)
        {
            StatusMessage = "Subscription already paid for today.";
            return;
        }

        Payment payment = new Payment(
            amount: 20.00m,
            type: PaymentType.TRAINER_SUBSCRIPTION
        )
        {
            TrainerId = currentTrainer.Id,
            TutelageId = 0
        };

        paymentRepository.Add(payment);
        UpdateSubscriptionStatus();
        StatusMessage = "✓ Subscription paid successfully!";
    }

    [RelayCommand]
    private void Logout()
    {
        CloseRequested?.Invoke();
    }

    public event Action? CloseRequested;
}