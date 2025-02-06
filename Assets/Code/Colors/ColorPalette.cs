using UnityEngine;

[CreateAssetMenu(fileName = "ColorData", menuName = "Data/Color Palette")]
public class ColorPalette : ScriptableObject
{
    public Color[] junkColors;

    public Color upgradeColor;
    public Color[] guiColors;
    public Gradient[] projectileColors;
    public Gradient upgradeProjectileColor;
    public Gradient[] burningColors;
    public Gradient upgradeBurningColor;

    public Color[] buttonColors;
}