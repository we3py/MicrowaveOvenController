using System.Timers;
using Timer = System.Timers.Timer;

namespace MicrowaveOven;

public class Heater : IDisposable
{
    private const int DefaultHeatingTimeInSeconds = 60;
    private const int TimeIntervalInMiliseconds = 1000;
    private const int MaximumSetupTimeInSeconds = 600;

    public event Action<double>? TimerElapsed;

    public Heater()
    {
        PowerState = PowerState.Off;
        Timer = new Timer(TimeIntervalInMiliseconds);
        Timer.Elapsed += OnTimerTick;
    }

    public double RemainingTime { get; private set; }
    public PowerState PowerState { get; set; }
    public Timer Timer { get; }

    private void OnTimerTick(object? sender, ElapsedEventArgs e)
    {
        if(RemainingTime <= 0)
        {
            ResetHeater();
            return;
        }

        RemainingTime--;
        TimerElapsed?.Invoke(RemainingTime);
    }

    public void StartHeater()
    {
        if (PowerState == PowerState.On)
        {
            var timeCheck = RemainingTime + DefaultHeatingTimeInSeconds;
            if (timeCheck > MaximumSetupTimeInSeconds) 
            { 
                RemainingTime = MaximumSetupTimeInSeconds;
                return;
            }

            RemainingTime += DefaultHeatingTimeInSeconds;
            return;
        }

        if (RemainingTime > 0)
        {
            PowerState = PowerState.On;
            Timer.Start();
            return;
        }

        RemainingTime = DefaultHeatingTimeInSeconds;
        PowerState = PowerState.On;
        Timer.Start();
    }

    public void StopHeater()
    {
        PowerState = PowerState.Off;
        Timer.Stop();
    }

    public void ResetHeater()
    {
        RemainingTime = 0;
        PowerState = PowerState.Off;
        Timer.Stop();
        TimerElapsed?.Invoke(RemainingTime);
    }

    public void Dispose()
    {
        Timer.Dispose();
    }
}