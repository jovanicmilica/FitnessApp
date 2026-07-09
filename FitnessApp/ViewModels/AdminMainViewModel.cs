using System;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FitnessApp.Models;
using FitnessApp.Repositories;
using FitnessApp.Enums;

namespace FitnessApp.ViewModels;

public partial class AdminMainViewModel : ObservableObject
{
    [ObservableProperty]
    private ObservableCollection<Trainer> pendingTrainers = new();

    [ObservableProperty]
    private ObservableCollection<Trainer> activeTrainers = new();

    [ObservableProperty]
    private Trainer? selectedPendingTrainer;

    [ObservableProperty]
    private Trainer? selectedActiveTrainer;

    [ObservableProperty]
    private string statusMessage = string.Empty;

    private readonly TrainerRepository trainerRepository;
    private readonly QualificationRepository qualificationRepository;

    public AdminMainViewModel(Administrator admin)
    {
        trainerRepository = new TrainerRepository();
        qualificationRepository = new QualificationRepository();
        LoadTrainers();
    }

    private void LoadTrainers()
    {
        PendingTrainers.Clear();
        ActiveTrainers.Clear();

        foreach (var trainer in trainerRepository.GetAll())
        {
            if (trainer.Status == TrainerStatus.PENDING)
                PendingTrainers.Add(trainer);
            else if (trainer.Status == TrainerStatus.ACTIVE)
                ActiveTrainers.Add(trainer);
        }
    }

    [RelayCommand]
    private void ApproveTrainer()
    {
        if (SelectedPendingTrainer == null)
        {
            StatusMessage = "Please select a trainer to approve.";
            return;
        }

        SelectedPendingTrainer.Status = TrainerStatus.ACTIVE;
        trainerRepository.Update(SelectedPendingTrainer);

        StatusMessage = $"{SelectedPendingTrainer.FirstName} {SelectedPendingTrainer.LastName} has been approved.";
        LoadTrainers();
    }

    [RelayCommand]
    private void RejectTrainer()
    {
        if (SelectedPendingTrainer == null)
        {
            StatusMessage = "Please select a trainer to reject.";
            return;
        }

        SelectedPendingTrainer.Status = TrainerStatus.REJECTED;
        trainerRepository.Update(SelectedPendingTrainer);

        StatusMessage = $"{SelectedPendingTrainer.FirstName} {SelectedPendingTrainer.LastName} has been rejected.";
        LoadTrainers();
    }

    [RelayCommand]
    private void RemoveTrainer()
    {
        if (SelectedActiveTrainer == null)
        {
            StatusMessage = "Please select a trainer to remove.";
            return;
        }

        SelectedActiveTrainer.Status = TrainerStatus.REMOVED;
        trainerRepository.Update(SelectedActiveTrainer);

        StatusMessage = $"{SelectedActiveTrainer.FirstName} {SelectedActiveTrainer.LastName} has been removed.";
        LoadTrainers();
    }

    [RelayCommand]
    private void Logout()
    {
        CloseRequested?.Invoke();
    }

    public event Action? CloseRequested;
}