using UnityEngine;

public class APRound : MonoBehaviour
{
    public int damage = 20;
    public float sabotDamage = 20f;
    public GameObject bulletPartPrefab;
    public GameObject explosionParticlesPrefab;
    public Rigidbody kostylb;
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
    private void Update()
    {
        if (kostylb.velocity.magnitude == 0)
        {
            Destroy(gameObject);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (kostylb.velocity.magnitude > 10)
        {
            SupplyMethods.DealDamage(collision, damage);
            if (collision.collider.CompareTag("Player") || collision.collider.CompareTag("Enemy"))
            {
                if (!isHit)
                {
                    if (explosionParticlesPrefab != null)
                    {
                        GameObject explosionParticles = Instantiate(explosionParticlesPrefab, transform.position, Quaternion.identity);
                        Destroy(explosionParticles, 3f); // Уничтожение частиц через 3 секунды
                    }
                    isHit = true;
                }
                Destroy(gameObject, .1f);
            }
            else
            {
                Destroy(gameObject, 1f);
            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        Physics.IgnoreCollision(other, GetComponent<Collider>());
        GetComponent<Renderer>().enabled = false;
        Renderer[] childRenderers = GetComponentsInChildren<Renderer>();
        foreach (Renderer childRenderer in childRenderers)
        {
            childRenderer.enabled = false;
        }


        // Destroy(gameObject, 1f);
    }
}
