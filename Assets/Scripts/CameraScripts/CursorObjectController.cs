using System.Collections;
using UnityEngine;

public class CursorObjectController : MonoBehaviour
{
    public GameObject cursorObjectPrefab; // Префаб объекта, который будет падать
    private GameObject cursorObjectInstance;
    public LayerMask groundLayer; // Слой для объектов земли
    public static CursorObjectController Instance { get; private set; } // Статическая переменная для текущего экземпляра

    void Awake()
    {
        Instance = this; // Устанавливаем текущий экземпляр
    }

    void Start()
    {
        // Создаем экземпляр объекта курсора
        cursorObjectInstance = Instantiate(cursorObjectPrefab);
    }

    void Update()
    {
        // Получаем позицию курсора в мировых координатах
        Vector3 cursorPosition = GetCursorWorldPosition();

        // Обновляем позицию объекта курсора
        if (cursorObjectInstance != null)
        {
            cursorObjectInstance.transform.position = cursorPosition;
        }

        // Начинаем падение объекта
        StartCoroutine(FallToGround(cursorObjectInstance.transform));
    }

    Vector3 GetCursorWorldPosition()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, groundLayer))
        {
            return hit.point;
        }
        return Vector3.zero;
    }
    
    IEnumerator FallToGround(Transform objTransform)
    {
        while (true)
        {
            Ray ray = new Ray(objTransform.position, Vector3.down);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, groundLayer))
            {
                if (hit.distance > 0.1f)
                {
                    // Падаем вниз
                    objTransform.position -= new Vector3(0, 0.1f, 0);
                }
                else
                {
                    // Остановить падение
                    yield break;
                }
            }
            yield return null;
        }
    }

    public float GetHeight()
    {
        if (cursorObjectInstance != null)
        {
            return cursorObjectInstance.transform.position.y;
        }
        return 0f;
    }

    public Vector3 GetCursorPosition()
    {
        if (cursorObjectInstance != null)
        {
            return cursorObjectInstance.transform.position;
        }
        return Vector3.zero;
    }
}
