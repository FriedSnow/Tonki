using System;
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
    public float recoilForce = 100f;            //отдача после выстрела главного орудия
    public float rotateSpeed = 100f;            //скорость поворота корпуса
    public float turretRotateSpeed = 5f;        //скорость поворота башни
    public GameObject burningParticlesPrefab;   //частицы горения танка после уничтожения
    public GameObject explosionParticlesPrefab; //частицы взрыва танка после уничтожения
    public GameObject mgProjectilePrefab;       //снаряд для зенитного пулемета
    public GameObject[] projectilePrefabs;      //массив типов снарядов танка
    public GameObject shootParticlesPrefab;     //частицы выстрела
    public GameObject shootParticlesMGPrefab;   //частицы выстрела
    public GameObject tank;                     //обьект танка
    public Material grayMaterial;               //материал уничтоженного танка
    public Rigidbody tankRigidbody;             //еще один обьект танка
    public Slider healthBar;                    //ссылка на UI элемент Slider
    public Text restartingText;                 //ссылка на UI текста выводимого при рестарте
    public Text ammoText;                       //ссылка на UI текста 
    public Text mgAmmoText;                     //ссылка на UI текста 
    public Transform firePoint;                 //точка выстрела
    public Transform recoilPoint;               //точка отдачи
    public Transform machineGun;                //объект зенитного пулемета
    public Transform machineGunFirePoint;       //точка выстрела зенитного пулемета
    public Transform tankTransform;             //и еще один обьект танка
    public Transform turret;                    //обьект башни
    public int ammo = 20;
    public int mgAmmo = 2000;
    public int health = 200;
    public int maxHealth = 200;
    private bool canBeDestroyed = true;
    private bool isDestroyed = false;
    private float nextFireTime = 0f;            //таймер основного орудия
    private float nextMGFireTime = 0f;          //таймер зенитного пулемета
    private int currentProjectileIndex = 0;     //индекс выбранного снаряда в массиве типов
    private Gradient gradient;                  //градиент для смены цветов
    private Image fillImage;                    //ссылка на Image компонента Fill
    private float speedMultiplier = 1f;
    void Start()
    {
        health = maxHealth;
        UpdateHealthBarColor();
    }
    private void Update()
    {
        if (!isDestroyed)
        {
            MoveTank();
            RotateTurret();
            RotateMachineGun();
            HandleShooting();
            HandleProjectileSwitching();
            UpdateAmmoText();
        }
        if (GroundCheck.Check())
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
        if (moveInput > 0 && currentSpeed < maxSpeed)
        {
            tankRigidbody.AddForce(transform.forward * acceleration * moveInput * speedMultiplier, ForceMode.Force);
        }
        else if (moveInput < 0 && currentSpeed < maxSpeed)
        {
            tankRigidbody.AddForce(transform.forward * acceleration * moveInput * speedMultiplier, ForceMode.Force);
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
        if (currentSpeed >= 1f)
            Debug.Log("Угол скольжения: " + Math.Round(angle, 2) + " Скорость: " + Math.Round(currentSpeed, 2));
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
            GameObject projectile = Instantiate(projectilePrefabs[currentProjectileIndex], firePoint.position, firePoint.rotation);
            Rigidbody rb = projectile.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.velocity = firePoint.forward * projectileSpeed;
            }
            Destroy(projectile, 5f);
            if (shootParticlesPrefab != null)
            {
                GameObject shootParticles = Instantiate(shootParticlesPrefab, firePoint.position, firePoint.rotation);
                Destroy(shootParticles, 3f);
            }
            if (tankRigidbody != null)
            {
                tankRigidbody.AddForce(-recoilPoint.forward * recoilForce * 10f, ForceMode.Impulse);
            }

            transform.RotateAround(recoilPoint.position, -recoilPoint.right, 5f);
        }
    }

    void FireMGProjectile()
    {
        GameObject mgProjectile = Instantiate(mgProjectilePrefab, machineGunFirePoint.position, machineGunFirePoint.rotation);
        Rigidbody rb = mgProjectile.GetComponent<Rigidbody>();
        if (shootParticlesMGPrefab != null)
        {
            GameObject shootParticlesMG = Instantiate(shootParticlesMGPrefab, machineGunFirePoint.position, machineGunFirePoint.rotation);
            Destroy(shootParticlesMG, 3f);
        }
        if (rb != null)
        {
            rb.velocity = machineGunFirePoint.forward * mgProjectileSpeed;
        }
        Destroy(mgProjectile, 3f);
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        if (healthBar != null)
        {
            healthBar.value = health;
            UpdateHealthBarColor();
        }
        if (health <= 0)
        {
            Die();
        }
    }
    void UpdateHealthBarColor()
    {
        if (healthBar != null)
        {
            healthBar.maxValue = maxHealth;
            healthBar.value = health;

            // Получаем ссылку на Image компонента Fill
            fillImage = healthBar.fillRect.GetComponent<Image>();

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
        if (fillImage != null && gradient != null)
        {
            float normalizedHealth = (float)health / maxHealth;
            fillImage.color = gradient.Evaluate(normalizedHealth);
        }
    }
    void UpdateAmmoText()
    {
        ammoText.text = ammo.ToString();
        mgAmmoText.text = mgAmmo.ToString();
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
            EnemyManager.score = 0;
            canBeDestroyed = false;
        }
    }

    void ShowRestartingMessage()
    {
        if (restartingText != null)
        {
            restartingText.text = "Restart...";
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
                ammo++;
                mgAmmo += 100;
                EnemyManager.score += 25;
                return; // Выходим из метода, так как мы уже обработали столкновение
            }
            else if (other.CompareTag("HP") && health < maxHealth)
            {
                health += 50;
                EnemyManager.score += 25;
                UpdateHealthBarColor();
                return; // Выходим из метода, так как мы уже обработали столкновение
            }

        }
    }
}