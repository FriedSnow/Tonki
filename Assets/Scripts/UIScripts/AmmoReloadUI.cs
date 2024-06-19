using UnityEngine;
using UnityEngine.UI;

public class AmmoReloadUI : MonoBehaviour
{
    public Image armorPiercingIcon;
    public Image armorPiercingReloadOverlay;
    public Image highExplosiveIcon;
    public Image highExplosiveReloadOverlay;
    public float reloadTime = 2f; // время перезарядки в секундах

    private bool isReloading = false;
    private float reloadProgress = 0f;

    void Start()
    {
        // Изначально слои перезарядки должны быть полностью заполнены
        armorPiercingReloadOverlay.fillAmount = 0f;
        highExplosiveReloadOverlay.fillAmount = 0f;
    }

    void Update()
    {
        if (isReloading)
        {
            reloadProgress += Time.deltaTime / reloadTime;
            UpdateReloadOverlay();

            if (reloadProgress >= 1f)
            {
                isReloading = false;
                reloadProgress = 0f;
            }
        }
    }

    public void StartReload()
    {
        isReloading = true;
        reloadProgress = 0f;
    }

    private void UpdateReloadOverlay()
    {
        armorPiercingReloadOverlay.fillAmount = 1f - reloadProgress;
        highExplosiveReloadOverlay.fillAmount = 1f - reloadProgress;
    }
}