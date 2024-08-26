using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class ToggleBlackAndWhite : MonoBehaviour
{
    // Ссылка на Post-Processing Volume
    public PostProcessVolume postProcessVolume;
    // Флаг для отслеживания состояния фильтра
    private bool isBlackAndWhite = false;
    private void Start()
    {
        ToggleFilter();
        ToggleFilter();
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T) && PlayerTankController._currentCameraIndex == 1)
        {
            ToggleFilter();
        }
    }

    // Метод для переключения фильтра по нажатию кнопки
    public void ToggleFilter()
    {
        isBlackAndWhite = !isBlackAndWhite;

        if (isBlackAndWhite)
        {
            // Включаем черно-белый фильтр
            postProcessVolume.weight = 1.0f;
        }
        else
        {
            // Отключаем фильтр
            postProcessVolume.weight = 0.0f;
        }
    }
}
