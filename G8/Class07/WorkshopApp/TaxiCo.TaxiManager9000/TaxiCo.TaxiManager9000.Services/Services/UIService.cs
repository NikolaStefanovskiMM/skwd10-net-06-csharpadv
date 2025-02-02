﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaxiCo.TaxiManager9000.Domain;
using TaxiCo.TaxiManager9000.Domain.Enums;
using TaxiCo.TaxiManager9000.Domain.Models;
using TaxiCo.TaxiManager9000.Services.Services.Interfaces;
using TaxiCo.TaxiManager9000.Services.Utilities;

namespace TaxiCo.TaxiManager9000.Services.Services
{
    public class UIService : IUIService
    {
        private List<MenuChoice> _menuItems;
        public List<MenuChoice> MenuItems { 
            get => _menuItems; 
            set 
            {
                if (_menuItems != null)
                {
                    _menuItems.Clear();
                }
                _menuItems = value;
            }                
        }

        public int ChooseMenu<T>(List<T> items)
        {
            Console.Clear();
            if(items.Count == 0)
            {
                ExtendedConsole.NoItemsMessage<T>();
                Console.ReadLine();
                return -1;
            }

            while (true)
            {
                Console.WriteLine("Enter a number to choose one of the followint:");
                for (int i = 0; i < items.Count; i++) Console.WriteLine($"{i+1}) {items[i]}");
                int choice = StringValidator.ValidateNumber(Console.ReadLine(), items.Count);
                if(choice == -1)
                {
                    ExtendedConsole.WriteLine("[Error] Input incorrect, Please try again", ConsoleColor.Red);
                    Console.ReadLine();
                    Console.Clear();
                    continue;
                }
                return choice;
            }
        }

        public int ChooseEntitiesMenu<T>(List<T> entities) where T : BaseEntity
        {
            Console.Clear();
            if(entities.Count == 0)
            {
                ExtendedConsole.NoItemsMessage<T>();
                Console.ReadLine();
                return -1;
            }
            while (true)
            {
                Console.WriteLine("Enter a number to choose one of the following");
                for (int i = 0; i < entities.Count; i++) Console.WriteLine($"{i+1}) {entities[i].Print()}");
                int choice = StringValidator.ValidateNumber(Console.ReadLine(), entities.Count);
                if(choice == -1)
                {
                    ExtendedConsole.WriteLine("[Error] Input incorrect, Please try again", ConsoleColor.Red);
                    Console.ReadLine();
                    Console.Clear();
                    continue;
                }
                return choice;
            }
        }

        public User LogInMenu()
        {
            Console.WriteLine("Taxi Manager 9000");
            ExtendedConsole.Separator();
            Console.WriteLine("Log In:");
            string? username = ExtendedConsole.GetInput("Username:");
            string? password = ExtendedConsole.GetInput("Password");
            return new User()
            {
                Username = username,
                Password = password
            };
        }

        public int MainMenu(Role role)
        {
            var menuItems = new List<MenuChoice>() { MenuChoice.ChangePassword, MenuChoice.Exit};
            switch (role)
            {
                case Role.Administrator:
                    menuItems.Prepend(MenuChoice.AddNewUser).ToList();
                    break;
                case Role.Manager:
                    menuItems = menuItems.Prepend(MenuChoice.ListAllDrivers).ToList();
                    menuItems = menuItems.Prepend(MenuChoice.TaxiLicenseStatus).ToList();
                    menuItems = menuItems.Prepend(MenuChoice.DriverManager).ToList();
                    break;
                case Role.Maintenance:
                    menuItems = menuItems.Prepend(MenuChoice.ListAllCars).ToList();
                    menuItems = menuItems.Prepend(MenuChoice.LicensePlateStatus).ToList();
                    break;
            }
            MenuItems = menuItems;
            return ChooseMenu(MenuItems);
        }

        public void Welcome(User user)
        {
            Console.WriteLine($"Welcome {user.Role} user!");
        }
    }

    public enum MenuChoice
    {
        AddNewUser,
        RemoveExistingUser,
        ListAllDrivers,
        TaxiLicenseStatus,
        DriverManager,
        ListAllCars,
        LicensePlateStatus,
        ChangePassword,
        Exit
    }

    public enum DriverManagerChoice
    {
        AssignDriver,
        UnassignDriver
    }
}
