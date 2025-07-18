using Spectre.Console;

namespace MicrowaveOven
{
    internal class Program
    {
        private bool _exitRequested;
        private Heater _heater;
        private MicrowaveOvenHw _microwaveOvenHw;
        private MicrowaveOvenController _microwaveOvenController;

        static async Task Main(string[] args)
        {
            var program = new Program();
            await program.RunAsync();
        }

        private async Task RunAsync()
        {
            using var heater = new Heater();
            using var microwaveOvenHw = new MicrowaveOvenHw(heater);
            var microwaveOvenController = new MicrowaveOvenController(microwaveOvenHw);

            _heater = heater;
            _microwaveOvenHw = microwaveOvenHw;
            _microwaveOvenController = microwaveOvenController;

            AnsiConsole.Write(new FigletText("Microwave Oven").LeftJustified());

            await AnsiConsole.Live(CreateStatusTable())
                .AutoClear(false)
                .Overflow(VerticalOverflow.Ellipsis)
                .StartAsync(async ctx =>
                {
                    var updateTask = Task.Run(async () =>
                    {
                        while (!_exitRequested)
                        {
                            ctx.UpdateTarget(CreateStatusTable());
                            await Task.Delay(100);
                        }
                    });

                    while (!_exitRequested)
                    {
                        var key = Console.ReadKey(true);

                        switch (key.Key)
                        {
                            case ConsoleKey.O:
                                _microwaveOvenHw.ChangeStateOfMicrowaveOvenDoors();
                                break;
                            case ConsoleKey.S:
                                _microwaveOvenHw.PressStartButton();
                                break;
                            case ConsoleKey.T:
                                _microwaveOvenHw.PressStopButton();
                                break;
                            case ConsoleKey.Q:
                            case ConsoleKey.Escape:
                                _exitRequested = true;
                                break;
                        }
                    }

                    await updateTask;
                });
        }

        private Table CreateStatusTable()
        {
            var table = new Table();
            table.AddColumn("Property");
            table.AddColumn("Value");

            table.AddRow("Light State", _microwaveOvenController.LightState.ToString().Equals("Off") ? "[red]Off[/]" : "[green]On[/]");
            table.AddRow("Door State", _microwaveOvenHw.DoorOpen ? "[red]Open[/]" : "[green]Closed[/]");
            table.AddRow("Heater State", _microwaveOvenHw.HeaterState.ToString().Equals("Off") ? "[red]Off[/]" : "[green]On[/]");
            table.AddRow("Timer", $"{_heater.RemainingTime} seconds");
            table.AddRow("", "");
            table.AddRow("Controls", "[yellow]O[/] - Open/Close Door, [yellow]S[/] - Start, [yellow]T[/] - Turn off, [yellow]Q[/] - Quit");

            return table;
        }
    }
}