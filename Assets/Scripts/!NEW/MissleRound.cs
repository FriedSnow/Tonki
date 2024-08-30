using System;
using System.Collections;
using UnityEngine;

public class MissleRound : MonoBehaviour
{
    public Transform target; // Цель, к которой направляем ракету
    public Transform engine;
    public Transform spawn;
    public ParticleSystem engineParticle;
    public GameObject he;
    public float thrustForce = 10f; // Сила тяги
    public float rotationForce = 10f; // Скорость поворота
    public float height = 10f;
    private bool rotationComplete = false;
    private bool isHit = false;

    private Rigidbody rb;
    private bool isEngineRunning = true;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        GetDistance();
        if (isEngineRunning)
        {
            rb.AddForceAtPosition(transform.up * thrustForce, engine.position, ForceMode.Force);  //main engine
        }
        if (rb.position.y > height && !rotationComplete)
        {
            if (!rotationComplete)
            {
                float currentZAngle = transform.eulerAngles.z;
                if (currentZAngle >= 90f && currentZAngle < 180f)
                {
                    rotationComplete = true;
                    // Останавливаем вращение
                    GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
                }
                else
                {
                    // Применяем крутящий момент
                    GetComponent<Rigidbody>().AddTorque(Vector3.forward * rotationForce);
                }
            }
            DisableEngine();
        }
        else if (rotationComplete)
        {
            Vector3 direction = target.position - transform.position;
            // Поворачиваем объект так, чтобы его верхняя часть смотрела на цель
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = targetRotation * Quaternion.Euler(90, 0, 0); // Корректируем

            StartEngine();
        }
    }
    void DisableEngine()
    {
        var emission = engineParticle.emission;
        emission.enabled = false;
        isEngineRunning = false;
    }
    void StartEngine()
    {
        var emission = engineParticle.emission;
        emission.enabled = true;
        isEngineRunning = true;
    }

    private IEnumerator Delay()
    {
        yield return new WaitForSeconds(3);
        rb.drag = 10;
        yield return new WaitForSeconds(2);
        rb.drag = 1;
    }
    void GetDistance()
    {
        float distance = Vector3.Distance(transform.position, target.transform.position);
        Debug.Log("Расстояние между объектами: " + distance);
        if (distance <= 2)
        {
            if (!isHit)
            {
                Instantiate(he, spawn.position, spawn.rotation);
                Destroy(gameObject);
                isHit = true;
            }
        }

    }
}