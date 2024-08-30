using UnityEngine;

public class WeakSpot : MonoBehaviour
{
    private static bool weakHit = false;
    private static bool _weakHit = false;
    private void Update() {
        _weakHit = weakHit;
    }
    private void OnTriggerEnter(Collider other)
    {
        // Проверяем, является ли объект снарядом
        if (other.CompareTag("Round"))
        {
            weakHit = true;
        }
        else
            weakHit = false;

    }
    public static bool IsWeakHit(){
        return _weakHit;
    }
}