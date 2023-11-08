using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Command
{
    public abstract void Execute();

    public abstract void Undo();
}

public class DoNothing : Command, Unrepeatable
{
    public override void Execute() {} // Do nothing
    public override void Undo() {} // Do nothing
}

public class MoveCommand : Command
{
    InputHandler input;
    Vector3 moveDir;

    public MoveCommand(InputHandler _input, Vector3 _moveDir)
    {
        input = _input;
        moveDir = _moveDir;
    }

    public override void Execute()
    {
        input.currentMoveInput += moveDir;
    }

    public override void Undo()
    {
        input.currentMoveInput -= moveDir;
    }
}

public class UndoCommand : Command, Unrepeatable
{
    InputHandler input;

    public UndoCommand(InputHandler _input)
    {
        input = _input;
    }

    public override void Execute()
    {
        input.UndoLastCommand();
    }

    public override void Undo()
    {

    }
}

public class RebindKeysCommand : Command, Unrepeatable
{
    InputHandler input;

    public RebindKeysCommand(InputHandler _input)
    { input = _input; }

    public override void Execute()
    {
        input.HandleKeybindSwitchInputs();
    }

    public override void Undo()
    {
    }
}

public class RedoCommand : Command, Unrepeatable
{
    InputHandler input;

    public RedoCommand(InputHandler _input)
    {
        input = _input;
    }

    public override void Execute()
    {
        //input.ReplayAllCommands();
        input.Redo();
    }

    public override void Undo()
    {

    }
}

public class FunctionCommand : Command
{
    public Storable.StoredFunction storedFunction;

    public FunctionCommand(Storable.StoredFunction _storable)
    {
        storedFunction = _storable;
    }

    public override void Execute()
    {
        //input.ReplayAllCommands();
        //input.Redo();
        storedFunction?.Invoke();
    }

    public override void Undo()
    {

    }
}
public class UnrepeatableCommand : FunctionCommand, Unrepeatable 
{
    public UnrepeatableCommand(Storable.StoredFunction _storable) : base(_storable) { }
}

public interface Unrepeatable { }

public static class Storable
{
    public delegate void StoredFunction();
}