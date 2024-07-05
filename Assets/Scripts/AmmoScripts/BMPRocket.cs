using UnityEngine;

public class BMPRocket : MonoBehaviour
{
     public float speed = 10f;
    public float rotateSpeed = 200f;
    private Camera mainCamera;
    private Vector3 target;
    public float damage = 25f;
    public float explosionRadius = 5f;
    public float explosionForce = 10f;
    public GameObject explosionParticlesPrefab;

    public void SetCamera(Camera camera)
    {
        mainCamera = camera;
    }

    void Update()
    {
        if (mainCamera != null)
        {
            // Получаем позицию курсора на экране
            Vector3 mousePosition = Input.mousePosition;
            Ray ray = mainCamera.ScreenPointToRay(mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                // Устанавливаем цель по координатам курсора
                target = hit.point;
            }
            else
            {
                // Если луч не попадает в объект, устанавливаем цель по координатам на плоскости
                Plane plane = new Plane(Vector3.up, 0);
                if (plane.Raycast(ray, out float distance))
                {
                    target = ray.GetPoint(distance);
                }
            }

            Vector3 direction = (target - transform.position).normalized;

            // Поворот ракеты в направлении цели
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, lookRotation, rotateSpeed * Time.deltaTime);

            // Перемещение ракеты вперед
            transform.Translate(Vector3.forward * speed * Time.deltaTime);
        }
    }

void OnCollisionEnter(Collision collision)
    {
        
        // Обработка урона
        Enemy enemy = collision.gameObject.GetComponent<Enemy>();
        if (enemy != null)
        {
            enemy.TakeDamage(damage);
        }
        // Уничтожить снаряд после попадания
        Destroy(gameObject);
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

            // if (enemy != null)
            // {
            //     enemy.TakeDamage(damage);
            // }
        }
    }
}