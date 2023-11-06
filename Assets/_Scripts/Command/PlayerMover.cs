using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMover : MonoBehaviour
{
    #region Properties
    Rigidbody rb;

    public Vector3 currentMoveVector = Vector3.zero;

    [SerializeField] float moveSpeed = 10f;
    #endregion

    #region Setup
    private void Start()
    {
        SetupPlayerMover();
    }

    private void SetupPlayerMover()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        HandleMovement();
    }
    #endregion

    #region Functions
    public void UpdateMoveVector(Vector3 newVector)
    {
        currentMoveVector = newVector;
    }

    private void HandleMovement()
    {
        if (currentMoveVector == Vector3.zero) { return; }

        // Move via RB
        rb.MovePosition(transform.position
                        + currentMoveVector
                        * moveSpeed
                        * Time.fixedDeltaTime);

        transform.position += currentMoveVector * moveSpeed;

        // Reset movement input
        currentMoveVector = Vector3.zero;
    }

    public void ResetPosition()
    {
        rb.MovePosition(Vector3.zero);
    }
    #endregion

}
