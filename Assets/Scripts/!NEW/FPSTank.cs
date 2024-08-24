using UnityEngine;

public class FPSTank : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float turnSpeed = 50f;
    public float rotationSpeed = 100f;
    
    public Transform turret;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        HandleMovement();
        FPSTurretRotation();
    }

    void HandleMovement()
    {
        float move = Input.GetAxis("Vertical") * moveSpeed * Time.deltaTime;
        float turn = Input.GetAxis("Horizontal") * turnSpeed * Time.deltaTime;

        transform.Translate(Vector3.forward * move);
        transform.Rotate(Vector3.up * turn);
    }

    void FPSTurretRotation()
    {
        float mouseX = Input.GetAxis("Mouse X");

        // Поворачиваем башню по горизонтали
        turret.Rotate(Vector3.up * mouseX * rotationSpeed * Time.deltaTime);
    }
}