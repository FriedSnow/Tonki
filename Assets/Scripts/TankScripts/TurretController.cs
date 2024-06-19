using UnityEngine;

public class TurretControl : MonoBehaviour
{
    public Transform tankTransform;
    public float rotationSpeed = 5f;

    void Update()
    {
        Vector3 targetPosition = GetMouseWorldPosition();
        Vector3 direction = targetPosition - transform.position;
        direction = tankTransform.InverseTransformDirection(direction); // Переводим направление в локальные координаты танка
        direction.y = 0; // Игнорируем высоту для вращения

        Quaternion targetRotation = Quaternion.LookRotation(tankTransform.TransformDirection(direction), tankTransform.up);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }

    Vector3 GetMouseWorldPosition()
    {
        Plane plane = new Plane(Vector3.up, 0);
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        float distance;
        if (plane.Raycast(ray, out distance))
        {
            return ray.GetPoint(distance);
        }
        return Vector3.zero;
    }
}