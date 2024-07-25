using UnityEngine;

public class BreakableObject : MonoBehaviour
{
    public int health = 1; // Примерное значение здоровья

    void Start()
    {
        // Инициализация, если необходимо
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        // Отсоединяем все дочерние объекты
        foreach (Transform child in transform)
        {
            child.SetParent(null);
            Rigidbody rb = child.gameObject.AddComponent<Rigidbody>();
            rb.AddExplosionForce(500f, transform.position, 5f); // Добавляем силу взрыва для эффекта "развала"
        }

        // Удаляем главный объект
        Destroy(gameObject);
    }
}