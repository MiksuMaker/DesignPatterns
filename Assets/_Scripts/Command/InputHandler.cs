using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class InputHandler : MonoBehaviour
{
    #region Properties
    PlayerMover mover;

    public Vector3 currentMoveInput = Vector3.zero;


    #region KeyBinds
    Command W_key;
    Command A_key;
    Command S_key;
    Command D_key;

    Command Q_key; Command E_key;
    Command Z_key; Command X_key; Command C_key;

    Command Space_Key;
    #endregion

    #region Commands
    DoNothing doNothing;
    UndoCommand undo_command;

    // MOVEMENT
    MoveCommand moveForward_command;
    MoveCommand moveLeft_command;
    MoveCommand moveBack_command;
    MoveCommand moveRight_command;

    #endregion


    Stack<Command> previousCommands = new Stack<Command>();

    bool switchingKeybinds = false;
    Command keybindToBeSwitched;

    #endregion

    #region Setup
    private void Start()
    {
        mover = FindObjectOfType<PlayerMover>();

        SetupCommands();
        SetupInitialKeyBinds();
    }

    private void SetupCommands()
    {
        // NULL
        doNothing = new DoNothing();

        undo_command = new UndoCommand(this);

        // MOVEMENT
        moveForward_command = new MoveCommand(this, Vector3.forward);
        moveLeft_command = new MoveCommand(this, Vector3.left);
        moveBack_command = new MoveCommand(this, Vector3.back);
        moveRight_command = new MoveCommand(this, Vector3.right);


    }

    private void SetupInitialKeyBinds()
    {
        SetupNullKeyBinds(new List<Command>() { Q_key, E_key, X_key, C_key });

        W_key = moveForward_command;
        A_key = moveLeft_command;
        S_key = moveBack_command;
        D_key = moveRight_command;

        Z_key = undo_command;
    }

    private void SetupNullKeyBinds(List<Command> commands)
    {
        for (int i = 0; i < commands.Count; i++)
        {
            commands[i] = doNothing;
        }
    }
    #endregion

    #region Update
    private void Update()
    {
        if (!switchingKeybinds)
        {
            CheckInputs();
            CheckMoveInputs();
        }
        else
        {
            HandleKeybindSwitchInputs();
        }
    }
    #endregion

    #region Input Handling
    private void CheckMoveInputs()
    {
        if (currentMoveInput == Vector3.zero) { return; }

        // Call PlayerMover
        mover.UpdateMoveVector(currentMoveInput.normalized);

        // Reset
        currentMoveInput = Vector3.zero;

    }

    private void CheckInputs()
    {
        #region Input Checking
        // CONTINOUS MOVEMENT
        //if (Input.GetKey(KeyCode.W)) { W_key.Execute(); }
        //if (Input.GetKey(KeyCode.A)) { A_key.Execute(); }
        //if (Input.GetKey(KeyCode.S)) { S_key.Execute(); }
        //if (Input.GetKey(KeyCode.D)) { D_key.Execute(); }
        //if (Input.GetKeyDown(KeyCode.W)) { W_key.Execute(); }
        //if (Input.GetKeyDown(KeyCode.A)) { A_key.Execute(); }
        //if (Input.GetKeyDown(KeyCode.S)) { S_key.Execute(); }
        //if (Input.GetKeyDown(KeyCode.D)) { D_key.Execute(); }

        // INTERMITTENT MOVEMENT
        if (Input.GetKeyDown(KeyCode.W)) { Execute(ref W_key); }
        if (Input.GetKeyDown(KeyCode.A)) { Execute(ref A_key); }
        if (Input.GetKeyDown(KeyCode.S)) { Execute(ref S_key); }
        if (Input.GetKeyDown(KeyCode.D)) { Execute(ref D_key); ; }

        if (Input.GetKeyDown(KeyCode.Q)) { Execute(ref Q_key); }
        if (Input.GetKeyDown(KeyCode.E)) { Execute(ref E_key); }
        if (Input.GetKeyDown(KeyCode.Z)) { Execute(ref Z_key); }
        if (Input.GetKeyDown(KeyCode.X)) { Execute(ref X_key); }
        if (Input.GetKeyDown(KeyCode.C)) { Execute(ref C_key); }

        if (Input.GetKeyDown(KeyCode.Space)) { Execute(ref Space_Key); }
        #endregion
    }

    #endregion

    #region Command Handling & Undo
    private void Execute(ref Command executable)
    {
        // Execute it
        executable.Execute();

        // Add it to the last commands list
        RegisterCommand(ref executable);
    }

    private void RegisterCommand(ref Command registerable)
    {
        if (registerable is UndoCommand) { return; } // Don't register undo

        previousCommands.Push(registerable);
    }

    public void UndoLastCommand()
    {
        if (previousCommands.Count == 0) { return; }

        Command command = previousCommands.Pop();
        command.Undo();
    }
    #endregion

    #region Command & Key Rebinding
    public void SwapCommands(ref Command A, ref Command B)
    {
        Command tmp = A; A = B; B = tmp;
    }

    private void HandleKeybindSwitchInputs()
    {
        #region V1
        //// Choose first keybind
        //if (keybindToBeSwitched == null)
        //{
        //    // --> Assign keybind
        //    if (Input.GetKey(KeyCode.Q)) { keybindToBeSwitched = Q_key; }
        //    if (Input.GetKey(KeyCode.W)) { keybindToBeSwitched = W_key; }
        //    if (Input.GetKey(KeyCode.E)) { keybindToBeSwitched = E_key; }
        //    if (Input.GetKey(KeyCode.A)) { keybindToBeSwitched = A_key; }
        //    if (Input.GetKey(KeyCode.S)) { keybindToBeSwitched = S_key; }
        //    if (Input.GetKey(KeyCode.D)) { keybindToBeSwitched = D_key; }
        //    return; // Return for now
        //}

        //if (Input.GetKey(KeyCode.Q)) { SwapCommands(ref keybindToBeSwitched, ref Q_key); }
        //if (Input.GetKey(KeyCode.W)) { SwapCommands(ref keybindToBeSwitched, ref W_key); }
        //if (Input.GetKey(KeyCode.E)) { SwapCommands(ref keybindToBeSwitched, ref E_key); }
        //if (Input.GetKey(KeyCode.A)) { SwapCommands(ref keybindToBeSwitched, ref A_key); }
        //if (Input.GetKey(KeyCode.S)) { SwapCommands(ref keybindToBeSwitched, ref S_key); }
        //if (Input.GetKey(KeyCode.D)) { SwapCommands(ref keybindToBeSwitched, ref D_key); }

        //keybindToBeSwitched = null;
        //switchingKeybinds = false;
        #endregion


    }
    #endregion
}
