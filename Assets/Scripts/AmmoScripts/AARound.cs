using UnityEngine;

public class AntiAircraftProjectile : Bullet
{
    public float speed = 100f; // Скорость снаряда
    public float explosionRadius = 5f; // Радиус взрыва
    public float explosionForce = 700f; // Сила взрыва

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.velocity = transform.forward * speed;
        damage = 5f;
    }

    void OnCollisionEnter(Collision collision)
    {
        Explode();
    }

    void Explode()
    {
        // Получаем все объекты в радиусе взрыва
        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius);
        foreach (Collider collider in colliders)
        {
            Rigidbody rb = collider.GetComponent<Rigidbody>();
            if (rb != null)
            {
                // Добавляем взрывную силу
                rb.AddExplosionForce(explosionForce, transform.position, explosionRadius);
            }

            // Если объект имеет компонент Enemy, наносим ему урон
            Enemy enemy = collider.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
            }
        }

        // Уничтожаем снаряд после взрыва
        Destroy(gameObject, 0.05f);
    }
}