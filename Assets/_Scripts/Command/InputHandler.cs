using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class InputHandler : MonoBehaviour
{
    #region Properties
    PlayerMover mover;

    public Vector3 currentMoveInput = Vector3.zero;

    KeyCode storedKeyCode;
    bool keyCodeIsStored = false;

    #region KeyBinds
    Command W_key;
    Command A_key;
    Command S_key;
    Command D_key;

    Command Q_key; Command E_key;
    Command Z_key; Command X_key; Command C_key;

    Command Space_key;
    #endregion

    #region Commands
    // SPECIAL
    DoNothing doNothing;
    UndoCommand undo_command;
    RebindKeysCommand rebindKeys_command;
    Command rebind_temp;

    // MOVEMENT
    MoveCommand moveForward_command;
    MoveCommand moveLeft_command;
    MoveCommand moveBack_command;
    MoveCommand moveRight_command;

    #endregion


    Stack<Command> previousCommands = new Stack<Command>();

    bool switchingKeybindsInProgress = false;
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
        // SPECIAL
        doNothing = new DoNothing();
        undo_command = new UndoCommand(this);
        rebindKeys_command = new RebindKeysCommand(this);

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
        Space_key = rebindKeys_command;
    }

    private void SetupNullKeyBinds(List<Command> commands)
    {
        // V1
        //for (int i = 0; i < commands.Count; i++)
        //{
        //    commands[i] = doNothing;
        //}

        // V2 : Manual labor
        Q_key = doNothing; E_key = doNothing; X_key = doNothing; C_key = doNothing;

    }
    #endregion

    #region Update
    private void Update()
    {
        if (!switchingKeybindsInProgress)
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
        if (Input.GetKeyDown(KeyCode.D)) { Execute(ref D_key); }

        if (Input.GetKeyDown(KeyCode.Q)) { Execute(ref Q_key); }
        if (Input.GetKeyDown(KeyCode.E)) { Execute(ref E_key); }
        if (Input.GetKeyDown(KeyCode.Z)) { Execute(ref Z_key); }
        if (Input.GetKeyDown(KeyCode.X)) { Execute(ref X_key); }
        if (Input.GetKeyDown(KeyCode.C)) { Execute(ref C_key); }

        if (Input.GetKeyDown(KeyCode.Space)) { Execute(ref Space_key); }
        #endregion
    }

    #endregion

    #region Command Handling & Undo
    private void Execute(ref Command executable)
    {
        if (switchingKeybindsInProgress && executable is not RebindKeysCommand)
        { return; }

        if (executable == null) { Debug.Log("Executable is of type DoNothing."); }

        // Execute it
        executable.Execute();

        // Add it to the last commands list
        RegisterCommand(ref executable);
    }

    private void RegisterCommand(ref Command registerable)
    {
        if (registerable is UndoCommand
            || registerable is RebindKeysCommand) { return; } // Don't register undo

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


    public void HandleKeybindSwitchInputs()
    {
        // Check if switching is in progress already
        if (!switchingKeybindsInProgress)
        {
            Debug.Log("Starting rebind process!");
            switchingKeybindsInProgress = true;
            return;
        }

        if (Input.GetKeyDown(KeyCode.W)) { RebindKey(ref W_key); }
        if (Input.GetKeyDown(KeyCode.A)) { RebindKey(ref A_key); }
        if (Input.GetKeyDown(KeyCode.S)) { RebindKey(ref S_key); }
        if (Input.GetKeyDown(KeyCode.D)) { RebindKey(ref D_key); }

        if (Input.GetKeyDown(KeyCode.Q)) { RebindKey(ref Q_key); }
        if (Input.GetKeyDown(KeyCode.E)) { RebindKey(ref E_key); }
        if (Input.GetKeyDown(KeyCode.Z)) { RebindKey(ref Z_key); }
        if (Input.GetKeyDown(KeyCode.X)) { RebindKey(ref X_key); }
        if (Input.GetKeyDown(KeyCode.C)) { RebindKey(ref C_key); }

    }

    private void RebindKey(ref Command key)
    {
        // Check if first temp command has been assigned already
        bool firstKeyAssigned = true; if (keyCodeIsStored == false) { firstKeyAssigned = false; }

        // If first key is not assigned yet, store it temporarily
        if (!firstKeyAssigned)
        {
            Debug.Log("Storing the first key, choose another!");
            StoreInputToKeyCode();
            return;
        }

        // If there is a stored key, Swap them!
        SwapCommands(ref KeyCodeToCommand(), ref key);

        // Empty the temp
        keyCodeIsStored = false;

        Debug.Log("Keys swapped!");

        // Get out of rebind mode
        switchingKeybindsInProgress = false;
    }

    private void StoreInputToKeyCode()
    {
        KeyCode c = KeyCode.A; // Default
        if (Input.GetKeyDown(KeyCode.W)) { c = KeyCode.W; }
        if (Input.GetKeyDown(KeyCode.A)) { c = KeyCode.A; }
        if (Input.GetKeyDown(KeyCode.S)) { c = KeyCode.S; }
        if (Input.GetKeyDown(KeyCode.D)) { c = KeyCode.D; }
                                                         
        if (Input.GetKeyDown(KeyCode.Q)) { c = KeyCode.Q; }
        if (Input.GetKeyDown(KeyCode.E)) { c = KeyCode.E; }
        if (Input.GetKeyDown(KeyCode.Z)) { c = KeyCode.Z; }
        if (Input.GetKeyDown(KeyCode.X)) { c = KeyCode.X; }
        if (Input.GetKeyDown(KeyCode.C)) { c = KeyCode.C; }

        // Store it and notify the bool
        storedKeyCode = c;
        keyCodeIsStored = true;
    }

    private ref Command KeyCodeToCommand()
    {
        switch (storedKeyCode)
        {
            case KeyCode.W: return ref W_key;
            case KeyCode.A: return ref A_key;
            case KeyCode.S: return ref S_key;
            case KeyCode.D: return ref D_key;
            case KeyCode.Q: return ref Q_key;
            case KeyCode.E: return ref E_key;
            case KeyCode.Z: return ref Z_key;
            case KeyCode.X: return ref X_key;
            case KeyCode.C: return ref C_key;

            case KeyCode.Space: return ref Space_key;
        }

        return ref W_key; // Default
    }
    #endregion
}
