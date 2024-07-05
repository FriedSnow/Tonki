using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    // Метод для загрузки основной сцены
    public void StartTankGame()
    {
        SceneManager.LoadScene("TankScene");
    }
    public void StartAAGame()
    {
        SceneManager.LoadScene("SPAAScene"); 
    }
    public void StartMSLGame()
    {
        SceneManager.LoadScene("MSLScene"); 
    }
    public void StartBMPGame()
    {
        SceneManager.LoadScene("BMPScene"); 
    }

    // Метод для выхода из игры
    public void ExitGame()
    {
        Application.Quit();
    }
}