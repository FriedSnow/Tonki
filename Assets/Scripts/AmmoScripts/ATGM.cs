using UnityEngine;

public class Rocket : MonoBehaviour
{
    public float speed = 10f;

    private Transform target;

    public void SetTarget(Transform targetTransform)
    {
        target = targetTransform;
    }

    void Update()
    {
        if (target != null)
        {
            Vector3 direction = (target.position - transform.position).normalized;
            transform.position += direction * speed * Time.deltaTime;
        }
    }
}
