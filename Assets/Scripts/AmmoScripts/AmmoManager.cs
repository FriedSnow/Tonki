using UnityEngine;
using UnityEngine.UI;

public class AmmoManager : MonoBehaviour
{
    public enum AmmoType { ArmorPiercing, HighExplosive}
    public AmmoType currentAmmoType = AmmoType.ArmorPiercing;

    public Text ammoTypeText;

    void Start()
    {
        UpdateAmmoTypeText();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            currentAmmoType = AmmoType.ArmorPiercing;
            UpdateAmmoTypeText();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            currentAmmoType = AmmoType.HighExplosive;
            UpdateAmmoTypeText();
        }
    }

    void UpdateAmmoTypeText()
    {
        switch (currentAmmoType)
        {
            case AmmoType.ArmorPiercing:
                ammoTypeText.text = "Ammo Type: AP";
                break;
            case AmmoType.HighExplosive:
                ammoTypeText.text = "Ammo Type: HE";
                break;
        }
    }
}