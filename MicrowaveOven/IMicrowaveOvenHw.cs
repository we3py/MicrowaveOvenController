﻿namespace MicrowaveOven
{
    /// <summary> 
    /// Interface to the Microwave oven hardware 
    /// </summary> 
    public interface IMicrowaveOvenHw
    {
        /// <summary> 
        /// Turns on the Microwave heater element 
        /// </summary> 
        void TurnOnHeater();

        /// <summary> 
        /// Turns oA the Microwave heater element 
        /// </summary> 
        void TurnOffHeater();

        /// <summary> 
        /// Indicates if the door to the Microwave oven is open or closed 
        /// </summary> 
        bool DoorOpen { get; }

        /// <summary> 
        /// Signal if the Door is opened or closed, 
        /// </summary> 
        event Action<bool> DoorOpenChanged;

        /// <summary> 
        /// Signals that the Start button is pressed 
        /// </summary> 
        event EventHandler StartButtonPressed;
    }
}
