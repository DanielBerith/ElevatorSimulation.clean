using System;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using ElevatorSimulation.Application;
using ElevatorSimulation.Infrastructure;
using ElevatorSimulation.Domain.Entities;
using ElevatorSimulation.Domain.Enums;
using ElevatorSimulation.Application.Interfaces;
using ElevatorSimulation.Application.Services;

namespace ElevatorSimulation.Presentation
{
    class Program
    {
        static void Main(string[] args)
        {
            // Setup the dependency injection container.
            var services = new ServiceCollection();

            // Configure logging to output to the console.
            services.AddLogging(builder =>
            {
                builder.AddConsole();
                builder.SetMinimumLevel(LogLevel.Information);
            });

            // Register Application and Infrastructure services.
            services.AddApplicationServices();
            services.ConfigureInfrastructureServices();

            // Register the RequestQueue as a singleton.
            services.AddSingleton<RequestQueue>();

            // Build the service provider.
            var serviceProvider = services.BuildServiceProvider();

            // Resolve required services.
            var elevatorService = serviceProvider.GetRequiredService<IElevatorService>();
            var requestService = serviceProvider.GetRequiredService<IRequestService>();
            var requestQueue = serviceProvider.GetRequiredService<RequestQueue>();

            // Create sample elevators.
            List<Elevator> elevators = new List<Elevator>
            {
                // Passenger elevator for calls from floor 1.
                new Elevator(1, 6, 5, ElevatorType.Passenger),
                // Freight elevator for calls from floor 3.
                new Elevator(2, 3, 10, ElevatorType.Freight),
                // HighSpeed elevator for calls from floor 6.
                new Elevator(3, 5, 8, ElevatorType.HighSpeed)
            };

            // Subscribe to the ElevatorStatusChanged event for real-time status updates.
            foreach (var elevator in elevators)
            {
                elevator.ElevatorStatusChanged += (e) =>
                {
                    Console.WriteLine($"[Status] Elevator {e.Id}: Floor {e.GetCurrentFloor()}, Direction {e.GetDirection()}, Occupants {e.GetOccupants()}");
                };
            }

            // Display instructions.
            Console.WriteLine("Elevator Simulation Console Application");
            Console.WriteLine("Commands:");
            Console.WriteLine("  call <floor> <passengers>   - Call an elevator to the specified floor with the given number of passengers.");
            Console.WriteLine("  status                      - Display current status of all elevators.");
            Console.WriteLine("  exit                        - Exit the application.");
            Console.WriteLine();

            // Interactive command loop.
            while (true)
            {
                Console.Write("Enter command: ");
                var input = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(input))
                    continue;

                var tokens = input.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                var command = tokens[0].ToLower();

                if (command == "exit")
                    break;
                else if (command == "status")
                {
                    Console.WriteLine("Elevator Status:");
                    foreach (var elevator in elevators)
                    {
                        Console.WriteLine($"Elevator {elevator.Id}: Floor {elevator.GetCurrentFloor()}, Direction {elevator.GetDirection()}, Occupants {elevator.GetOccupants()}");
                    }
                }
                else if (command == "call")
                {
                    if (tokens.Length < 3)
                    {
                        Console.WriteLine("Usage: call <floor> <passengers>");
                        continue;
                    }

                    if (!int.TryParse(tokens[1], out int floor))
                    {
                        Console.WriteLine("Invalid floor number.");
                        continue;
                    }
                    if (!int.TryParse(tokens[2], out int passengers))
                    {
                        Console.WriteLine("Invalid passengers number.");
                        continue;
                    }

                    // Create a new request and enqueue it.
                    var request = new Request(floor, passengers);
                    requestQueue.EnqueueRequest(request);

                    // Dispatch the elevator for the given request.
                    requestService.DispatchElevator(elevators, request);
                }
                else
                {
                    Console.WriteLine("Unknown command.");
                }
            }

            Console.WriteLine("Processing complete. Press any key to exit...");
            Console.ReadKey();
        }
    }
}