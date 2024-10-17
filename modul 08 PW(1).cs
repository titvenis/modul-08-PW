using System;
using System.Collections.Generic;

public interface ICommand
{
    void Execute();
    void Undo();
}

public class Light
{
    public void On() => Console.WriteLine("Свет включен");
    public void Off() => Console.WriteLine("Свет выключен");
}

public class AirConditioner
{
    public void On() => Console.WriteLine("Кондиционер включен");
    public void Off() => Console.WriteLine("Кондиционер выключен");
}

public class TV
{
    public void On() => Console.WriteLine("Телевизор включен");
    public void Off() => Console.WriteLine("Телевизор выключен");
}

public class LightOnCommand : ICommand
{
    private readonly Light _light;

    public LightOnCommand(Light light) => _light = light;

    public void Execute() => _light.On();
    public void Undo() => _light.Off();
}

public class LightOffCommand : ICommand
{
    private readonly Light _light;

    public LightOffCommand(Light light) => _light = light;

    public void Execute() => _light.Off();
    public void Undo() => _light.On();
}

public class AirConditionerOnCommand : ICommand
{
    private readonly AirConditioner _airConditioner;

    public AirConditionerOnCommand(AirConditioner airConditioner) => _airConditioner = airConditioner;

    public void Execute() => _airConditioner.On();
    public void Undo() => _airConditioner.Off();
}

public class AirConditionerOffCommand : ICommand
{
    private readonly AirConditioner _airConditioner;

    public AirConditionerOffCommand(AirConditioner airConditioner) => _airConditioner = airConditioner;

    public void Execute() => _airConditioner.Off();
    public void Undo() => _airConditioner.On();
}

public class TVOnCommand : ICommand
{
    private readonly TV _tv;

    public TVOnCommand(TV tv) => _tv = tv;

    public void Execute() => _tv.On();
    public void Undo() => _tv.Off();
}

public class TVOffCommand : ICommand
{
    private readonly TV _tv;

    public TVOffCommand(TV tv) => _tv = tv;

    public void Execute() => _tv.Off();
    public void Undo() => _tv.On();
}

public class RemoteControl
{
    private readonly ICommand[] _onCommands;
    private readonly ICommand[] _offCommands;
    private ICommand _lastCommand;

    public RemoteControl()
    {
        _onCommands = new ICommand[5];
        _offCommands = new ICommand[5];
        _lastCommand = null;
    }

    public void SetCommand(int slot, ICommand onCommand, ICommand offCommand)
    {
        _onCommands[slot] = onCommand;
        _offCommands[slot] = offCommand;
    }

    public void OnButtonPressed(int slot)
    {
        if (_onCommands[slot] != null)
        {
            _onCommands[slot].Execute();
            _lastCommand = _onCommands[slot];
        }
        else
        {
            Console.WriteLine("Команда не назначена");
        }
    }

    public void OffButtonPressed(int slot)
    {
        if (_offCommands[slot] != null)
        {
            _offCommands[slot].Execute();
            _lastCommand = _offCommands[slot];
        }
        else
        {
            Console.WriteLine("Команда не назначена");
        }
    }

    public void UndoButtonPressed()
    {
        _lastCommand?.Undo();
    }
}

public class MacroCommand : ICommand
{
    private readonly List<ICommand> _commands;

    public MacroCommand(List<ICommand> commands) => _commands = commands;

    public void Execute()
    {
        foreach (var command in _commands)
            command.Execute();
    }

    public void Undo()
    {
        foreach (var command in _commands)
            command.Undo();
    }
}

class Program
{
    static void Main()
    {
        var remoteControl = new RemoteControl();

        var livingRoomLight = new Light();
        var ac = new AirConditioner();
        var tv = new TV();

        var lightOn = new LightOnCommand(livingRoomLight);
        var lightOff = new LightOffCommand(livingRoomLight);
        var acOn = new AirConditionerOnCommand(ac);
        var acOff = new AirConditionerOffCommand(ac);
        var tvOn = new TVOnCommand(tv);
        var tvOff = new TVOffCommand(tv);

        remoteControl.SetCommand(0, lightOn, lightOff);
        remoteControl.SetCommand(1, acOn, acOff);
        remoteControl.SetCommand(2, tvOn, tvOff);

        remoteControl.OnButtonPressed(0);
        remoteControl.OffButtonPressed(0);
        remoteControl.UndoButtonPressed();

        var macroCommand = new MacroCommand(new List<ICommand> { lightOn, acOn, tvOn });
        Console.WriteLine("\nВыполнение макрокоманды:");
        macroCommand.Execute();
        Console.WriteLine("\nОтмена макрокоманды:");
        macroCommand.Undo();
    }
}
