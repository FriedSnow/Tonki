using UnityEngine;

public class Projectile : MonoBehaviour
{
    public int damage = 20;
    public float sabotDamage = 20f;
    public GameObject bulletPartPrefab;
    public int numOfParts = 3;
    public float spreadAngle = 15f;
    public float partSpeed = 10f;
    void Start(){
        // for (int i = 0; i < numOfParts; i++)
        // {
        //     GameObject part = Instantiate(bulletPartPrefab, transform.position, transform.rotation);
        //     float angle = (i - (numOfParts - 1) / 2f) * spreadAngle;
        //     part.transform.Rotate(0, angle, 0);

        //     Rigidbody rb = part.GetComponent<Rigidbody>();
        //     if (rb != null)
        //     {
        //         rb.velocity = part.transform.forward * partSpeed;
        //     }
        //     else
        //     {
        //         Debug.LogWarning("Bullet part prefab does not have a Rigidbody component.");
        //     }

        //     BulletPart bulletPartScript = part.AddComponent<BulletPart>();
        //     bulletPartScript.damage = sabotDamage;
        // }
    }

    void OnCollisionEnter(Collision collision)
    {

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

        Destroy(gameObject);
    }
    private void OnTriggerEnter(Collider other){

        Physics.IgnoreCollision(other, GetComponent<Collider>());

    }
}
