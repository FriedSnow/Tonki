using UnityEngine;

public class BulletSelfKill : MonoBehaviour
{
    public float lifespan = 5f;

    private void Start()
    {
        Destroy(gameObject, lifespan);
    }
}