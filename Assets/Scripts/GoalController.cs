using UnityEngine;
using UnityEngine.Events;

public class GoalController : MonoBehaviour
{
    public UnityEvent onTriggerEnter;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ball"))
        {
            onTriggerEnter.Invoke();
        }
    }
}
