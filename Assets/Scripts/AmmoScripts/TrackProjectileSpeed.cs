using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackProjectileSpeed : MonoBehaviour
{
    private Rigidbody rb;
    private Vector3 previousPosition;
    private float currentSpeed;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        previousPosition = transform.position;
    }

    void FixedUpdate()
    {
        // Вычисляем текущую скорость
        Vector3 currentPosition = transform.position;
        float distance = Vector3.Distance(currentPosition, previousPosition);
        currentSpeed = distance / Time.fixedDeltaTime;
        previousPosition = currentPosition;

        // Можете использовать текущую скорость в другой логике
        Debug.Log("Current speed: " + currentSpeed + " m/s");
    }
}