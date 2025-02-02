﻿#region Setup
using Class13.TaxiManager9000.Domain.Entities;
using Class13.TaxiManager9000.Domain.Enums;
using Class13.TaxiManager9000.Services.Helpers;
using Class13.TaxiManager9000.Services.Interfaces;
using Class13.TaxiManager9000.Services.Services;

ICarService carService = new CarService();
IDriverService driverService = new DriverService();
IUserService userService = new UserService();
IUIService uiService = new UIService();
ILogger logger = new Logger();

InitializeStartingData();
#endregion

while (true)
{
    Console.Clear();
    if (userService.CurrentUser == null)
    {
        try
        {
            User loginUser = uiService.LogIn();
            userService.Login(loginUser.Username, loginUser.Password);

            ConsoleHelper.WriteLine($"Successful Login! Welcome[{userService.CurrentUser.Role}] user!", ConsoleColor.Green);
        }
        catch (Exception ex)
        {
            logger.Log("Error", ex.Message, ex.StackTrace, "not logged in");
            ConsoleHelper.WriteLine(ex.Message, ConsoleColor.Red);
            continue;
        }
    }

    bool loopActive = true;
    while (loopActive)
    {
        Console.Clear();
        int selectedItem = uiService.MainMenu(userService.CurrentUser.Role);
        if (selectedItem == -1)
        {
            ConsoleHelper.WriteLine("Wrong option Selected", ConsoleColor.Red);
            continue;
        }
        MenuOptions choise = uiService.MenuChoice[selectedItem - 1];
        switch (choise)
        {
            case MenuOptions.AddNewUser:
                {
                    try
                    {
                        string username = ConsoleHelper.GetInput("Username:");
                        string password = ConsoleHelper.GetInput("Password:");
                        List<string> roles = new List<string>() { "Administrator", "Manager", "Maintenance" };
                        int enumInt = uiService.ChooseMenu(roles);

                        if (enumInt < 0 || enumInt > 2)
                        {
                            ConsoleHelper.WriteLine("Invalid role selection!", ConsoleColor.Red);
                            break;
                        }

                        Role role = (Role)enumInt;
                        User user = new User(username, password, role);
                        userService.Add(user);

                        ConsoleHelper.WriteLine("New User Added", ConsoleColor.Green);
                    }
                    catch (Exception ex)
                    {
                        ConsoleHelper.WriteLine(ex.Message, ConsoleColor.Red);
                    }
                    break;
                }
            case MenuOptions.RemoveExistingUser:
                {
                    try
                    {
                        Console.WriteLine("Select User For Removal (insert number in front of username):");
                        var usersForRemoval = userService.GetUsersForRemoval();
                        int selectedUser = uiService.ChooseEntityMenu(usersForRemoval);
                        if (selectedUser == -1)
                        {
                            ConsoleHelper.WriteLine("Wrong option Selected", ConsoleColor.Red);
                            continue;
                        }

                        if (userService.Remove(usersForRemoval[selectedUser - 1].Id))
                        {
                            ConsoleHelper.WriteLine("User removed", ConsoleColor.Yellow);
                        }
                    }
                    catch (Exception ex)
                    {
                        ConsoleHelper.WriteLine(ex.Message, ConsoleColor.Red);
                    }
                    break;
                }
            case MenuOptions.ListAllDrivers:
                {
                    var allData = driverService.GetAll();
                    AddvertisementHelper.DiscountAdd();
                    await allData;
                    allData.Result.ForEach(x => Console.WriteLine(x.Print()));
                    Console.ReadLine();
                    break;
                }
            case MenuOptions.TaxiLicenseStatus:
                {
                    carService.GetAll().Result.ForEach(x =>
                                {
                                    var status = x.IsLicenseExpired();

                                    switch (status)
                                    {
                                        case ExpieryStatus.Expired:
                                            Console.ForegroundColor = ConsoleColor.Red;
                                            break;
                                        case ExpieryStatus.Valid:
                                            Console.ForegroundColor = ConsoleColor.Green;
                                            break;
                                        case ExpieryStatus.Warning:
                                            Console.ForegroundColor = ConsoleColor.Yellow;
                                            break;
                                    }

                                    Console.WriteLine(x.Print());
                                    Console.ResetColor();
                                });
                    Console.ReadLine();
                    break;
                }
            case MenuOptions.DriverManager:
                {
                    var driverManagerMenu = new List<DriverManagerChoice>() { DriverManagerChoice.AssignDriver, DriverManagerChoice.UnassignDriver };
                    int driverManagerChoice = uiService.ChooseMenu(driverManagerMenu);
                    var availableDrivers = driverService.GetAll(x => driverService.IsAvailableDriver(x));

                    if (driverManagerChoice == 1)
                    {
                        var availableForAssigningDrivers = availableDrivers
                            .Where(x => x.CarId == null)
                            .ToList();
                        int assigningDrvierChoice = uiService
                            .ChooseEntityMenu<Driver>(availableForAssigningDrivers);
                        if (assigningDrvierChoice == -1) continue;

                        var availableCarsForAssigning = carService
                            .GetAll(x => carService.IsAvailableCar(x))
                            .ToList();
                        var assigningCarChoice = uiService
                            .ChooseEntityMenu<Car>(availableCarsForAssigning);
                        if (assigningCarChoice == -1) continue;

                        driverService.AssignDriver(
                            availableForAssigningDrivers[assigningDrvierChoice - 1],
                            availableCarsForAssigning[assigningCarChoice - 1]
                            );
                        carService.AssignDriver(availableForAssigningDrivers[assigningDrvierChoice - 1],
                            availableCarsForAssigning[assigningCarChoice - 1]
                            );
                    }
                    else if (driverManagerChoice == 2)
                    {
                        var availableForUnassigningDrivers = availableDrivers
                            .Where(x => x.CarId != null)
                            .ToList();
                        var unassigningDrvierChoice = uiService
                            .ChooseEntityMenu<Driver>(availableForUnassigningDrivers);
                        if (unassigningDrvierChoice == -1) continue;

                        driverService.Unassign(availableForUnassigningDrivers[unassigningDrvierChoice - 1]);
                    }
                    break;
                }
            case MenuOptions.ListAllCars:
                {
                    var taskData = carService.GetAll();
                    AddvertisementHelper.DiscountAdd();
                    await taskData;
                    taskData.Result.ForEach(x => Console.WriteLine(x.Print()));
                    Console.ReadLine();
                    break;
                }
            case MenuOptions.LicensePlateStatus:
                {
                    foreach (var car in carService.GetAll().Result)
                    {
                        ExpieryStatus status = car.IsLicenseExpired();
                        switch (status)
                        {
                            case ExpieryStatus.Expired:
                                Console.ForegroundColor = ConsoleColor.Red;
                                break;
                            case ExpieryStatus.Valid:
                                Console.ForegroundColor = ConsoleColor.Green;
                                break;
                            case ExpieryStatus.Warning:
                                Console.ForegroundColor = ConsoleColor.Yellow;
                                break;
                        }
                        Console.WriteLine($"Car Id: {car.Id} - Plate: {car.LicensePlate} with expiery date: {car.LicensePlateExpieryDate}");
                        Console.ResetColor();
                    }
                    Console.ReadLine();
                    break;
                }
            case MenuOptions.ChangePassword:
                {
                    string oldPass = ConsoleHelper.GetInput("Insert old password: ");
                    string newPass = ConsoleHelper.GetInput("Insert new password: ");
                    if (userService.ChangePassword(userService.CurrentUser.Id, oldPass, newPass))
                    {
                        ConsoleHelper.WriteLine("Password changed!", ConsoleColor.Green);
                    }
                    break;
                }
            case MenuOptions.Exit:
                {
                    userService.CurrentUser = null;
                    loopActive = false;
                    break;
                }
        }
    }
}

#region Seeding
void InitializeStartingData()
{
    User administrator = new User("BobBobsky", "bobbest1", Role.Administrator);
    User manager = new User("JillWayne", "jillawesome1", Role.Manager);
    User maintenances = new User("GregGregsky", "supergreg1", Role.Maintenance);
    List<User> seedUsers = new List<User>() { administrator, manager, maintenances };
    userService.Seed(seedUsers);

    Car car1 = new Car("Auris (Toyota)", "AFW950", new DateTime(2023, 12, 1));
    Car car2 = new Car("Auris (Toyota)", "CKE480", new DateTime(2021, 10, 15));
    Car car3 = new Car("Transporter (Volkswagen)", "GZDR69", new DateTime(2024, 5, 30));
    Car car4 = new Car("Mondeo (Ford)", "5RIP283", new DateTime(2022, 5, 13));
    Car car5 = new Car("Premier (Peugeot)", "2AR9907", new DateTime(2022, 11, 9));
    Car car6 = new Car("Vito (Mercedes)", "6RND294", new DateTime(2023, 3, 11));
    List<Car> seedCars = new List<Car>() { car1, car2, car3, car4, car5, car6 };
    carService.Seed(seedCars);

    Driver driver1 = new Driver("Romario", "Walsh", Shift.NoShift, null, "LC12456123", new DateTime(2023, 11, 5));
    Driver driver2 = new Driver("Kathleen", "Rankin", Shift.Morning, car1.Id, "LC54435234", new DateTime(2022, 1, 12));
    Driver driver3 = new Driver("Ashanti", "Mooney", Shift.Evening, car1.Id, "LC65803245", new DateTime(2022, 5, 19));
    Driver driver4 = new Driver("Zakk", "Hook", Shift.Afternoon, car1.Id, "LC20897583", new DateTime(2023, 9, 28));
    Driver driver5 = new Driver("Xavier", "Kelly", Shift.NoShift, null, "LC15636280", new DateTime(2024, 6, 1));
    Driver driver6 = new Driver("Joy", "Shelton", Shift.Evening, car2.Id, "LC47845611", new DateTime(2023, 7, 3));
    Driver driver7 = new Driver("Kristy", "Riddle", Shift.Morning, car3.Id, "LC19006543", new DateTime(2024, 6, 12));
    Driver driver8 = new Driver("Stuart", "Mayer", Shift.Evening, car3.Id, "LC53187767", new DateTime(2023, 10, 10));
    List<Driver> seedDrivers = new List<Driver>() { driver1, driver2, driver3, driver4, driver5, driver6, driver7, driver8 };
    driverService.Seed(seedDrivers);
}
#endregion