using UnityEngine;

public class ExplosiveObject : MonoBehaviour
{
    public float explosionForce = 70000f; // Сила взрыва
    public float explosionRadius = 500f;  // Радиус взрыва
    public GameObject explosionEffect;  // Эффект взрыва (например, частицы)

    private void OnCollisionEnter(Collision collision)
    {
        Explode();
    }

    void Explode()
    {
        // Если есть эффект взрыва, создаем его
        if (explosionEffect != null)
        {
            GameObject explosionParticles = Instantiate(explosionEffect, transform.position, Quaternion.identity);
            Destroy(explosionParticles, 3f); // Уничтожение частиц через 3 секунды
        }

        // Находим все объекты в радиусе взрыва
        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius);

        // Применяем силу взрыва ко всем найденным объектам
        foreach (Collider nearbyObject in colliders)
        {
            Rigidbody rb = nearbyObject.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.AddExplosionForce(explosionForce, transform.position, explosionRadius);
            }
        }

        // Уничтожаем взрывающийся объект
        Destroy(gameObject);
    }
}
