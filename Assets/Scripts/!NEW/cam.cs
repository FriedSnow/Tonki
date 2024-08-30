using System.Collections;
using UnityEngine;

public class Cam : MonoBehaviour
{
    public Transform target;

    private void Start() {
       
    }
    private void Update() {
        transform.LookAt(target);
    }
}
