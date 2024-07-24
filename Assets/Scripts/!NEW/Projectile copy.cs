using UnityEngine;

public class Projectile2 : MonoBehaviour
{
    public GameObject explosionParticlesPrefab;
    public int damage = 20;
    public float explosionRadius = 5f;
    public float explosionForce = 10f;
    private bool isExploded = false;
    void Start(){

    }

    void OnCollisionEnter(Collision collision)
    {
        if (!isExploded){

        Explode();
        if (collision.collider.CompareTag("Player"))
        {
            PlayerTankController playerTank = collision.collider.GetComponent<PlayerTankController>();
            if (playerTank != null)
            {
                playerTank.TakeDamage(damage);
            }
        }

        if (collision.collider.CompareTag("Enemy"))
        {
            EnemyTankController enemyTank = collision.collider.GetComponent<EnemyTankController>();
            if (enemyTank != null)
            {
                enemyTank.TakeDamage(damage);
            }
        }
        }
    }
    private void OnTriggerEnter(Collider other){

        //Physics.IgnoreCollision(other, GetComponent<Collider>());
        GetComponent<Renderer>().enabled = false;
        Renderer[] childRenderers = GetComponentsInChildren<Renderer>();
        foreach (Renderer childRenderer in childRenderers)
        {
            childRenderer.enabled = false;
        }

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

            EnemyTankController enemyTank = nearbyObject.GetComponent<EnemyTankController>();
            if (enemyTank != null)
            {
                enemyTank.TakeDamage(damage);
            }
        }
        Destroy(gameObject);
    }
}
