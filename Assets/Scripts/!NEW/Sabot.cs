using UnityEngine;

public class Sabot : MonoBehaviour
{

    public int sabotDamage = 20;

    void Start(){

    }

    void OnCollisionEnter(Collision collision)
    {

        if (collision.collider.CompareTag("Player"))
        {
            PlayerTankController playerTank = collision.collider.GetComponent<PlayerTankController>();
            if (playerTank != null)
            {
                playerTank.TakeDamage(sabotDamage);
            }
        }

        if (collision.collider.CompareTag("Enemy"))
        {
            EnemyTankController enemyTank = collision.collider.GetComponent<EnemyTankController>();
            if (enemyTank != null)
            {
                enemyTank.TakeDamage(sabotDamage);
            }
        }
    }
    private void OnTriggerEnter(Collider other){

        //Physics.IgnoreCollision(other, GetComponent<Collider>());
        GetComponent<Renderer>().enabled = false;
        Renderer[] childRenderers = GetComponentsInChildren<Renderer>();
        foreach (Renderer childRenderer in childRenderers)
        {
            childRenderer.enabled = false;
        }

    }
}
