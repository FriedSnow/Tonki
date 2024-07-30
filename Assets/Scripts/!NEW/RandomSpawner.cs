using UnityEngine;

public class RandomSpawner : MonoBehaviour
{
    // Массив Transform, где будут спавниться объекты
    public Transform[] spawnPoints;

    // Префаб объекта, который будет спавниться
    public GameObject prefabToSpawn;

    // Время между спавнами
    public float spawnInterval = 5.0f;

    // Таймер для отслеживания времени
    private float timer;

    void Start()
    {
        // Инициализация таймера
        timer = spawnInterval;
    }

    void Update()
    {
        // Обновление таймера
        timer -= Time.deltaTime;

        // Проверка, прошло ли время для следующего спавна
        if (timer <= 0)
        {
            if (EnemyManager.enemyCount <= 10)
            {
                // Спавн объекта
                SpawnObject();
                // Сброс таймера
                timer = spawnInterval;
            }
        }
    }

    void SpawnObject()
    {
        // Проверка, есть ли точки спавна в массиве
        if (spawnPoints.Length == 0)
        {
            Debug.LogWarning("Массив spawnPoints пуст!");
            return;
        }

        // Выбор случайной точки из массива
        int randomIndex = Random.Range(0, spawnPoints.Length);

        // Спавн объекта в выбранной точке
        Instantiate(prefabToSpawn, spawnPoints[randomIndex].position, spawnPoints[randomIndex].rotation);
    }
}