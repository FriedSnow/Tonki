using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class AmmoBox : MonoBehaviour
{
    public float moveSpeed = 1f;
    public float rotationSpeed = 45f;
    public float minY = -1f;
    public float maxY = 1f;
    public GameObject claimPrefab;
    private bool isMovingDown = true;

    void Update()
    {
        // Плавное перемещение по оси Y
        transform.Translate(Vector3.up * (isMovingDown ? -moveSpeed : moveSpeed) * Time.deltaTime);

        // Проверка на достижение минимальной/максимальной позиции по оси Y
        if (isMovingDown && transform.position.y <= minY)
        {
            isMovingDown = false;
        }
        else if (!isMovingDown && transform.position.y >= maxY)
        {
            isMovingDown = true;
        }

        // Плавное вращение вокруг оси Y
        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // PlayerTankController.health += 50;
            if (claimPrefab != null)
            {
                GameObject claim = Instantiate(claimPrefab, transform.position, gameObject.transform.rotation);
                Destroy(claim, 3f);
            }
            Destroy(gameObject);
            return;
        }
    }
}
