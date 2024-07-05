using UnityEngine;

public class ArmorPiercingBullet2 : MonoBehaviour
{
    public float damage = 15f;
    private ParticleSystem tracer;
    private Renderer bulletRenderer;
    public GameObject explosionParticlesPrefab;

    private bool damageApplied = false;
    [SerializeField] MeshRenderer meshRenderer;
    void Start()
    {
        tracer = GetComponentInChildren<ParticleSystem>();
        if (tracer != null)
        {
            tracer.Play();
        }
        bulletRenderer = GetComponent<Renderer>();
    }

    private void OnTriggerEnter(Collider other)
    {
        Enemy enemy = other.GetComponent<Enemy>();
        if (enemy != null && !damageApplied)
        {
            enemy.TakeDamage(damage);
            damageApplied = true;

            if (explosionParticlesPrefab != null)
            {
                GameObject explosionParticles = Instantiate(explosionParticlesPrefab, transform.position, Quaternion.identity);
                Destroy(explosionParticles, 3f);
            }

            // Сделать снаряд невидимым
      

            Debug.Log("contact");
        GetComponent<Renderer>().enabled = false;

        // Если у объекта есть дочерние объекты с рендерерами, скрываем и их
        Renderer[] childRenderers = GetComponentsInChildren<Renderer>();
        foreach (Renderer childRenderer in childRenderers)
        {
            childRenderer.enabled = false;
        }
        }
    }


}

