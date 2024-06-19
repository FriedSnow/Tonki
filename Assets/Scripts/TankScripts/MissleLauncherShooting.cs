using UnityEngine;

public class MissileLauncherController : MonoBehaviour
{
    public GameObject missilePrefab;
    public Transform[] firePoints; // Массив из двух стволов
    public Camera mainCamera;
    public float fireRate = 1f; // Задержка между выстрелами в секундах

    private float nextFireTime = 0f;
    private int currentFirePoint = 0;

    void Update()
    {
        if (Input.GetButtonDown("Fire1") && Time.time >= nextFireTime)
        {
            nextFireTime = Time.time + fireRate;
            ShootMissile();
        }
    }

    void ShootMissile()
    {
        // Создаем снаряд в точке запуска
        GameObject missile = Instantiate(missilePrefab, firePoints[currentFirePoint].position, firePoints[currentFirePoint].rotation);
        MissileController missileController = missile.GetComponent<MissileController>();

        if (missileController != null)
        {
            // Устанавливаем цель ракеты на позицию объекта курсора
            Vector3 targetPosition = CursorObjectController.Instance.GetCursorPosition();
            missileController.SetTarget(targetPosition);
        }
        // Переключаемся на следующий ствол
        currentFirePoint = (currentFirePoint + 1) % firePoints.Length;
    }
}
