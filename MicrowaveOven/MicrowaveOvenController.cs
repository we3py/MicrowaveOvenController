namespace MicrowaveOven
{
    public class MicrowaveOvenController
    {
        private readonly IMicrowaveOvenHw? _microwaveOvenHw;

        public MicrowaveOvenController(IMicrowaveOvenHw microwaveOvenHw)
        {
            LightState = PowerState.Off;

            _microwaveOvenHw = microwaveOvenHw;

            _microwaveOvenHw.StartButtonPressed += OnStartButtonPressed;
            _microwaveOvenHw.DoorOpenChanged += OnDoorOpenChange;
        }

        public PowerState LightState { get; private set; }

        private void OnDoorOpenChange(bool isDoorOpen)
        {
            LightState = isDoorOpen ? PowerState.On : PowerState.Off;

            if (isDoorOpen)
            {
                _microwaveOvenHw?.TurnOffHeater();
            }
        }

        private void OnStartButtonPressed(object? sender, EventArgs e)
        {
            if (_microwaveOvenHw is { DoorOpen: false })
            {
                _microwaveOvenHw.TurnOnHeater();
            }
        }
    }
}
