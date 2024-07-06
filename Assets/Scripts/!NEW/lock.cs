using UnityEngine;

public class LockPosition : MonoBehaviour
{
    private Vector3 initialPosition;
    private Quaternion initialRotation;

    void Start()
    {
        initialPosition = transform.position;
        initialRotation = transform.rotation;
    }

    void Update()
    {
        transform.position = initialPosition;
        transform.rotation = initialRotation;
    }
}