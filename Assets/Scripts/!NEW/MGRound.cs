using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MGRound : MonoBehaviour
{
    public GameObject explosionParticlesPrefab;
    public int damage = 2;
    void OnCollisionEnter(Collision collision)
    {
        SupplyMethods.DealDamage(collision, damage);
        if (!collision.collider.CompareTag("Player"))
        {
            if (explosionParticlesPrefab != null)
            {
                GameObject explosionParticles = Instantiate(explosionParticlesPrefab, transform.position, Quaternion.identity);
                Destroy(explosionParticles, 3f); // Уничтожение частиц через 3 секунды
            }
        }
        Destroy(gameObject);
    }
}