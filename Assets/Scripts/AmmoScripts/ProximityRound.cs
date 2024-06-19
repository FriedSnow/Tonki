using UnityEngine;

public class ProximityProjectile : Bullet
{
    public float speed = 100f; // Скорость снаряда
    public float detectionRadius = 10f; // Радиус срабатывания радиовзрывателя
    public float explosionRadius = 5f; // Радиус взрыва
    public float explosionForce = 700f; // Сила взрыва
    public GameObject explosionParticlesPrefab;

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.velocity = transform.forward * speed;
    }

    void Update()
    {
        CheckForEnemies();
    }

    void CheckForEnemies()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, detectionRadius);
        foreach (Collider collider in colliders)
        {
            if (collider.CompareTag("Enemy")) // Убедитесь, что враги имеют тег "Enemy"
            {
                Explode();
                break;
            }
        }
    }

    void Explode()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius);
        foreach (Collider collider in colliders)
        {
            Rigidbody rb = collider.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.AddExplosionForce(explosionForce, transform.position, explosionRadius);
            }

            Enemy enemy = collider.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
            }
        }
        if (explosionParticlesPrefab != null)
        {
            GameObject explosionParticles = Instantiate(explosionParticlesPrefab, transform.position, Quaternion.identity);
            Destroy(explosionParticles, 3f); // Уничтожение частиц через 3 секунды
        }

        Destroy(gameObject); // Уничтожаем снаряд после взрыва
    }

    void OnCollisionEnter(Collision collision)
    {
        Explode();
    }
}