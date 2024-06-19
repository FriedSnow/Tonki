using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 10f, rotateSpeed = .000000000001f;

    private void Update()
    {
        // Получение ввода с клавиатуры
        float vertical = Input.GetAxis("Vertical");
        float rotate = 0f;

        if(Input.GetKey(KeyCode.A))
            rotate = -1f;
        else if(Input.GetKey(KeyCode.D))
            rotate = 1f;

        // Вычисление направления движения
        Vector3 moveDirection = new Vector3(0f, 0f, vertical);
        moveDirection = transform.TransformDirection(moveDirection);

        transform.Rotate(Vector3.up * rotateSpeed * Time.deltaTime * rotate, Space.World);

        // Применение движения к объекту
        transform.position += moveDirection * moveSpeed * Time.deltaTime;
    }
}
