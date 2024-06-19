using UnityEngine;

public class MoveTowardsPlayer : MonoBehaviour
{
    public Transform player;
    public float speed = 3f;
    public float rotationSpeed = 2f;
    private bool isAlive = true;

    void Update()
    {
        if (!isAlive) return;

        Vector3 direction = (player.position - transform.position).normalized;
        direction.y = 0; // Игнорируем вертикальную составляющую направления

        // Поворачиваем врага в сторону игрока постепенно
        if (direction != Vector3.zero)
        {
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);
        }

        // Двигаемся вперед только по горизонтали
        Vector3 moveDirection = transform.forward;
        moveDirection.y = 0; // Убираем вертикальную составляющую движения
        transform.position += moveDirection * speed * Time.deltaTime;
    }

    public void StopMovement()
    {
        isAlive = false;
    }
}
