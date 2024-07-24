using UnityEngine;
using UnityEngine.UI;

public class AmmoTypeUI : MonoBehaviour
{
    public Image armorPiercingIcon;
    public Image antiTankIcon;
    public Image highExplosiveIcon;
    public Color selectedColor = Color.white;
    public Color deselectedColor = Color.gray;

    private enum AmmoType { ArmorPiercing, AntiTank, HighExplosive }
    private AmmoType currentAmmoType;

    void Start()
    {
        SetAmmoType(AmmoType.ArmorPiercing);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SetAmmoType(AmmoType.ArmorPiercing);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            SetAmmoType(AmmoType.AntiTank);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SetAmmoType(AmmoType.HighExplosive);
        }
    }

    void SetAmmoType(AmmoType type)
    {
        currentAmmoType = type;
        switch (type)
        {
            case AmmoType.ArmorPiercing:
                armorPiercingIcon.color = selectedColor;
                antiTankIcon.color = deselectedColor;
                highExplosiveIcon.color = deselectedColor;
                break;
            case AmmoType.HighExplosive:
                armorPiercingIcon.color = deselectedColor;
                antiTankIcon.color = selectedColor;
                highExplosiveIcon.color = deselectedColor;
                break;
            case AmmoType.AntiTank:
                armorPiercingIcon.color = deselectedColor;
                antiTankIcon.color = deselectedColor;
                highExplosiveIcon.color = selectedColor;
                break;
        }
    }
}