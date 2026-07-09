namespace FitnessApp.Helpers;

using System;
using System.Collections.Generic;
using FitnessApp.Models;
using FitnessApp.Repositories;
using FitnessApp.Enums;

public class SeedDataHelper
{
    public static void InitializeSampleData()
    {
        try
        {
            var clientRepo = new ClientRepository();
            var trainerRepo = new TrainerRepository();
            var qualificationRepo = new QualificationRepository();
            var healthRecordRepo = new HealthRecordRepository();
            var adminRepo = new AdminRepository();

            // Create sample health records
            var healthRecord1 = new HealthRecord
            {
                HealthConditions = new List<string> { "None" },
                Disabilities = new List<string> { }
            };

            var healthRecord2 = new HealthRecord
            {
                HealthConditions = new List<string> { "Mild asthma", "Allergies" },
                Disabilities = new List<string> { }
            };

            // Add health records first
            healthRecordRepo.Add(healthRecord1);
            healthRecordRepo.Add(healthRecord2);

            // Create sample clients
            var client1 = new Client
            {
                FirstName = "John",
                LastName = "Smith",
                Email = "john.smith@email.com",
                Password = "password",
                HealthRecordId = 1
            };

            var client2 = new Client
            {
                FirstName = "Sarah",
                LastName = "Johnson",
                Email = "sarah.johnson@email.com",
                Password = "password",
                HealthRecordId = 2
            };

            // Create sample trainers
            var trainer1 = new Trainer
            {
                FirstName = "Michael",
                LastName = "Williams",
                Email = "michael.williams@email.com",
                Password = "password",
                PricePerMonth = 99.99m,
                Status = TrainerStatus.ACTIVE,
                Rating = 0.0
            };

            var trainer2 = new Trainer
            {
                FirstName = "Emma",
                LastName = "Brown",
                Email = "emma.brown@email.com",
                Password = "password",
                PricePerMonth = 89.99m,
                Status = TrainerStatus.ACTIVE,
                Rating = 0.0
            };

            // Add clients
            clientRepo.Add(client1);
            clientRepo.Add(client2);

            // Add trainers
            trainerRepo.Add(trainer1);
            trainerRepo.Add(trainer2);

            // Create and add qualifications for trainer1
            var qual1 = new Qualification(
                "Certified Personal Trainer (CPT)",
                "International Fitness Association",
                "https://certificates.example.com/cpt-001",
                new DateTime(2020, 6, 15)
            )
            {
                TrainerId = 1
            };

            var qual2 = new Qualification(
                "Nutrition Specialist",
                "National Board of Fitness Examiners",
                "https://certificates.example.com/nutrition-001",
                new DateTime(2021, 3, 20)
            )
            {
                TrainerId = 1
            };

            // Create and add qualifications for trainer2
            var qual3 = new Qualification(
                "Certified Strength and Conditioning Specialist (CSCS)",
                "National Strength and Conditioning Association",
                "https://certificates.example.com/cscs-001",
                new DateTime(2019, 9, 10)
            )
            {
                TrainerId = 2
            };

            var qual4 = new Qualification(
                "Group Fitness Instructor",
                "American Council on Exercise",
                "https://certificates.example.com/gfi-001",
                new DateTime(2022, 1, 25)
            )
            {
                TrainerId = 2
            };

            // Add qualifications
            qualificationRepo.Add(qual1);
            qualificationRepo.Add(qual2);
            qualificationRepo.Add(qual3);
            qualificationRepo.Add(qual4);

            // Create sample admin
            var admin = new Administrator
            {
                FirstName = "Sofija",
                LastName = "Zoric",
                Email = "sofija@email.com",
                Password = "password"
            };

            // Add admin
            adminRepo.Add(admin);

            Console.WriteLine("\n╔════════════════════════════════════════╗");
            Console.WriteLine("║   ✓ Sample Data Initialized            ║");
            Console.WriteLine("╚════════════════════════════════════════╝");
            Console.WriteLine("✓ Added 2 health records");
            Console.WriteLine($"✓ Added 2 clients: {client1.FirstName} {client1.LastName}, {client2.FirstName} {client2.LastName}");
            Console.WriteLine($"✓ Added 2 trainers: {trainer1.FirstName} {trainer1.LastName}, {trainer2.FirstName} {trainer2.LastName}");
            Console.WriteLine($"✓ Added 4 qualifications (2 per trainer)");
            Console.WriteLine($"✓ Added 1 admin: {admin.FirstName} {admin.LastName}");
            Console.WriteLine("═══════════════════════════════════════\n");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"✗ Error initializing sample data: {ex.Message}");
            Console.WriteLine($"✗ Stack trace: {ex.StackTrace}");
        }
    }
}
