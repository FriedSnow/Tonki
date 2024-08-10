using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EnemyManager : MonoBehaviour
{
    public static EnemyManager instance;
    public Text restartingText; // Ссылка на текстовый объект UI
    public Text scoreText;                      //ссылка на UI текста 
    public static int score = 0;                      //ссылка на UI текста 
    public float restartDelay = 3f; // Задержка перед перезапуском сцены

    public static int enemyCount = 0;
    private void Start() {
        score = 0;   
    }
    private void Update()
    {
        UpdateScore();
    }
    void UpdateScore(){

        if (scoreText != null)
        {
            scoreText.text = score.ToString();
        }
    }
    void Awake()
    {
        UpdateScore();
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
        score += 100;
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
            restartingText.text = Values.restartWin;
        }
    }

    private void RestartScene()
    {   
        score = 0;
        Debug.Log("Перезапуск сцены...");
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
