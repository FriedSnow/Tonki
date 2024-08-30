using UnityEngine;

public class RocketController : MonoBehaviour
{
    public float thrust = 10f;
    public float torque = 5f;
    public float targetHeight = 100f;
    public Transform target;
    public Transform engine;
    private Rigidbody rb;

    private bool hasLaunched = false;
    private bool hasReachedHeight = false;
    private bool hasTurnedHorizontal = false;
    private bool hasTurnedToTarget = false;

    void Start()
    {
        if (rb == null)
        {
            rb = GetComponent<Rigidbody>();
        }
        Launch();
    }

    void Update()
    {
        if (!hasLaunched)
        {
            Launch();
        }
        else if (!hasReachedHeight && transform.position.y >= targetHeight)
        {
            StopEngines();
            hasReachedHeight = true;
        }
        else if (hasReachedHeight && !hasTurnedHorizontal)
        {
            TurnHorizontal();
        }
        else if (hasTurnedHorizontal && !hasTurnedToTarget)
        {
            TurnTowardsTarget();
        }
        else if (hasTurnedToTarget)
        {
            FlyToTarget();
        }
    }

    void Launch()
    {
        rb.AddForceAtPosition(Vector3.up * thrust, engine.position, ForceMode.Force);
        hasLaunched = true;
    }

    void StopEngines()
    {
        rb.velocity = Vector3.zero;
    }

    void TurnHorizontal()
    {
        Quaternion targetRotation = Quaternion.Euler(0, 0, 90);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, torque * Time.deltaTime);

        if (Quaternion.Angle(transform.rotation, targetRotation) < 0.1f)
        {
            hasTurnedHorizontal = true;
        }
    }

    void TurnTowardsTarget()
    {
        Vector3 directionToTarget = (target.position - transform.position).normalized;
        Quaternion targetRotation = Quaternion.LookRotation(directionToTarget);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, torque * Time.deltaTime);

        if (Quaternion.Angle(transform.rotation, targetRotation) < 0.1f)
        {
            hasTurnedToTarget = true;
        }
    }

    void FlyToTarget()
    {
        rb.AddForceAtPosition(Vector3.forward * thrust, engine.position, ForceMode.Force);
    }
}