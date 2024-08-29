using System;
using System.Collections;
using System.Data.Common;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerTankController : MonoBehaviour
{
    public float APSpeed = 200f;
    public float HEATSpeed = 200f;
    public float HESpeed = 100f;
    public float fireRate = 1f;
    public float mgFireRate = 0.1f;
    public float mgProjectileSpeed = 300f;
    public float mgRotateSpeed = 10f;
    public float maxSpeed = 15f;                // Максимальная скорость танка
    public float maxAngularVelocity = 2f;       //максимальная скорость поворота корпуса
    public float normal = 15f;
    public float slow = 5f;
    public float acceleration = 5f;             // Ускорение танка
    public float rotateAcceleration = 10f;      // Ускорение поворота танка
    public float projectileSpeed = 200f;        //скорость снаряда по умолчанию
    public float recoilForce = 2000f;           //отдача после выстрела главного орудия
    public float rotateSpeed = 100f;            //скорость поворота корпуса
    public float turretRotateSpeed = 5f;        //скорость поворота башни
    public float fpsSpeed = 10f;                //скорость поворота башни в прицеле
    public GameObject burningParticlesPrefab;   //частицы горения танка после уничтожения
    public GameObject explosionParticlesPrefab; //частицы взрыва танка после уничтожения
    public GameObject mgProjectilePrefab;       //снаряд для зенитного пулемета
    public GameObject[] projectilePrefabs;      //массив типов снарядов танка
    public GameObject shootParticlesPrefab;     //частицы выстрела
    public GameObject lightParticlesPrefab;     //частицы выстрела
    public GameObject shootParticlesMGPrefab;   //частицы выстрела
    public GameObject lightMGParticlesPrefab;   //частицы выстрела
    public GameObject tank;                     //обьект танка
    public Material grayMaterial;               //материал уничтоженного танка
    public Rigidbody tankRigidbody;             //еще один обьект танка
    public Slider[] healthBar;                  //ссылка на UI элемент Slider
    public Text[] restartingText;               //ссылка на UI текста выводимого при рестарте
    public Text[] ammoText;                     //ссылка на UI текста 
    public Text[] mgAmmoText;                   //ссылка на UI текста 
    public Transform firePoint;                 //точка выстрела
    public Transform recoilPoint;               //точка отдачи
    public Transform machineGun;                //объект зенитного пулемета
    public Transform machineGunFirePoint;       //точка выстрела зенитного пулемета
    public Transform tankTransform;             //и еще один обьект танка
    public Transform turret;                    //обьект башни
    public int ammo = 20;
    public int mgAmmo = 2000;
    public static int health = 200;
    public int maxHealth = 200;
    public bool isFPV = false;
    public Camera[] cameras;                    // Массив всех камер в сцене
    public RawImage turretUI;
    public Image reloadImage;

    // ---------- ---------- ---------- ---------- ---------- ---------- ----------

    private int currentCameraIndex = 0;         // Индекс текущей камеры
    public static int _currentCameraIndex;         // Индекс текущей камеры
    private bool canBeDestroyed = true;
    private bool isDestroyed = false;
    private float nextFireTime = 0f;            //таймер основного орудия
    private float nextMGFireTime = 0f;          //таймер зенитного пулемета
    private int currentProjectileIndex = 0;     //индекс выбранного снаряда в массиве типов
    private Gradient gradient;                  //градиент для смены цветов
    private Image fillImage1;                   //ссылка на Image компонента Fill
    private Image fillImage2;                   //ссылка на Image компонента Fill
    private float speedMultiplier = 1f;
    private float targetFOV;
    void Start()
    {
        projectileSpeed = APSpeed;
        health = maxHealth;
        UpdateHealthBarColor();
        targetFOV = cameras[1].fieldOfView;
        reloadImage.fillAmount = 0f;
    }
    private void Update()
    {
        if (!isDestroyed)
        {
            SwitchCameraCheck();
            MoveTank();
            HandleShooting();
            HandleProjectileSwitching();
            UpdateAmmoText();
            UpdateHealthBarColor();
            RotateUI();
            CameraZoom();
            kekW();
            if (!isFPV)
            {
                RotateTurret();
                RotateMachineGun();
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
            else
            {
                FPVTurretRotation();
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
            _currentCameraIndex = currentCameraIndex;
        }
        // в следующих 4 строках нет ничего святого
        if (GroundCheck.Check() && !IsAngleExceeded(tank, 30f))
            speedMultiplier = normal;
        else
            speedMultiplier = slow;
    }
    private void OnTriggerEnter(Collider other)
    {
        CheckAmmoBox(other);
    }
    void MoveTank()
    {
        float rotateInput = Input.GetAxis("Horizontal");
        float moveInput = Input.GetAxis("Vertical");
        float currentSpeed = tankRigidbody.velocity.magnitude;

        //вперед - назад
        if (tankTransform.rotation.x <= 10f && tankTransform.rotation.x >= -10f)
        {
            if (moveInput > 0 && currentSpeed < maxSpeed)
            {
                tankRigidbody.AddForce(transform.forward * acceleration * moveInput * speedMultiplier, ForceMode.Force);
            }
            else if (moveInput < 0 && currentSpeed < maxSpeed)
            {
                tankRigidbody.AddForce(transform.forward * acceleration * moveInput * speedMultiplier * .25f, ForceMode.Force);
            }
        }

        // ограничение максимальной скорости
        if (currentSpeed > maxSpeed)
        {
            tankRigidbody.velocity = tankRigidbody.velocity.normalized * maxSpeed;
        }

        //влево - вправо
        if (rotateInput > 0)
        {
            // Применяем крутящий момент влево
            tankRigidbody.AddTorque(Vector3.up * rotateAcceleration * speedMultiplier);
        }
        else if (rotateInput < 0)
        {
            // Применяем крутящий момент вправо
            tankRigidbody.AddTorque(Vector3.up * -rotateAcceleration * speedMultiplier);
        }
        else if (rotateInput == 0)
        {
            Vector3 angularVelocity = tankRigidbody.angularVelocity;
            angularVelocity.y = 0;
            tankRigidbody.angularVelocity = angularVelocity;
        }
        tankRigidbody.angularVelocity = Vector3.ClampMagnitude(tankRigidbody.angularVelocity, maxAngularVelocity);

        // Выводим угол скольжения в консоль
        Vector3 velocityDirection = tankRigidbody.velocity.normalized;
        float angle = Vector3.Angle(transform.forward, velocityDirection);
        // if (currentSpeed >= 1f)
        //     Debug.Log("Угол скольжения: " + Math.Round(angle, 2) + " Скорость: " + Math.Round(currentSpeed, 2));
    }
    Vector3 GetMouseWorldPosition()
    {
        Plane plane = new Plane(Vector3.up, tankTransform.position.y);
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        float distance;
        if (plane.Raycast(ray, out distance))
        {
            return ray.GetPoint(distance);
        }
        return Vector3.zero;
    }

    void RotateTurret()
    {
        Vector3 targetPosition = GetMouseWorldPosition();
        Vector3 direction = targetPosition - turret.position;
        direction = tankTransform.InverseTransformDirection(direction);
        direction.y = 0;

        Quaternion targetRotation = Quaternion.LookRotation(tankTransform.TransformDirection(direction), tankTransform.up);
        turret.rotation = Quaternion.Lerp(turret.rotation, targetRotation, Time.deltaTime * turretRotateSpeed);
    }

    void FPVTurretRotation()
    {
        float mouseX = Input.GetAxis("Mouse X");
        // Поворачиваем башню по горизонтали
        turret.Rotate(Vector3.up * mouseX * turretRotateSpeed * fpsSpeed * Time.deltaTime);
        machineGun.localEulerAngles = new Vector3(0, 0, 0);
    }

    void RotateMachineGun()
    {
        Vector3 targetPosition = GetMouseWorldPosition();
        Vector3 direction = targetPosition - machineGun.position;
        direction = tankTransform.InverseTransformDirection(direction);
        direction.y = 0;

        Quaternion targetRotation = Quaternion.LookRotation(tankTransform.TransformDirection(direction), tankTransform.up);
        machineGun.rotation = Quaternion.Lerp(machineGun.rotation, targetRotation, Time.deltaTime * mgRotateSpeed);
    }

    void HandleShooting()
    {
        if (Input.GetMouseButtonDown(0) && Time.time >= nextFireTime && ammo > 0)
        {
            FireProjectile();
            nextFireTime = Time.time + fireRate;
            ammo--;
        }

        if (Input.GetKey(KeyCode.Space) && Time.time >= nextMGFireTime && mgAmmo > 0)
        {
            FireMGProjectile();
            nextMGFireTime = Time.time + mgFireRate;
            mgAmmo--;
        }
    }

    void HandleProjectileSwitching()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            currentProjectileIndex = 0;
            projectileSpeed = APSpeed;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            currentProjectileIndex = 1;
            projectileSpeed = HEATSpeed;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            currentProjectileIndex = 2;
            projectileSpeed = HESpeed;
        }
    }

    void FireProjectile()
    {
        if (currentProjectileIndex >= 0 && currentProjectileIndex < projectilePrefabs.Length)
        {
            StartCoroutine(ReloadCoroutine());
            GameObject projectile = Instantiate(projectilePrefabs[currentProjectileIndex], firePoint.position, firePoint.rotation);
            Rigidbody rb = projectile.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.velocity = firePoint.forward * projectileSpeed;
            }
            Destroy(projectile, 5f);
            if (shootParticlesPrefab && lightParticlesPrefab != null)
            {
                GameObject shootParticles = Instantiate(shootParticlesPrefab, firePoint.position, firePoint.rotation);
                GameObject light = Instantiate(lightParticlesPrefab, firePoint.position, firePoint.rotation);
                Destroy(shootParticles, .2f);
                Destroy(light, .075f);
            }
            if (tankRigidbody != null)
            {
                // tankRigidbody.AddForce(-recoilPoint.forward * recoilForce * 10f, ForceMode.Impulse);
            }
            tankRigidbody.AddTorque(-turret.transform.right * recoilForce, ForceMode.Impulse);

            // transform.RotateAround(recoilPoint.position, -recoilPoint.right, 5f);
        }
    }

    void FireMGProjectile()
    {
        GameObject mgProjectile = Instantiate(mgProjectilePrefab, machineGunFirePoint.position, machineGunFirePoint.rotation);
        Rigidbody rb = mgProjectile.GetComponent<Rigidbody>();
        if (shootParticlesMGPrefab && lightMGParticlesPrefab != null)
        {
            GameObject shootParticlesMG = Instantiate(shootParticlesMGPrefab, machineGunFirePoint.position, machineGunFirePoint.rotation);
            GameObject MGlight = Instantiate(lightMGParticlesPrefab, machineGunFirePoint.position, machineGunFirePoint.rotation);
            Destroy(shootParticlesMG, 3f);
            Destroy(MGlight, .075f);
        }
        if (rb != null)
        {
            rb.velocity = machineGunFirePoint.forward * mgProjectileSpeed;
        }
        Destroy(mgProjectile, 5f);
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        if (healthBar != null)
        {
            healthBar[0].value = health;
            healthBar[1].value = health;
            UpdateHealthBarColor();
        }
        if (health <= 0)
        {
            Die();
        }
    }
    void UpdateHealthBarColor()
    {
        //if (healthBar != null)
        {
            healthBar[0].maxValue = maxHealth;
            healthBar[0].value = health;
            healthBar[1].maxValue = maxHealth;
            healthBar[1].value = health;

            // Получаем ссылку на Image компонента Fill
            fillImage1 = healthBar[0].fillRect.GetComponent<Image>();
            fillImage2 = healthBar[1].fillRect.GetComponent<Image>();

            // Настраиваем градиент
            gradient = new Gradient();
            gradient.SetKeys(
                new GradientColorKey[] {
                    new GradientColorKey(Color.green, 1f),
                    new GradientColorKey(Color.yellow, 0.5f),
                    new GradientColorKey(Color.red, 0f)
                },
                new GradientAlphaKey[] {
                    new GradientAlphaKey(1f, 0f),
                    new GradientAlphaKey(1f, 1f)
                }
            );

        }
        if (fillImage1 != null && gradient != null)
        {
            float normalizedHealth = (float)health / maxHealth;
            fillImage1.color = gradient.Evaluate(normalizedHealth);
            fillImage2.color = gradient.Evaluate(normalizedHealth);
        }
    }
    void UpdateAmmoText()
    {
        ammoText[0].text = ammo.ToString();
        mgAmmoText[0].text = mgAmmo.ToString();
        ammoText[1].text = ammo.ToString();
        mgAmmoText[1].text = mgAmmo.ToString();
    }

    void Die()
    {
        isDestroyed = true;
        if (canBeDestroyed)
        {
            ShowRestartingMessage();
            Invoke(nameof(RestartScene), 3f);
            Invoke(nameof(RemoveTankModel), 3f);
            Renderer[] renderers = GetComponentsInChildren<Renderer>();
            foreach (Renderer renderer in renderers)
            {
                if (renderer != null)
                {
                    renderer.material = grayMaterial;
                }
            }
            if (burningParticlesPrefab != null)
            {
                Instantiate(burningParticlesPrefab, transform.position, Quaternion.identity);
            }
            if (explosionParticlesPrefab != null)
            {
                GameObject explosionParticles = Instantiate(explosionParticlesPrefab, transform.position, Quaternion.identity);
                Destroy(explosionParticles, 3f);
            }
            if (EnemyManager.score > GetRecord())
            {
                SetRecord(EnemyManager.score);
            }

            canBeDestroyed = false;
            EnemyManager.score = 0;
        }
    }

    void ShowRestartingMessage()
    {
        if (restartingText != null)
        {
            restartingText[0].text = $"{Values.restartLose} Score: {EnemyManager.score} Record: {GetRecord()}";
            restartingText[1].text = $"{Values.restartLose} Score: {EnemyManager.score} Record: {GetRecord()}";
        }
    }

    void RemoveTankModel()
    {
        Destroy(tank);
    }

    void RestartScene()
    {
        Debug.Log("Перезапуск сцены...");
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public bool IsDestroyed()
    {
        return isDestroyed;
    }

    void CheckAmmoBox(Collider other)
    {
        if (other.gameObject != gameObject)
        {
            // Проверяем, что тег объекта-триггера совпадает с одним из тегов, которые мы хотим отслеживать
            if (other.CompareTag("Ammo"))
            {
                ammo += 2;
                mgAmmo += 100;
                EnemyManager.score += 25;
                return; // Выходим из метода, так как мы уже обработали столкновение
            }
            else if (other.CompareTag("HP") && health < maxHealth)
            {
                health += 100;
                EnemyManager.score += 25;
                UpdateHealthBarColor();
                return; // Выходим из метода, так как мы уже обработали столкновение
            }

        }

    }
    public bool IsAngleExceeded(GameObject obj, float maxAngle)
    {
        // Получаем угол поворота объекта
        float angle = obj.transform.eulerAngles.x;

        // Нормализуем угол в диапазон от -180 до 180 градусов
        if (angle > 180f)
        {
            angle -= 360f;
        }

        Debug.Log($"{angle} {Mathf.Abs(angle) > maxAngle}");
        // Проверяем, превышает ли угол заданное значение
        return Mathf.Abs(angle) > maxAngle;
    }

    void SwitchCameraCheck()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            // Отключаем предыдущую камеру
            cameras[currentCameraIndex].gameObject.SetActive(false);

            // Увеличиваем индекс текущей камеры
            currentCameraIndex++;

            // Если индекс превышает количество камер, сбрасываем его до 0
            if (currentCameraIndex >= cameras.Length)
            {
                currentCameraIndex = 0;
            }
            // Включаем новую текущую камеру
            cameras[currentCameraIndex].gameObject.SetActive(true);
            isFPV = !isFPV;
        }
    }

    void RotateUI()
    {
        // Получаем текущий угол поворота башни по оси Y
        float turretRotation = turret.localEulerAngles.y;
        // Применяем этот угол к RawImage
        turretUI.rectTransform.rotation = Quaternion.Euler(0, 0, -turretRotation);
    }
    private IEnumerator ReloadCoroutine()
    {
        float timer = 0f;
        reloadImage.fillAmount = 0f;

        while (timer < fireRate)
        {
            timer += Time.deltaTime;
            reloadImage.fillAmount = timer / fireRate;
            yield return null;
        }

        reloadImage.fillAmount = 0f; // Скрыть или сбросить заполнение после завершения
    }


    void CameraZoom()
    {
        float scrollDelta = Input.mouseScrollDelta.y;
        if (scrollDelta != 0)
        {
            targetFOV -= scrollDelta * 10;
            targetFOV = Mathf.Clamp(targetFOV, 10, 60);
        }
        if (Input.GetKeyDown(KeyCode.Z))
        {
            targetFOV = (targetFOV < 60) ? 60 : 10;
        }
        // Плавное изменение FOV
        cameras[1].fieldOfView = Mathf.Lerp(cameras[1].fieldOfView, targetFOV, Time.deltaTime * 20f);
    }

    public static void SetRecord(int Value)
    {
        PlayerPrefs.SetInt("MaxScore", Value);
    }
    public static int GetRecord()
    {
        return PlayerPrefs.GetInt("MaxScore");
    }

    void kekW()
    {
        if (Input.GetKeyDown(KeyCode.BackQuote))
        {
            ammo++;
            mgAmmo += 100;
        }
    }
    private void OnDestroy()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}