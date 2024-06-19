using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    // Метод для загрузки основной сцены
    public void StartTankGame()
    {
        SceneManager.LoadScene("MainScene");
    }
    public void StartAAGame()
    {
        SceneManager.LoadScene("MainScene1"); 
    }
    public void StartMSLGame()
    {
        SceneManager.LoadScene("MainScene1 1"); 
    }

    // Метод для выхода из игры
    public void ExitGame()
    {
        Application.Quit();
    }
}