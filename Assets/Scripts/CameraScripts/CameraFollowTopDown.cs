using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowTopDown : MonoBehaviour
{
    public float zoomSpeed = 10.0f;
    public Transform targetObject; // Ссылка на объект, за которым должна следить камера
    private Vector3 offset; // Смещение камеры относительно объекта

    void Start()
    {
        // Вычисляем смещение камеры относительно объекта
        offset = transform.position - targetObject.position;
    }

    void LateUpdate()
    {
        // Обновляем положение камеры, чтобы она следовала за объектом
        transform.position = targetObject.position + offset;
    }
}
