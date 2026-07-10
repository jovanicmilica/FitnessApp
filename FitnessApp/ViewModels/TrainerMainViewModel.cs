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
    
    // Equipment
    private readonly EquipmentRepository equipmentRepository;
    private int? editingEquipmentId = null;

    [ObservableProperty]
    private ObservableCollection<Equipment> equipmentList = new();

    [ObservableProperty]
    private Equipment? selectedEquipment;

    [ObservableProperty]
    private string equipmentName = string.Empty;

    [ObservableProperty]
    private string equipmentDescription = string.Empty;

    [ObservableProperty]
    private decimal equipmentQuantity;

// Props
    private readonly PropRepository propRepository;
    private int? editingPropId = null;

    [ObservableProperty]
    private ObservableCollection<Prop> propList = new();

    [ObservableProperty]
    private Prop? selectedProp;

    [ObservableProperty]
    private string propName = string.Empty;

    [ObservableProperty]
    private string propDescription = string.Empty;

    [ObservableProperty]
    private decimal propQuantity;

// Exercise CRUD
    private int? editingExerciseId = null;

    [ObservableProperty]
    private Exercise? selectedExerciseForCrud;

    [ObservableProperty]
    private string exerciseName = string.Empty;

    [ObservableProperty]
    private string exerciseDescription = string.Empty;

    [ObservableProperty]
    private string exerciseVideoUrl = string.Empty;

    [ObservableProperty]
    private decimal exerciseDuration;

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
        equipmentRepository = new EquipmentRepository();
        propRepository = new PropRepository();

        LoadPendingRequests();
        LoadActiveTutelages();
        LoadExercises();
        LoadFeedbacks();
        UpdateSubscriptionStatus();
        LoadEquipment();
        LoadProps();
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
    
    private void LoadEquipment()
    {
        EquipmentList.Clear();
        foreach (var e in equipmentRepository.GetAll())
            EquipmentList.Add(e);
    }

    private void LoadProps()
    {
        PropList.Clear();
        foreach (var p in propRepository.GetAll())
            PropList.Add(p);
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
    private void EditEquipment()
    {
        if (SelectedEquipment == null) return;
        editingEquipmentId = SelectedEquipment.Id;
        EquipmentName = SelectedEquipment.Name;
        EquipmentDescription = SelectedEquipment.Description;
        EquipmentQuantity = SelectedEquipment.Quantity;
    }

    [RelayCommand]
    private void DeleteEquipment()
    {
        if (SelectedEquipment == null)
        {
            ErrorMessage = "Please select equipment to delete.";
            return;
        }
        equipmentRepository.Delete(SelectedEquipment.Id);
        LoadEquipment();
        ClearEquipmentForm();
        StatusMessage = "Equipment deleted.";
    }

    [RelayCommand]
    private void SaveEquipment()
    {
        if (string.IsNullOrEmpty(EquipmentName))
        {
            ErrorMessage = "Please enter equipment name.";
            return;
        }

        if (editingEquipmentId.HasValue)
        {
            // Update
            Equipment existing = equipmentRepository.GetById(editingEquipmentId.Value);
            if (existing != null)
            {
                existing.Name = EquipmentName;
                existing.Description = EquipmentDescription;
                existing.Quantity = (int)EquipmentQuantity;
                equipmentRepository.Update(existing);
                StatusMessage = "Equipment updated.";
            }
            editingEquipmentId = null;
        }
        else
        {
            // Add
            Equipment newEquipment = new Equipment
            {
                Name = EquipmentName,
                Description = EquipmentDescription,
                Quantity = (int)EquipmentQuantity
            };
            equipmentRepository.Add(newEquipment);
            StatusMessage = "Equipment added.";
        }

        LoadEquipment();
        ClearEquipmentForm();
        ErrorMessage = string.Empty;
    }

    [RelayCommand]
    private void ClearEquipmentForm()
    {
        editingEquipmentId = null;
        EquipmentName = string.Empty;
        EquipmentDescription = string.Empty;
        EquipmentQuantity = 0;
        SelectedEquipment = null;
    }
    
    [RelayCommand]
    private void EditProp()
    {
        if (SelectedProp == null) return;
        editingPropId = SelectedProp.Id;
        PropName = SelectedProp.Name;
        PropDescription = SelectedProp.Description;
        PropQuantity = SelectedProp.Quantity;
    }

    [RelayCommand]
    private void DeleteProp()
    {
        if (SelectedProp == null)
        {
            ErrorMessage = "Please select a prop to delete.";
            return;
        }
        propRepository.Delete(SelectedProp.Id);
        LoadProps();
        ClearPropForm();
        StatusMessage = "Prop deleted.";
    }

    [RelayCommand]
    private void SaveProp()
    {
        if (string.IsNullOrEmpty(PropName))
        {
            ErrorMessage = "Please enter prop name.";
            return;
        }

        if (editingPropId.HasValue)
        {
            Prop existing = propRepository.GetById(editingPropId.Value);
            if (existing != null)
            {
                existing.Name = PropName;
                existing.Description = PropDescription;
                existing.Quantity = (int)PropQuantity;
                propRepository.Update(existing);
                StatusMessage = "Prop updated.";
            }
            editingPropId = null;
        }
        else
        {
            Prop newProp = new Prop
            {
                Name = PropName,
                Description = PropDescription,
                Quantity = (int)PropQuantity
            };
            propRepository.Add(newProp);
            StatusMessage = "Prop added.";
        }

        LoadProps();
        ClearPropForm();
        ErrorMessage = string.Empty;
    }

    [RelayCommand]
    private void ClearPropForm()
    {
        editingPropId = null;
        PropName = string.Empty;
        PropDescription = string.Empty;
        PropQuantity = 0;
        SelectedProp = null;
    }
    
    [RelayCommand]
private void EditExercise()
{
    if (SelectedExerciseForCrud == null) return;
    editingExerciseId = SelectedExerciseForCrud.Id;
    ExerciseName = SelectedExerciseForCrud.Name;
    ExerciseDescription = SelectedExerciseForCrud.Description;
    ExerciseVideoUrl = SelectedExerciseForCrud.VideoUrl;
    ExerciseDuration = SelectedExerciseForCrud.Duration;
}

[RelayCommand]
private void DeleteExercise()
{
    if (SelectedExerciseForCrud == null)
    {
        ErrorMessage = "Please select an exercise to delete.";
        return;
    }
    exerciseRepository.Delete(SelectedExerciseForCrud.Id);
    LoadExercises();
    ClearExerciseForm();
    StatusMessage = "Exercise deleted.";
}

[RelayCommand]
private void SaveExercise()
{
    if (string.IsNullOrEmpty(ExerciseName))
    {
        ErrorMessage = "Please enter exercise name.";
        return;
    }

    if (editingExerciseId.HasValue)
    {
        Exercise? existing = exerciseRepository.GetById(editingExerciseId.Value);
        if (existing != null)
        {
            existing.Name = ExerciseName;
            existing.Description = ExerciseDescription;
            existing.VideoUrl = ExerciseVideoUrl;
            existing.Duration = (int)ExerciseDuration;
            exerciseRepository.Update(existing);
            StatusMessage = "Exercise updated.";
        }
        editingExerciseId = null;
    }
    else
    {
        Exercise newExercise = new Exercise(
            ExerciseName,
            ExerciseDescription,
            ExerciseVideoUrl,
            (int)ExerciseDuration
        );
        exerciseRepository.Add(newExercise);
        StatusMessage = "Exercise added.";
    }

    LoadExercises();
    ClearExerciseForm();
    ErrorMessage = string.Empty;
}

[RelayCommand]
private void ClearExerciseForm()
{
    editingExerciseId = null;
    ExerciseName = string.Empty;
    ExerciseDescription = string.Empty;
    ExerciseVideoUrl = string.Empty;
    ExerciseDuration = 0;
    SelectedExerciseForCrud = null;
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
        LogoutRequested?.Invoke();
    }

    public event Action? LogoutRequested;
    public event Action? CloseRequested;
}