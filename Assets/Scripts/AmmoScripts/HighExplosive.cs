using UnityEngine;

public class HighExplosiveBullet : BulletSelfKill
{
    public float explosionRadius = 5f;
    public float explosionForce = 10f;
    public float damage = 20f;
    public GameObject explosionParticlesPrefab;

    private void OnCollisionEnter(Collision collision)
    {
        Explode();
        Destroy(gameObject);
    }

    public void Explode()
    {
        // Создание частиц взрыва
        if (explosionParticlesPrefab != null)
        {
            GameObject explosionParticles = Instantiate(explosionParticlesPrefab, transform.position, Quaternion.identity);
            Destroy(explosionParticles, 3f); // Уничтожение частиц через 3 секунды
        }

        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius);
        foreach (Collider nearbyObject in colliders)
        {
            Rigidbody rb = nearbyObject.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.AddExplosionForce(explosionForce, transform.position, explosionRadius);
            }

            Enemy enemy = nearbyObject.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
            }
        }
    }
}