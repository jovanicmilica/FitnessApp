using System;
using System.Collections.ObjectModel;
using System.Linq;
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

        var allTrainers = trainerRepository.GetAll();
        
        // Pending treneri
        var pending = allTrainers
            .Where(t => t.Status == TrainerStatus.PENDING || t.Status == TrainerStatus.REJECTED)
            .OrderBy(t => t.FirstName)
            .ToList();
        
        // Aktivni treneri - SORTIRANI PO OCENI (od najbolje ka najlošijoj)
        var active = allTrainers
            .Where(t => t.Status == TrainerStatus.ACTIVE)
            .OrderByDescending(t => t.Rating)  // ⭐ Najveća ocena prva
            .ThenBy(t => t.FirstName)          // Ako ista ocena, po imenu
            .ToList();

        foreach (var trainer in pending)
        {
            PendingTrainers.Add(trainer);
        }

        foreach (var trainer in active)
        {
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
        SelectedPendingTrainer = null;  // Resetuj selekciju
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
        SelectedPendingTrainer = null;  // Resetuj selekciju
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
        SelectedActiveTrainer = null;  // Resetuj selekciju
    }

    [RelayCommand]
    private void Logout()
    {
        CloseRequested?.Invoke();
    }

    public event Action? CloseRequested;
}