using System;
using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    #region Properties
    public event Action OnEnemyDeath;

    Rigidbody rb;

    [SerializeField, Range(0f, 1f)]
    float chanceToMove = 0.5f;
    float movementDistance = 5f;
    Vector3 desiredPos;
    [SerializeField]
    float tooFarDistance = 1f;

    [SerializeField]
    float fiddleStrength = 5f;
    [SerializeField]
    float fiddleFrequency = 2f;

    [SerializeField] bool movevementOn = true;
    #endregion

    #region Setup
    private void Start()
    {
        SetupEnemy();
    }

    private void SetupEnemy()
    {
        rb = GetComponent<Rigidbody>();
        desiredPos = transform.position;

        OnEnemyDeath += FindObjectOfType<ScoreManager>().EnemyWasSlain;

        StartCoroutine(Mover());
    }
    #endregion

    #region Running

    #endregion

    #region Movement
    IEnumerator Mover()
    {
        while (movevementOn)
        {
            yield return new WaitForSeconds(fiddleFrequency);

            // Fiddle
            Fiddle();

            // Move desired pos
            if (UnityEngine.Random.Range(0f, 1f) <= chanceToMove)
            {
                // Move to random direction
                MoveDesiredPos();
            }
        }
    }

    private void MoveDesiredPos()
    {
        int rand = UnityEngine.Random.Range(1, 4);

        Vector3 d = Vector3.zero;

        switch (rand)
        {
            case 1: d = Vector3.forward; break;
            case 2: d = Vector3.back; break;
            case 3: d = Vector3.left; break;
            case 4: d = Vector3.right; break;
        }

        // Move desired pos
        desiredPos += (d * movementDistance);
        Debug.DrawRay(desiredPos, Vector3.up * 3f, Color.red, 5f);
    }

    private void Fiddle()
    {
        // Check if too far from desired pos
        bool tooFar = false;
        float checkDist = tooFarDistance * tooFarDistance;
        if (Vector3.SqrMagnitude(transform.position - desiredPos) >= checkDist)
        {
            tooFar = true;
        }

        // Get appropriate fiddle vector
        Vector3 fiddleVector = Vector3.zero;
        float s = fiddleStrength;
        if (tooFar)
        {
            // Towards desired position
            fiddleVector = (desiredPos - transform.position).normalized * UnityEngine.Random.Range(s * 0.5f, s * 2f);
        }
        else
        {
            // Random
            fiddleVector = new Vector3(UnityEngine.Random.Range(-s, s),
                                       UnityEngine.Random.Range(0f, 2f),
                                       UnityEngine.Random.Range(-s, s));
        }

        Debug.DrawRay(desiredPos + fiddleVector, Vector3.up * 3f, Color.yellow, 1f);

        // Move towards that pos
        rb.AddForce(fiddleVector, ForceMode.Impulse);
    }
    #endregion

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == 6)
        {
            // Colliding with Player
            //Debug.Log("Collision with Player");
            Destroy(gameObject);
        }
    }

    private void OnDestroy()
    {
        OnEnemyDeath?.Invoke();
    }
}

