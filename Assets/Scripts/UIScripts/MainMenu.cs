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
    public void StartPVPGame()
    {
        SceneManager.LoadScene("PVEScene 1"); 
    }

    // Метод для выхода из игры
    public void ExitGame()
    {
        Application.Quit();
    }
}