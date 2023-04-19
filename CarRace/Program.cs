using System.Security.Cryptography.X509Certificates;
using static Program;

public partial class Program
{
    static async Task Main(string[] args)
    {

        Console.WriteLine("Hello and welcome to todays race 8) Have a seat and sit down, grab a drink and relax.");
        Console.WriteLine("Press enter to start the race");
        Console.ReadKey();
        Console.WriteLine();
        Console.WriteLine("The race has begun!");
        Console.WriteLine();

        // Creating 3 car objects
        Car minicooper = new Car()
        {
            id = 1,
            name = "Minicooper",
            speed = 120, //  in km/h
            distanceDriven = 0, // in km
            distanceLeft = 10, //  in km
            time = 0 // in seconds
        };
        Car volvo = new Car()
        {
            id = 2,
            name = "Volvo",
            speed = 120, //  in km/h
            distanceDriven = 0, // in km
            distanceLeft = 10, //  in km
            time = 0 // in seconds
        };
        Car suzukiSwift = new Car()
        {
            id = 3,
            name = "Suzuki Swift",
            speed = 120, //  in km/h
            distanceDriven = 0, // in km
            distanceLeft = 10, //  in km
            time = 0 // in seconds
    };

        // Creating and starting each cars seperate Drive tasks
        var minicooperTask = DriveCar(minicooper);
        var volvoTask = DriveCar(volvo);
        var suzukiSwiftTask = DriveCar(suzukiSwift);
        var carStatusTask = CarStatus(new List<Car> { minicooper, volvo, suzukiSwift});

       
        var carTasks = new List<Task> { minicooperTask, volvoTask, suzukiSwiftTask, carStatusTask };

        while (carTasks.Count > 0)
        {
            // Waits for any task to be completed
            Task finishedTask = await Task.WhenAny(carTasks);

            if (finishedTask == minicooperTask)
            {
                // If carTasks is > 3 that means no car has yet finished and so the first one in will be the winner and have a special message.
                if (carTasks.Count > 3)
                {
                    Console.WriteLine();
                    Console.WriteLine("The Minicooper has won the race!");
                    PrintCar(minicooper);
                }
                else
                {
                    Console.WriteLine();
                    Console.WriteLine("The Minicooper finished the race.");
                    PrintCar(minicooper);
                }
                
            }
            else if (finishedTask == volvoTask)
            {
                if (carTasks.Count > 3)
                {
                    Console.WriteLine();
                    Console.WriteLine("The Volvo has won the race!");
                    PrintCar(volvo);
                }
                else
                {
                    Console.WriteLine();
                    Console.WriteLine("The Volvo finished the race.");
                    PrintCar(volvo);
                }
                
                
            }
            else if (finishedTask == suzukiSwiftTask)
            {
                if (carTasks.Count > 3)
                {
                    Console.WriteLine();
                    Console.WriteLine("The Suzuki Swift has won the race! ezclap 8)");
                    PrintCar(suzukiSwift);
                }
                else
                {
                    Console.WriteLine();
                    Console.WriteLine("The Suzuki Swift has finished the race");
                    PrintCar(suzukiSwift);
                }
            }
            // Runs after all car tasks are done 
            else if (finishedTask == carStatusTask)
            {
                Console.WriteLine();
                Console.WriteLine("All cars have finished the race.");
                break;
            }

            await finishedTask;
            carTasks.Remove(finishedTask);
        }
    }
    public async static Task<Car> DriveCar(Car car)
    {
        
        int uhOhCounter = 0;
        // Updates cars every second
        int driveInterval = 1;
        // Calculates how far each car drives during each second
        decimal intervalDist = car.speed * 0.000277778M; // distance travelled per 1 sec
        // Creates a timestamp for when each car starts
        DateTime start = DateTime.Now;
        while (true)
        {
            // Creates random number from 1-100 for uhOH moments
            Random rnd = new Random();
            int uhOH = rnd.Next(1, 100);

            // Saves how far the car has left in seconds instead of hours
            decimal timeLeftInSeconds = car.timeLeft() * 3600;

            await Wait(driveInterval); // Waits 1 second

            
            var newTime = (DateTime.Now - start).TotalSeconds; 
            car.time = newTime; // Updates the cars current time its been racing each second
            car.distanceDriven += intervalDist; // in km
            car.distanceLeft -= intervalDist; // in km
            uhOhCounter++;
            
            // Every 30 seconds a uhOH moment happens where there are different % chances of bad things happening to the car, unless there is 30 seconds or less left for the car to reach the end.
            if (uhOhCounter == 30 && timeLeftInSeconds >= 30)
            {
                
                if (uhOH <= 2) // 2% for running out of fule
                {
                    Console.WriteLine();
                    Console.WriteLine($"The {car.name} has run out of gas and needs 30 seconds to refule!");
                    await car.outOfGas();
                    Console.WriteLine();
                    Console.WriteLine($"The {car.name} is now refuled and on the race track again!");
                    uhOhCounter = 0;
                    car.time += 30;
                }
                else if (uhOH > 2 && uhOH <= 6) // 4% for getting a flat tire
                {
                    Console.WriteLine();
                    Console.WriteLine($"The {car.name} has gotten a flat tire and needs 20 seconds to replace it!");
                    await car.flatTire();
                    Console.WriteLine();
                    Console.WriteLine($"The {car.name} has replaced the flat tire and is back on the race track!");
                    uhOhCounter = 0;
                    car.time += 20;
                }
                else if (uhOH > 6 && uhOH <= 16) // 10% for bird attack
                {
                    Console.WriteLine();
                    Console.WriteLine($"The {car.name} has gotten a bird on the windshield and needs 10 seconds to remove it it!");
                    await car.birdOnWindshield();
                    Console.WriteLine();
                    Console.WriteLine($"The {car.name} has removed the bird and is back in the race!");
                    uhOhCounter = 0;
                    car.time += 10;
                }
                else if (uhOH > 16 && uhOH <= 36) // 20% for engine failure
                {
                    Console.WriteLine();
                    Console.WriteLine($"The {car.name} has gotten engine failure and its max speed is reduced by 1km/h!");
                    car.speed -= 1;
                    uhOhCounter = 0;
                }
                else // Nothing happens 
                {
                    // uhOh counter resets to 0 
                    uhOhCounter = 0;
                }
            }
            if (car.distanceDriven >= 10)
            {
                // car has finished race and loop ends
                return car;
            }
        }

    }
    public async static Task Wait(int delay = 1) // If no other argument is given it defaults to 1
    {
        await Task.Delay(TimeSpan.FromSeconds(delay));
    }
    // Method to print out the cars results 
    public static void PrintCar(Car car)
    {
        Console.WriteLine();
        Console.WriteLine("Car results: ");
        Console.WriteLine($"Speed: {car.speed} ");
        Console.WriteLine($"Finished race in: {car.time} seconds");

    }
    // Method to see the cars current status
    public async static Task CarStatus(List<Car> cars)
    {
       
        while (true)
        {
            
            DateTime start = DateTime.Now;

            bool gotKey = false;

            while ((DateTime.Now - start).TotalSeconds < 2)
            {
                if (Console.KeyAvailable)
                {
                    gotKey = true;
                    break;
                }
            }

            if (gotKey)
            {
                Console.ReadKey();
                
                cars.ForEach(car =>
                {
                    if (car.timeLeft() <= 0)
                    {
                        Console.WriteLine("------------------------------------------------------------------------------------------------------------");
                        Console.WriteLine($"The {car.name} has finished the race");
                        Console.WriteLine("------------------------------------------------------------------------------------------------------------");
                    }
                    else
                    {
                        Console.WriteLine("------------------------------------------------------------------------------------------------------------");
                        Console.WriteLine($"{car.name} has " + Decimal.Round(car.timeLeft() * 3600) + " seconds left at this moment in time until goal");
                        Console.WriteLine($"and is driving at {car.speed}km/h and has driven " + Decimal.Round(car.distanceDriven) + "km.");
                        Console.WriteLine("------------------------------------------------------------------------------------------------------------");
                    }
                    
                });
                gotKey = false;
            }
            await Task.Delay(10);

            // When all cars remaining time is 0 ends the simulation
            var totalRemaining = cars.Select(car => car.timeLeft()).Sum();

            if (totalRemaining <= 0)
            {
                return;
            }
        }
    }
}


