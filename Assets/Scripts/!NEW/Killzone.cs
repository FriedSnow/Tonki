using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Killzone : MonoBehaviour
{
    public int killzoneDamage = 9999;
    void OnCollisionEnter(Collision collision)
    {
        SupplyMethods.DealDamage(collision, killzoneDamage);
    }
}
