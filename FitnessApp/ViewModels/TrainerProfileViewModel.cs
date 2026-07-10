using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FitnessApp.Models;
using FitnessApp.Repositories;
using FitnessApp.Enums;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FitnessApp.ViewModels
{
    public partial class TrainerProfileViewModel : ObservableObject
    {
        private readonly Trainer _trainer;
        private readonly TrainerRepository _trainerRepository;
        private readonly QualificationRepository _qualificationRepository;

        // Osnovni podaci
        [ObservableProperty]
        private string _firstName = string.Empty;

        [ObservableProperty]
        private string _lastName = string.Empty;

        [ObservableProperty]
        private string _email = string.Empty;

        [ObservableProperty]
        private string _password = string.Empty;

        // Profesionalni podaci
        [ObservableProperty]
        private decimal _pricePerMonth;

        [ObservableProperty]
        private TrainerStatus _status;

        [ObservableProperty]
        private double _rating;

        [ObservableProperty]
        private string _statusDisplay = string.Empty;

        [ObservableProperty]
        private string _ratingDisplay = string.Empty;

        // Qualifikacije
        [ObservableProperty]
        private ObservableCollection<Qualification> _qualifications = new();

        [ObservableProperty]
        private Qualification? _selectedQualification;

        [ObservableProperty]
        private string _newQualificationName = string.Empty;

        [ObservableProperty]
        private string _newQualificationInstitution = string.Empty;

        [ObservableProperty]
        private string _newQualificationCertificateUrl = string.Empty;

        [ObservableProperty]
        private DateTimeOffset? _newQualificationDateIssued = DateTimeOffset.Now;

        // UI stanje
        [ObservableProperty]
        private string _saveMessage = string.Empty;

        [ObservableProperty]
        private bool _isLoading;

        [ObservableProperty]
        private bool _isSaveSuccess;

        [ObservableProperty]
        private bool _isEditing;

        public TrainerProfileViewModel(Trainer trainer)
        {
            _trainer = trainer;
            _trainerRepository = new TrainerRepository();
            _qualificationRepository = new QualificationRepository();

            LoadProfile();
            LoadQualifications();
            UpdateDisplayValues();
        }

        private void LoadProfile()
        {
            FirstName = _trainer.FirstName ?? string.Empty;
            LastName = _trainer.LastName ?? string.Empty;
            Email = _trainer.Email ?? string.Empty;
            Password = _trainer.Password ?? string.Empty;
            PricePerMonth = _trainer.PricePerMonth;
            Status = _trainer.Status;
            Rating = _trainer.Rating;
        }

        private void LoadQualifications()
        {
            Qualifications.Clear();
            var allQualifications = _qualificationRepository.GetAll();
            var trainerQualifications = allQualifications.Where(q => q.TrainerId == _trainer.Id);

            foreach (var qualification in trainerQualifications)
            {
                Qualifications.Add(qualification);
            }
        }

        private void UpdateDisplayValues()
        {
            StatusDisplay = Status switch
            {
                TrainerStatus.ACTIVE => "Active",
                TrainerStatus.PENDING => "Pending",
                TrainerStatus.SUSPENDED => "Suspended",
                TrainerStatus.REJECTED => "Rejected",
                TrainerStatus.REMOVED => "Removed",
                _ => Status.ToString()
            };

            RatingDisplay = Rating > 0 ? $"⭐ {Rating:F1} / 5" : "⭐ No ratings yet";
        }

        [RelayCommand]
        private void ToggleEdit()
        {
            IsEditing = !IsEditing;
            SaveMessage = string.Empty;
            IsSaveSuccess = false;

            if (!IsEditing)
            {
                LoadProfile();
                LoadQualifications();
                UpdateDisplayValues();
                ClearQualificationForm();
            }
        }

        [RelayCommand]
        private void AddQualification()
        {
            if (string.IsNullOrWhiteSpace(NewQualificationName))
            {
                SaveMessage = "Qualification name is required.";
                return;
            }

            if (string.IsNullOrWhiteSpace(NewQualificationInstitution))
            {
                SaveMessage = "Institution is required.";
                return;
            }

            if (NewQualificationDateIssued == null)
            {
                SaveMessage = "Date issued is required.";
                return;
            }

            var newQualification = new Qualification(
                NewQualificationName,
                NewQualificationInstitution,
                NewQualificationCertificateUrl,
                NewQualificationDateIssued.Value.DateTime  
            )
            {
                TrainerId = _trainer.Id
            };

            _qualificationRepository.Add(newQualification);
            Qualifications.Add(newQualification);
            ClearQualificationForm();

            SaveMessage = "Qualification added successfully!";
            IsSaveSuccess = true;
        }

        [RelayCommand]
        private void RemoveQualification()
        {
            if (SelectedQualification == null)
            {
                SaveMessage = "Please select a qualification to remove.";
                return;
            }

            _qualificationRepository.Delete(SelectedQualification.Id);
            Qualifications.Remove(SelectedQualification);
            SelectedQualification = null;

            SaveMessage = "Qualification removed successfully!";
            IsSaveSuccess = true;
        }

        private void ClearQualificationForm()
        {
            NewQualificationName = string.Empty;
            NewQualificationInstitution = string.Empty;
            NewQualificationCertificateUrl = string.Empty;
            NewQualificationDateIssued = DateTimeOffset.Now;  // ⭐ POPRAVLJENO
        }

        [RelayCommand]
        private async Task SaveProfile()
        {
            IsLoading = true;
            SaveMessage = string.Empty;
            IsSaveSuccess = false;

            try
            {
                // Validacija
                if (string.IsNullOrWhiteSpace(FirstName))
                {
                    SaveMessage = "First name is required.";
                    return;
                }

                if (string.IsNullOrWhiteSpace(LastName))
                {
                    SaveMessage = "Last name is required.";
                    return;
                }

                if (string.IsNullOrWhiteSpace(Email))
                {
                    SaveMessage = "Email is required.";
                    return;
                }

                if (string.IsNullOrWhiteSpace(Password) || Password.Length < 6)
                {
                    SaveMessage = "Password must be at least 6 characters.";
                    return;
                }

                if (PricePerMonth < 0)
                {
                    SaveMessage = "Price per month cannot be negative.";
                    return;
                }

                // Proveri da li email već postoji
                var existingTrainer = _trainerRepository.GetByEmail(Email);
                if (existingTrainer != null && existingTrainer.Id != _trainer.Id)
                {
                    SaveMessage = "Email already in use by another account.";
                    return;
                }

                // Ažuriraj podatke
                _trainer.FirstName = FirstName;
                _trainer.LastName = LastName;
                _trainer.Email = Email;
                _trainer.Password = Password;
                _trainer.PricePerMonth = PricePerMonth;

                _trainerRepository.Update(_trainer);

                SaveMessage = "Profile updated successfully!";
                IsSaveSuccess = true;
                IsEditing = false;
                UpdateDisplayValues();

                await Task.Delay(3000);
                SaveMessage = string.Empty;
            }
            catch (Exception ex)
            {
                SaveMessage = $"Error: {ex.Message}";
                IsSaveSuccess = false;
            }
            finally
            {
                IsLoading = false;
            }
        }

        [RelayCommand]
        private void RefreshProfile()
        {
            LoadProfile();
            LoadQualifications();
            UpdateDisplayValues();
            SaveMessage = "Profile refreshed!";
            IsSaveSuccess = false;
        }
    }
}