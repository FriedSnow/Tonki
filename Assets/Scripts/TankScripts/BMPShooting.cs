using UnityEngine;

public class BMPShooting : MonoBehaviour
{
    public GameObject rocketPrefab;
    public Transform[] firePoints; // Добавьте сюда 4 точки стрельбы
    public Transform cursorTarget;

    private int currentFirePointIndex = 0;

    void Update()
    {
        if (Input.GetButtonDown("Fire1")) // Стрельба по нажатию кнопки мыши
        {
            Shoot();
        }
    }

    void Shoot()
    {
        if (firePoints.Length == 0 || rocketPrefab == null || cursorTarget == null)
        {
            Debug.LogWarning("Не все объекты настроены для стрельбы!");
            return;
        }

        // Определение текущей точки стрельбы
        Transform firePoint = firePoints[currentFirePointIndex];

        // Создание ракеты
        GameObject rocketInstance = Instantiate(rocketPrefab, firePoint.position, firePoint.rotation);

        // Установка цели для ракеты
        Rocket rocket = rocketInstance.GetComponent<Rocket>();
        if (rocket != null)
        {
            rocket.SetTarget(cursorTarget);
        }

        // Переключение на следующую точку стрельбы
        currentFirePointIndex = (currentFirePointIndex + 1) % firePoints.Length;
    }
}
