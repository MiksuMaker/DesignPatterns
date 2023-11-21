using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMover : MonoBehaviour
{
    #region Properties
    Rigidbody rb;

    public Vector3 currentMoveVector = Vector3.zero;

    Vector3 desiredPos;

    [SerializeField] float moveSpeed = 10f;

    [SerializeField] Transform graphics;

    // STATE MACHINE
    public enum State
    {
        standing, jumping, crouching
    }

    public State currentState = State.standing;
    #endregion

    #region Setup
    private void Start()
    {
        SetupPlayerMover();
    }

    private void SetupPlayerMover()
    {
        rb = GetComponent<Rigidbody>();
        desiredPos = transform.position;
    }

    private void FixedUpdate()
    {
        HandleMovement();
    }

    private void Update()
    {
        LerpTowardsPos();
    }
    #endregion

    #region Movement
    public void UpdateMoveVector(Vector3 newVector)
    {
        currentMoveVector = newVector;
    }

    private void HandleMovement()
    {
        if (currentMoveVector == Vector3.zero) { return; }

        // Move via RB
        //rb.MovePosition(transform.position
        //                + currentMoveVector
        //                * moveSpeed
        //                * Time.fixedDeltaTime);


        //transform.position += currentMoveVector * moveSpeed;
        desiredPos += currentMoveVector * moveSpeed;

        // Move the transform to DESIRED position
        //transform.position = desiredPos;

        // Reset movement input
        currentMoveVector = Vector3.zero;
    }

    private void LerpTowardsPos()
    {
        if (transform.position == desiredPos) { return; }

        Vector3 dist = (desiredPos - transform.position);

        // If close enough, snap to place
        if (dist.magnitude <= 0.05f) { transform.position = desiredPos; return; }

        // Else, lerp towards desired pos
        Vector3 nextPos = transform.position + dist * 20f * Time.deltaTime;

        transform.position = nextPos;
    }

    public void ResetPosition()
    {
        rb.MovePosition(Vector3.zero);
    }

    public void Jump()
    {
        if (CheckUndesiredState(new List<State> { State.crouching })) { Debug.Log("no JUMPING while CROUCHED");  return; }
        ChangeState(State.jumping);

        desiredPos += Vector3.up * 5f;
    }

    public void Unjump()
    {
        if (!CheckWantedState(new List<State> { State.jumping })) { return; }
        ChangeState(State.standing);

        desiredPos += Vector3.down * 5f;
    }

    public void Crouch()
    {
        if (CheckUndesiredState(new List<State> { State.jumping })) { Debug.Log("no CROUCHING while JUMPING"); return; }
        ChangeState(State.crouching);

        graphics.localScale = new Vector3(1f, 0.5f, 1f);
    }

    public void Uncrouch()
    {
        if (!CheckWantedState(new List<State> { State.crouching })) { return; }
        ChangeState(State.standing);

        graphics.localScale = new Vector3(1f, 1f, 1f);
    }
    #endregion

    #region State Machine
    private void ExecuteStateMachine()
    {
        switch (currentState)
        {
            case State.standing:

                break;

            case State.jumping:

                break;

            case State.crouching:

                break;
        }
    }

    public void ChangeState(State newState)
    {
        Debug.Log("State: " + newState);
        currentState = newState;
    }

    public bool CheckWantedState(List<State> states)
    {
        foreach (var s in states)
        {
            // If state is found, return TRUE
            if (currentState == s) { return true; }
        }
        return false;
    }

    public bool CheckUndesiredState(List<State> states)
    {
        foreach (var s in states)
        {
            // If state is found, return FALSE
            if (currentState == s) { return true; }
        }
        return false;
    }
    #endregion
}
