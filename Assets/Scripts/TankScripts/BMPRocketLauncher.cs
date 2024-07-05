using UnityEngine;

public class BMPRocketLauncher : MonoBehaviour
{
    public GameObject missilePrefab;
    public Transform[] missileLaunchPoints; // Точки запуска для ракет
    public Camera mainCamera;
    public float missileFireRate = 1f; // Задержка между выстрелами ракет

    private float nextMissileFireTime = 0f;
    private int currentLaunchPointIndex = 0;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R) && Time.time >= nextMissileFireTime)
        {
            nextMissileFireTime = Time.time + missileFireRate;
            FireMissile();
        }
    }

    void FireMissile()
    {
        if (missileLaunchPoints.Length == 0)
            return;

        Transform launchPoint = missileLaunchPoints[currentLaunchPointIndex];
        currentLaunchPointIndex = (currentLaunchPointIndex + 1) % missileLaunchPoints.Length;

        ShootMissile(launchPoint);
    }

    void ShootMissile(Transform launchPoint)
    {
        GameObject missile = Instantiate(missilePrefab, launchPoint.position, launchPoint.rotation);
        BMPRocket missileController = missile.GetComponent<BMPRocket>();

        if (missileController != null)
        {
            missileController.SetCamera(mainCamera);
        }
    }
}
