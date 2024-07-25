using UnityEngine;

public class HEATRound : MonoBehaviour
{
    public GameObject cumulativeJetPrefab; // Префаб кумулятивной струи
    public GameObject jetParticlesPrefab;
    public Transform jetPoint;
    public float jetLifetime = 2.0f; // Время жизни струи
    public int damage = 50; // Урон, наносимый струей
    public float explosionRadius = 5f;
    public float explosionForce = 10f;
    public float jetLength = 10.0f; // Длина струи
    // public GameObject explosionParticlesPrefab;
    private bool isArmed = false; // Снаряд взведен

    private void OnCollisionEnter(Collision collision)
    {
        if (!isArmed)
        {
            isArmed = true;
            return;
        }
        SupplyMethods.DealDamage(collision, 2);

        // Останавливаем снаряд
        GetComponent<Rigidbody>().velocity = Vector3.zero;
        GetComponent<Rigidbody>().isKinematic = true;

        // Спавним кумулятивную струю
        GameObject cumulativeJet = Instantiate(cumulativeJetPrefab, jetPoint.position, transform.rotation);
        Destroy(gameObject); // Уничтожаем снаряд

        // Настраиваем и отображаем кумулятивную струю
        LineRenderer lineRenderer = cumulativeJet.GetComponent<LineRenderer>();
        if (lineRenderer != null)
        {
            lineRenderer.SetPosition(0, transform.position);
            lineRenderer.SetPosition(1, transform.position + transform.forward * jetLength);
        }
        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius);
        foreach (Collider nearbyObject in colliders)
        {
            Rigidbody rb = nearbyObject.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.AddExplosionForce(explosionForce, transform.position, explosionRadius);
            }
        }

        // Уничтожаем струю через заданное время
        Destroy(cumulativeJet, jetLifetime);

        // Наносим урон всем объектам на пути струи
        RaycastHit[] hits = Physics.RaycastAll(transform.position, transform.forward, jetLength);
        foreach (RaycastHit hit in hits)
        {
            SupplyMethods.DealDamage(hit, damage);
        }
        if (jetParticlesPrefab != null)
        {
            GameObject jetParticles = Instantiate(jetParticlesPrefab, jetPoint.position, transform.rotation);
            Destroy(jetParticles, 3f); // Уничтожение частиц через 3 секунды
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

        // if (explosionParticlesPrefab != null)
        // {
        //     GameObject explosionParticles = Instantiate(explosionParticlesPrefab, transform.position, Quaternion.identity);
        //     Destroy(explosionParticles, 3f); // Уничтожение частиц через 3 секунды
        // }

        // Destroy(gameObject, 1f);
    }
}