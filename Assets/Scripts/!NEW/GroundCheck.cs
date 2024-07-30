using UnityEngine;
using System.Collections;

public class GroundCheck : MonoBehaviour
{
    // Массив триггер-объектов
    public Collider[] triggerColliders;
    public static bool isOnGround;
    // Маска слоёв, с которыми нужно проверять соприкосновение
    public LayerMask layerMask;
    // Время задержки перед изменением isOnGround на false
    private float delayTime = 0.3f;

    private Coroutine groundCheckCoroutine;

    private void Update() {
        bool currentlyOnGround = IsAnyTriggerColliding();

        if (currentlyOnGround)
        {
            if (groundCheckCoroutine != null)
            {
                StopCoroutine(groundCheckCoroutine);
                groundCheckCoroutine = null;
            }
            isOnGround = true;
        }
        else if (groundCheckCoroutine == null)
        {
            groundCheckCoroutine = StartCoroutine(ResetGroundStatusAfterDelay());
        }
    }

    // Метод, который проверяет, соприкасается ли хоть один триггер с чем-либо из указанных слоёв
    public bool IsAnyTriggerColliding()
    {
        foreach (Collider col in triggerColliders)
        {
            if (col.isTrigger && IsCollidingWithLayers(col))
            {
                return true;
            }
        }
        return false;
    }

    // Вспомогательный метод, который проверяет, соприкасается ли данный триггер с чем-либо из указанных слоёв
    private bool IsCollidingWithLayers(Collider col)
    {
        Collider[] hitColliders = Physics.OverlapBox(col.bounds.center, col.bounds.extents, col.transform.rotation, layerMask);

        foreach (Collider hitCol in hitColliders)
        {
            if (hitCol != col)
            {
                return true;
            }
        }

        return false;
    }

    // Корутин для задержки перед изменением isOnGround на false
    private IEnumerator ResetGroundStatusAfterDelay()
    {
        yield return new WaitForSeconds(delayTime);
        isOnGround = false;
        groundCheckCoroutine = null;
    }

    // Статический метод для проверки isOnGround
    public static bool Check(){
        return isOnGround;
    }
}