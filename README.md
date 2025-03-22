# Elevator Simulation

A challenging console application in C# that simulates the movement of elevators within a large building to optimize passenger transportation. The solution is built using Clean Architecture, SOLID principles, and object-oriented programming (OOP) concepts to ensure modularity, maintainability, and extensibility.

## Table of Contents

- [Overview](#overview)
- [Features](#features)
- [Architecture](#architecture)
- [Project Structure](#project-structure)
- [Getting Started](#getting-started)
- [Running the Application](#running-the-application)
- [Running Unit Tests](#running-unit-tests)
- [Dependencies](#dependencies)
- [Contributing](#contributing)
- [License](#license)

## Overview

The Elevator Simulation project is designed to simulate real-time elevator operations within a multi-story building. The application:
- Displays the real-time status of each elevator (current floor, direction, occupancy).
- Allows interactive elevator control via the console.
- Supports multiple floors and multiple elevator types.
- Implements an efficient dispatching strategy to minimize wait times and optimize usage.
- Handles passenger limits by boarding as many passengers as possible while reporting those left waiting.
- Considers different elevator types (Passenger, Freight, HighSpeed) and assigns elevators based on the requested floor.

## Features

- **Real-Time Elevator Status:**  
  Monitor the current floor, movement direction, and occupancy of each elevator.

- **Interactive Elevator Control:**  
  Users can call an elevator by specifying a floor and the number of passengers waiting.

- **Multiple Floors and Elevators Support:**  
  Simulates a building with various floors and different elevator types.

- **Efficient Elevator Dispatching:**  
  Uses dispatching strategies (NearestElevatorStrategy, FloorBasedDispatchingStrategy) to assign the best-suited elevator for a request.

- **Passenger Limit Handling:**  
  Ensures that an elevator boards as many passengers as possible without exceeding its capacity, reporting any remaining waiting passengers.

- **Extensibility for Different Elevator Types:**  
  Designed to easily accommodate additional elevator types (e.g., HighSpeed, Freight, Glass).

- **Real-Time Operation:**  
  Provides immediate responses to user interactions and updates elevator status in real-time.

## Architecture

The project follows **Clean Architecture** principles by separating the solution into distinct layers:

- **Domain Layer:**  
  Contains core business entities (Elevator, Request) and enums (Direction, ElevatorType).

- **Application Layer:**  
  Contains the business logic, including services (ElevatorService, RequestService), request queue management, and dispatching strategies.

- **Infrastructure Layer:**  
  Handles cross-cutting concerns such as logging (implemented via `AppLogger<T>`) and dependency injection configurations.

- **Presentation Layer:**  
  Provides the console user interface that interacts with the user, handles input, and displays output.

## Project Structure

```
ElevatorSimulation/
│
├── src/
│   ├── Application/
│   │   ├── Interfaces/
│   │   │   ├── IElevatorService.cs
│   │   │   ├── IRequestService.cs
│   │   │   ├── IDispatchingStrategy.cs
│   │   ├── Services/
│   │       ├── ElevatorService.cs
│   │       ├── RequestService.cs
│   │       ├── RequestQueue.cs
│   │       ├── NearestElevatorStrategy.cs
│   │       ├── FloorBasedDispatchingStrategy.cs
│   ├── Domain/
│   │   ├── Entities/
│   │   │   ├── Elevator.cs
│   │   │   ├── Request.cs
│   │   ├── Enums/
│   │       ├── Direction.cs
│   │       ├── ElevatorType.cs
│   ├── Infrastructure/
│   │   ├── Logging/
│   │       ├── AppLogger.cs
│   │   ├── Extensions/
│   │       ├── InfrastructureServicesRegistration.cs
│   ├── Presentation/
│       ├── ConsoleApp/
│           ├── Program.cs
│
└── tests/
    ├── ElevatorSimulation.Tests/
        ├── Features/
            ├── ElevatorService/
            │   ├── ElevatorServiceTests.cs
            ├── RequestService/
            │   ├── RequestServiceTests.cs
            ├── DispatchingStrategies/
                ├── NearestElevatorStrategyTests.cs
                ├── FloorBasedDispatchingStrategyTests.cs
        ├── Mocks/
            ├── ElevatorServiceMockTests.cs
            ├── NearestElevatorStrategyTests.cs
            ├── RequestServiceMockTests.cs
```

## Getting Started

1. **Clone the Repository:**

   ```bash
   git clone https://github.com/your-username/ElevatorSimulation.git
   cd ElevatorSimulation
   ```

2. **Build the Project:**

   Ensure you have the .NET SDK installed. Then run:

   ```bash
   dotnet build
   ```

## Running the Application

Navigate to the console application's folder and run:

```bash
dotnet run --project src/Presentation/ConsoleApp
```

You will see an interactive console where you can:
- Call an elevator using `call <floor> <passengers>`.
- View elevator status using `status`.
- Exit the application using `exit`.

## Running Unit Tests

Navigate to the tests directory and run:

```bash
dotnet test
```

This will execute all unit tests using xUnit, ensuring that your services and dispatching strategies function as expected.

## Dependencies

- .NET 5.0 (or later)
- Microsoft.Extensions.DependencyInjection
- Microsoft.Extensions.Logging
- xUnit (for testing)
- Moq (for mocking in tests)
- Shouldly (for expressive assertions)

## Contributing

Contributions are welcome!  
- Fork the repository.
- Create a feature branch (`git checkout -b feature/YourFeature`).
- Commit your changes.
- Open a pull request.

Please adhere to the coding standards and architecture guidelines established in this project.

## License

This project is licensed under the MIT License. See the [LICENSE](LICENSE) file for details.
