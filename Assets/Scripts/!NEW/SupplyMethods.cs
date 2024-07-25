using UnityEngine;

public class SupplyMethods : MonoBehaviour
{
    public static void DealDamage(Collision collision, int damage)
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
        if (collision.collider.CompareTag("Breakable"))
        {
            BreakableObject breakable = collision.collider.GetComponent<BreakableObject>();
            if (breakable != null)
            {
                breakable.TakeDamage(damage);
            }
        }
    }
    public static void DealDamage(RaycastHit hit, int damage)
    {
        if (hit.collider.CompareTag("Player"))
        {
            PlayerTankController playerTank = hit.collider.GetComponent<PlayerTankController>();
            if (playerTank != null)
            {
                playerTank.TakeDamage(damage);
            }
        }
        if (hit.collider.CompareTag("Enemy"))
        {
            EnemyTankController enemyTank = hit.collider.GetComponent<EnemyTankController>();
            if (enemyTank != null)
            {
                enemyTank.TakeDamage(damage);
            }
        }
        if (hit.collider.CompareTag("Breakable"))
        {
            BreakableObject breakable = hit.collider.GetComponent<BreakableObject>();
            if (breakable != null)
            {
                breakable.TakeDamage(damage);
            }
        }
    }
}