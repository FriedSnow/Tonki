using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Killzone : MonoBehaviour
{
    public int killzoneDamage = 9999;
    void OnCollisionEnter(Collision collision)
    {

        if (collision.collider.CompareTag("Player"))
        {
            PlayerTankController playerTank = collision.collider.GetComponent<PlayerTankController>();
            if (playerTank != null)
            {
                playerTank.TakeDamage(killzoneDamage);
            }
        }

        if (collision.collider.CompareTag("Enemy"))
        {
            EnemyTankController enemyTank = collision.collider.GetComponent<EnemyTankController>();
            if (enemyTank != null)
            {
                enemyTank.TakeDamage(killzoneDamage);
            }
        }
    }
}
