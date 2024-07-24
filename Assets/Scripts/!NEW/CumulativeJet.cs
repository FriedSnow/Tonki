using UnityEngine;

public class CumulativeJet : MonoBehaviour
{
    private int damage;
    private float lifeTime;

    public void Initialize(int damage, float lifeTime)
    {
        this.damage = damage;
        this.lifeTime = lifeTime;
        Invoke("DestroyJet", lifeTime);
    }

    void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerTankController playerTank = other.GetComponent<PlayerTankController>();
            if (playerTank != null)
            {
                playerTank.TakeDamage(damage);
            }
        }

        if (other.CompareTag("Enemy"))
        {
            EnemyTankController enemyTank = other.GetComponent<EnemyTankController>();
            if (enemyTank != null)
            {
                enemyTank.TakeDamage(damage);
            }
        }
    }

    void DestroyJet()
    {
        Destroy(gameObject);
    }
}

