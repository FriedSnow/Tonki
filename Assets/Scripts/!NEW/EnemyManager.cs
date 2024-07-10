using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EnemyManager : MonoBehaviour
{
    public static EnemyManager instance;
    public Text restartingText; // Ссылка на текстовый объект UI
    public float restartDelay = 3f; // Задержка перед перезапуском сцены

    private int enemyCount = 0;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            //DontDestroyOnLoad(gameObject); // Сохраняем объект при перезагрузке сцены
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void RegisterEnemy()
    {
        enemyCount++;
        Debug.Log("Enemy registered. Current enemy count: " + enemyCount);
    }

    public void UnregisterEnemy()
    {
        enemyCount--;
        Debug.Log("Enemy unregistered. Current enemy count: " + enemyCount);

        if (enemyCount <= 0)
        {
            StartCoroutine(RestartSceneWithDelay());
        }
    }

    private IEnumerator RestartSceneWithDelay()
    {
        ShowRestartingMessage();
        yield return new WaitForSeconds(restartDelay);
        RestartScene();
    }

    private void ShowRestartingMessage()
    {
        if (restartingText != null)
        {
            restartingText.text = "Restart...";
        }
    }

    private void RestartScene()
    {
        Debug.Log("Перезапуск сцены...");
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
