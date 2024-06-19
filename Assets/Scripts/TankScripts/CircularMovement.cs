using UnityEngine;

public class CircularMovement : MonoBehaviour
{
    public Transform centerPoint;
    public float radius = 5f;
    public float speed = 2f;
    private float angle;
    private bool isAlive = true;

    void Update()
    {
        if (!isAlive) return;

        angle += speed * Time.deltaTime;
        float x = Mathf.Cos(angle) * radius;
        float z = Mathf.Sin(angle) * radius;

        Vector3 newPosition = new Vector3(centerPoint.position.x + x, transform.position.y, centerPoint.position.z + z);
        Vector3 direction = (newPosition - transform.position).normalized;

        // Поворачиваем врага в направлении движения
        transform.forward = direction;

        transform.position = newPosition;
    }

    public void StopMovement()
    {
        isAlive = false;
    }
}