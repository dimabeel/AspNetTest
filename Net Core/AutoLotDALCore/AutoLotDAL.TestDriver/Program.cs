using System;
using System.Linq;
using AutoLotDALCore.DataInitialization;
using AutoLotDALCore.EF;
using AutoLotDALCore.Models;
using AutoLotDALCore.Repos;
using Microsoft.EntityFrameworkCore;

namespace AutoLotDALCore.TestDriver
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("***** Fun with EF Core *****\n");
            using (var context = new AutoLotContext())
            {
                MyDataInitializer.RecreateDataBase(context);
                MyDataInitializer.InitializeData(context);
                foreach (var car in context.Cars)
                {
                    Console.WriteLine(car);
                }
            }

            Console.WriteLine("***** Using a repo *****\n");
            using (var repo = new InventoryRepo(new AutoLotContext()))
            {
                foreach (var car in repo.GetAll())
                {
                    Console.WriteLine(car);
                }
            }

            Console.ReadLine();
        }

        private static void AddNewRecord(Inventory car)
        {
            using (var repo = new InventoryRepo(new AutoLotContext()))
            {
                repo.Add(car);
            }
        }

        private static void UpdateRecord(int carId)
        {
            using (var repo = new InventoryRepo(new AutoLotContext()))
            {
                // Grab the car, change it, save! 
                var carToUpdate = repo.GetOne(carId);
                if (carToUpdate == null) return;
                carToUpdate.Color = "Blue";
                repo.Update(carToUpdate);
            }
        }

        private static void RemoveRecordByCar(Inventory carToDelete)
        {
            using (var repo = new InventoryRepo(new AutoLotContext()))
            {
                repo.Delete(carToDelete);
            }
        }

        private static void RemoveRecordById(int carId, byte[] timeStamp)
        {
            using (var repo = new InventoryRepo(new AutoLotContext()))
            {
                repo.Delete(carId, timeStamp);
            }
        }

        private static void TestConcurrency()
        {
            var repo1 = new InventoryRepo(new AutoLotContext());
            //Use a second repo to make sure using a different context
            var repo2 = new InventoryRepo(new AutoLotContext());
            var car1 = repo1.GetOne(1);
            var car2 = repo2.GetOne(1);
            car1.PetName = "NewName";
            repo1.Update(car1);
            car2.PetName = "OtherName";
            try
            {
                repo2.Update(car2);
            }
            catch (DbUpdateConcurrencyException ex)
            {
                var entry = ex.Entries.Single();
                var currentValues = entry.CurrentValues;
                var originalValues = entry.OriginalValues;
                var dbValues = entry.GetDatabaseValues();
                Console.WriteLine(" ******** Concurrency ************");
                Console.WriteLine("Type\tPetName");
                Console.WriteLine($"Current:\t{currentValues[nameof(Inventory.PetName)]}");
                Console.WriteLine($"Orig:\t{originalValues[nameof(Inventory.PetName)]}");
                Console.WriteLine($"db:\t{dbValues[nameof(Inventory.PetName)]}");
            }
        }
    }

}
