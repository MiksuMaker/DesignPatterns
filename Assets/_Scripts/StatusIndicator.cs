using UnityEngine;
using TMPro;

public class StatusIndicator : MonoBehaviour
{
    [SerializeField] TextMeshPro statusText;

    private void Start()
    {
        FindObjectOfType<PlayerMover>().OnStateChange += UpdateStatusIndicator;
    }

    private void UpdateStatusIndicator(PlayerMover.State currentState)
    {

        statusText.text = currentState.ToString();
    }
}

