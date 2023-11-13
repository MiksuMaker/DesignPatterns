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

    #region Functions
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
    #endregion

}
