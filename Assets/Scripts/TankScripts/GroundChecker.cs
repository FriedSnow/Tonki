using UnityEngine;

public class GroundChecker : MonoBehaviour
{
    public Transform[] groundCheckPoints; // Массив точек для проверки касания земли
    public float groundCheckDistance = 0.5f; // Расстояние для проверки касания земли
    public LayerMask groundLayer; // Слой земли

    public bool IsGrounded()
    {
        foreach (Transform checkPoint in groundCheckPoints)
        {
            if (Physics.Raycast(checkPoint.position, Vector3.down, groundCheckDistance, groundLayer))
            {
                return true;
            }
        }
        return false;
    }
}