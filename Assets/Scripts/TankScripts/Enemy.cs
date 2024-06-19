using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float health = 100f;
    public Material grayMaterial;
    public GameObject burningParticlesPrefab;
    public GameObject turret;
    public GameObject body;
    public float turretThrowForce = 10f;
    private bool canBeDestroyed = true;
    private bool canBeDamaged = true;
    private CircularMovement circularMovement;
    private MoveTowardsPlayer moveTowardsPlayer;

    void Start()
    {
        circularMovement = GetComponent<CircularMovement>();
        moveTowardsPlayer = GetComponent<MoveTowardsPlayer>();
    }

    public void TakeDamage(float damage)
    {
        if (canBeDamaged){
            health -= damage;
        }
        if (health <= 0f && canBeDestroyed)
        {
            Die();
        }
    }

    void Die()
    {
        // Изменение текстуры на серую для всех рендереров
        Renderer[] renderers = GetComponentsInChildren<Renderer>();
        foreach (Renderer renderer in renderers)
        {
            if (renderer != null)
            {
                renderer.material = grayMaterial;
            }
        }

        // Создание частиц горения
        if (burningParticlesPrefab != null)
        {
            Instantiate(burningParticlesPrefab, transform.position, Quaternion.identity);
        }

        turret.transform.SetParent(null);
        Rigidbody detachedTurret = turret.AddComponent<Rigidbody>();
        detachedTurret.AddForce(Vector3.up * turretThrowForce, ForceMode.Impulse);
        detachedTurret.mass = 70;

        canBeDestroyed = false;
        canBeDamaged = false;
        
        if (circularMovement != null)
        {
            circularMovement.StopMovement();
        }
        if (moveTowardsPlayer != null)
        {
            moveTowardsPlayer.StopMovement();
        }
    }
}