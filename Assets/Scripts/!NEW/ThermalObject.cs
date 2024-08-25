using UnityEngine;

public class ThermalObject : MonoBehaviour
{
    public float temperature = 20.0f; // Температура по умолчанию

    void OnMouseDown()
    {
        // Изменение температуры по клику
        temperature += 5.0f;
        Debug.Log("Temperature: " + temperature);
    }
}