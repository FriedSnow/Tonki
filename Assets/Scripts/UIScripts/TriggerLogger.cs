using UnityEngine;

public class TriggerLogger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Trigger entered!");
    }

    private void OnTriggerStay(Collider other)
    {
        Debug.Log("Trigger is being held!");
    }

    private void OnTriggerExit(Collider other)
    {
        Debug.Log("Trigger exited!");
    }
}