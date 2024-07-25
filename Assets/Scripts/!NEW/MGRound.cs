using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MGRound : MonoBehaviour
{
    public int damage = 2;
    void OnCollisionEnter(Collision collision)
    { 
        SupplyMethods.DealDamage(collision, damage);
        Destroy(gameObject);
    }
}