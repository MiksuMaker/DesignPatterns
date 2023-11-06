using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Command
{
    public abstract void Execute();

    public abstract void Undo();
}

public class DoNothing : Command
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

public class UndoCommand : Command
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