using UnityEngine;

public class CameraSwitcher : MonoBehaviour
{
    public Camera normalCamera;
    public Camera thermalCamera;

    void Start()
    {
        // Убедитесь, что обычная камера активна в начале
        SetNormalView();
    }

    void Update()
    {
        // Переключение камер по нажатию кнопки (например, "T")
        if (Input.GetKeyDown(KeyCode.T))
        {
            if (normalCamera.gameObject.activeSelf)
            {
                SetThermalView();
            }
            else
            {
                SetNormalView();
            }
        }
    }

    void SetNormalView()
    {
        normalCamera.gameObject.SetActive(true);
        thermalCamera.gameObject.SetActive(false);
    }

    void SetThermalView()
    {
        normalCamera.gameObject.SetActive(false);
        thermalCamera.gameObject.SetActive(true);
    }
}