using UnityEngine;
using UnityEngine.UI;

public class LightToggle : MonoBehaviour
{
    // Массив всех источников света на сцене
    private Light[] allLights;

    // Указание на выбранный источник света
    public Light[] selectedLight;

    // Флаг, показывающий текущее состояние
    private bool isOnlySelectedLightOn = false;

    void Start()
    {
        // Найдем все источники света на сцене
        allLights = FindObjectsOfType<Light>();
        selectedLight[0].enabled = false;
        selectedLight[1].enabled = false;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T) && PlayerTankController._currentCameraIndex == 1)
        {
            ToggleLights();
        }
        if (PlayerTankController._currentCameraIndex == 0)
        {
            foreach (Light light in allLights)
            {
                if (light != selectedLight[0])
                {
                    light.enabled = true;
                }
            }
            selectedLight[0].enabled = false;
            selectedLight[1].enabled = false;
            RenderSettings.ambientIntensity = 1;
        }
    }
    // Метод для переключения света при нажатии кнопки
    public void ToggleLights()
    {
        if (isOnlySelectedLightOn)
        {
            // Если включен только один источник света, включаем все остальные, кроме выбранного
            foreach (Light light in allLights)
            {
                if (light != selectedLight[0])
                {
                    light.enabled = true;
                }
            }
            selectedLight[0].enabled = false;
            selectedLight[1].enabled = false;
            RenderSettings.ambientIntensity = 1;
        }
        else
        {
            // Если все источники включены, отключаем все кроме одного
            foreach (Light light in allLights)
            {
                light.enabled = false;
            }
            selectedLight[0].enabled = true;
            selectedLight[1].enabled = true;
        }

        // Инвертируем состояние
        isOnlySelectedLightOn = !isOnlySelectedLightOn;
        RenderSettings.ambientIntensity = 0;
    }
}
