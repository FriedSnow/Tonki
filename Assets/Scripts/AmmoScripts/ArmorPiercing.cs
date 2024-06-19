using UnityEngine;

public class ArmorPiercingBullet : MonoBehaviour
{
    public float damage = 20f;
    public float sabotDamage = 20f;
    public GameObject bulletPartPrefab;
    public int numOfParts = 3;
    public float spreadAngle = 15f;
    public float partSpeed = 10f;
    private int hitCount = 0;
    private ParticleSystem tracer;
    public GameObject explosionParticlesPrefab;

    void Start()
    {
        // Создаем дополнительные части
        for (int i = 0; i < numOfParts; i++)
        {
            GameObject part = Instantiate(bulletPartPrefab, transform.position, transform.rotation);
            float angle = (i - (numOfParts - 1) / 2f) * spreadAngle;
            part.transform.Rotate(0, angle, 0);

            Rigidbody rb = part.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.velocity = part.transform.forward * partSpeed;
            }
            else
            {
                Debug.LogWarning("Bullet part prefab does not have a Rigidbody component.");
            }

            BulletPart bulletPartScript = part.AddComponent<BulletPart>();
            bulletPartScript.damage = sabotDamage;
        }
        tracer = GetComponentInChildren<ParticleSystem>();
        if (tracer != null)
        {
            tracer.Play();
        }
    }

    private void OnTriggerEnter(Collider other)
    {

        Enemy enemy = other.GetComponent<Enemy>();
        if (enemy != null)
        {
            enemy.TakeDamage(damage);
            hitCount++;

            // Игнорировать дальнейшие столкновения с этим врагом
            Physics.IgnoreCollision(other, GetComponent<Collider>());

            // Уничтожить снаряд через 1 секунду после второго столкновения
            if (hitCount >= 2)
            {
                Destroy(gameObject, .1f);

            }
            if (explosionParticlesPrefab != null)
            {
                GameObject explosionParticles = Instantiate(explosionParticlesPrefab, transform.position, Quaternion.identity);
                Destroy(explosionParticles, 3f); // Уничтожение частиц через 3 секунды
            }
        }

    }
}

public class BulletPart : MonoBehaviour
{
    public float damage;

    private void OnTriggerEnter(Collider other)
    {
        Enemy enemy = other.GetComponent<Enemy>();
        if (enemy != null)
        {
            enemy.TakeDamage(damage);
        }

        // Удаляем часть сразу после попадания
        Destroy(gameObject);
    }
}