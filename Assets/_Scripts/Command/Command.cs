using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Command
{
    public abstract void Execute();
}

public class DoNothing : Command
{
    public override void Execute() {} // Do nothing
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
}