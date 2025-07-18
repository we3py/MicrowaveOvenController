# Microwave Oven Controller

This project implements a simple controller for a microwave oven, focusing on core functionalities and user interactions as specified in the requirements. The solution includes the controller logic and corresponding unit tests, designed within a Visual Studio environment.

## Features

The microwave oven model is designed with the following components and behaviors:

* **Heater:** Can be turned on or off.

* **Door:** Can be opened and closed by the user.

* **Start Button:** Can be pressed by the user.

* **Maximum Timer:** The oven's timer can go up to a maximum of 10 minutes.

* **Reset Functionality:** The heating process and timer can be reset.

## User Stories

The controller adheres to the following user interaction scenarios:

* **Door Open/Close & Light:**

    * When the door is opened, the light turns on.

    * When the door is closed, the light turns off.

* **Door Open & Heater:**

    * When the door is opened, the heater stops if it is currently running.

* **Start Button Behavior:**

    * When the start button is pressed while the door is open, nothing happens.

    * When the start button is pressed while the door is closed, the heater runs for 1 minute.

    * When the start button is pressed while the door is closed and the heater is already running, the remaining heating time is increased by 1 minute.

## Hardware Interface

The controller interacts with the microwave oven hardware through the following interface:

```csharp
public interface IMicrowaveOvenHW
{
    /// <summary>
    /// Turns on the Microwave heater element
    /// </summary>
    void TurnOnHeater();

    /// <summary>
    /// Turns off the Microwave heater element
    /// </summary>
    void TurnOffHeater();

    /// <summary>
    /// Indicates if the door to the Microwave oven is open or closed
    /// </summary>
    bool DoorOpen { get; }

    /// <summary>
    /// Signal if the Door is opened or closed.
    /// </summary>
    event Action<bool> DoorOpenChanged;

    /// <summary>
    /// Signals that the Start button is pressed
    /// </summary>
    event EventHandler StartButtonPressed;
}
```
## Getting Started

To set up and run this project:

### Option 1: Using Visual Studio

1.  Clone the repository.

2.  Open the solution in Visual Studio.

3.  Build the solution to restore NuGet packages and compile the code.

4.  Run the unit tests to verify the controller's functionality.

### Option 2: Using the .NET CLI (Command Line Interface)

This option is great if you prefer working in the terminal or don't have Visual Studio installed.
1. **Prerequisite:** Ensure you have the [.NET SDK](https://dotnet.microsoft.com/download) installed on your machine.
2.  Clone the repository
   
3.  Go to the project folder (e.g., `src/MicrowaveOven`). 

4.  **Run the application:**
    ```bash
    dotnet run
    ```

## Usage (Console Application)

This application is designed as a console application. User interaction is primarily through keyboard inputs. The display in the console is enhanced using the Spectre.Console framework for a rich and interactive user experience.

## Screenshots

<img width="933" height="305" alt="Zrzut ekranu 2025-07-18 152801" src="https://github.com/user-attachments/assets/7b1fcb91-f6c5-4eff-8f76-791a2445cb4f" />

## Additional Notes

This project focuses on the core logic and user stories provided. Further enhancements could include:
* Error handling for unexpected hardware states.
