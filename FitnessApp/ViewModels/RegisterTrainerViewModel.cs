using System;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FitnessApp.Models;
using FitnessApp.Repositories;
using FitnessApp.Enums;

namespace FitnessApp.ViewModels;

public partial class RegisterTrainerViewModel : ObservableObject
{
    [ObservableProperty]
    private string firstName = string.Empty;

    [ObservableProperty]
    private string lastName = string.Empty;

    [ObservableProperty]
    private string email = string.Empty;

    [ObservableProperty]
    private string password = string.Empty;

    [ObservableProperty]
    private string confirmPassword = string.Empty;

    [ObservableProperty]
    private decimal pricePerMonth;

    // Kvalifikacija
    [ObservableProperty]
    private string qualificationName = string.Empty;

    [ObservableProperty]
    private string qualificationInstitution = string.Empty;

    [ObservableProperty]
    private string qualificationCertificateUrl = string.Empty;

    [ObservableProperty]
    private DateTime qualificationDateIssued = DateTime.Now;

    [ObservableProperty]
    private string errorMessage = string.Empty;

    [ObservableProperty]
    private string successMessage = string.Empty;

    private readonly TrainerRepository trainerRepository;
    private readonly QualificationRepository qualificationRepository;

    public RegisterTrainerViewModel()
    {
        trainerRepository = new TrainerRepository();
        qualificationRepository = new QualificationRepository();
    }

    [RelayCommand]
    private void Register()
    {
        ErrorMessage = string.Empty;
        SuccessMessage = string.Empty;

        // Validacija osnovnih polja
        if (string.IsNullOrEmpty(FirstName) ||
            string.IsNullOrEmpty(LastName) ||
            string.IsNullOrEmpty(Email) ||
            string.IsNullOrEmpty(Password) ||
            string.IsNullOrEmpty(ConfirmPassword))
        {
            ErrorMessage = "Please fill in all fields.";
            return;
        }

        if (Password != ConfirmPassword)
        {
            ErrorMessage = "Passwords do not match.";
            return;
        }

        if (PricePerMonth <= 0)
        {
            ErrorMessage = "Please enter a valid price per month.";
            return;
        }

        // Validacija kvalifikacije
        if (string.IsNullOrEmpty(QualificationName) ||
            string.IsNullOrEmpty(QualificationInstitution))
        {
            ErrorMessage = "Please fill in at least one qualification.";
            return;
        }

        /*
        // Provjeri da li email već postoji
        Trainer existing = trainerRepository.GetByEmail(Email);
        if (existing != null)
        {
            ErrorMessage = "An account with this email already exists.";
            return;
        }

        // Kreiraj novog trenera
        Trainer newTrainer = new Trainer
        {
            FirstName = FirstName,
            LastName = LastName,
            Email = Email,
            Password = Password,
            PricePerMonth = PricePerMonth,
            Status = TrainerStatus.PENDING,
            Rating = 0.0
        };

        trainerRepository.Add(newTrainer);

        // Kreiraj kvalifikaciju i poveži sa trenerom
        Qualification qualification = new Qualification(
            QualificationName,
            QualificationInstitution,
            QualificationCertificateUrl,
            QualificationDateIssued
        )
        {
            TrainerId = newTrainer.Id
        };

        qualificationRepository.Add(qualification);
        */

        // TEST: Ispis podataka
        Console.WriteLine($"✓ Trainer Registration: {FirstName} {LastName} ({Email})");
        SuccessMessage = $"Registration submitted! Trainer: {FirstName} {LastName}";

        // Očisti formu
        FirstName = string.Empty;
        LastName = string.Empty;
        Email = string.Empty;
        Password = string.Empty;
        ConfirmPassword = string.Empty;
        PricePerMonth = 0;
        QualificationName = string.Empty;
        QualificationInstitution = string.Empty;
        QualificationCertificateUrl = string.Empty;
        QualificationDateIssued = DateTime.Now;
    }

    [RelayCommand]
    private void BackToLogin()
    {
        CloseRequested?.Invoke();
    }

    public event Action? CloseRequested;
}