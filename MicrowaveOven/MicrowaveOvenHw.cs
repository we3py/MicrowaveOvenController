namespace MicrowaveOven
{
    public class MicrowaveOvenHw : IMicrowaveOvenHw, IDisposable
    {
        private readonly Heater _heater;

        public event Action<bool>? DoorOpenChanged;
        public event EventHandler? StartButtonPressed;

        public MicrowaveOvenHw(Heater heater)
        {
            _heater = heater;
            DoorOpen = false;
        }

        public bool DoorOpen { get; private set; }
        public PowerState HeaterState => _heater.PowerState;

        public void TurnOffHeater()
        {
            if (_heater.PowerState == PowerState.Off)
                return;

            _heater.StopHeater();
        }

        public void TurnOnHeater()
        {
            if (DoorOpen)
                return;

            _heater.StartHeater();
        }

        public void ChangeStateOfMicrowaveOvenDoors()
        {
            DoorOpen = !DoorOpen;
            if (DoorOpen)
            {
                _heater.StopHeater();
            }

            DoorOpenChanged?.Invoke(DoorOpen);
        }

        public void PressStartButton()
        {
            StartButtonPressed?.Invoke(this, EventArgs.Empty);
        }

        public void PressStopButton()
        {
            if (_heater.RemainingTime > 0)
            {
                _heater.ResetHeater();
            }
        }

        public void Dispose()
        {
            _heater.Dispose();
        }
    }
}
