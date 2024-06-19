using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankController : MonoBehaviour
{
    public float moveSpeed = 15f;
    public float turnSpeed = 120f;

    void Update()
    {
        float move = Input.GetAxis("Vertical") * moveSpeed * Time.deltaTime;
        float turn = Input.GetAxis("Horizontal") * turnSpeed * Time.deltaTime;

        transform.Translate(0, 0, move);
        transform.Rotate(0, turn, 0);
    }
}