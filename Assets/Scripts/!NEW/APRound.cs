using UnityEngine;

public class APRound : MonoBehaviour
{
    public int damage = 20;
    public float sabotDamage = 20f;
    public GameObject bulletPartPrefab;
    public GameObject explosionParticlesPrefab;
    public int numOfParts = 3;
    public float spreadAngle = 15f;
    public float partSpeed = 100f;
    private bool isHit = false;
    void Start()
    {
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
    }

    void OnCollisionEnter(Collision collision)
    {
        SupplyMethods.DealDamage(collision, damage);
        if (!isHit)
        {
            if (explosionParticlesPrefab != null)
            {
                GameObject explosionParticles = Instantiate(explosionParticlesPrefab, transform.position, Quaternion.identity);
                Destroy(explosionParticles, 3f); // Уничтожение частиц через 3 секунды
            }
            isHit = true;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        //Physics.IgnoreCollision(other, GetComponent<Collider>());
        GetComponent<Renderer>().enabled = false;
        Renderer[] childRenderers = GetComponentsInChildren<Renderer>();
        foreach (Renderer childRenderer in childRenderers)
        {
            childRenderer.enabled = false;
        }


        // Destroy(gameObject, 1f);
    }
}
