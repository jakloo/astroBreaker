using UnityEngine;
using System.Collections.Generic;

public class ColorGetter : MonoBehaviour {


    // colors for TMP cannot be fetched from a GUI-edited array because of a Unity bug 
    static List<Color> guiColors = new(){
        createColor(0,115,255),
        createColor(0,255,255),
        createColor(0,255,0),
        createColor(255,255,0),
        createColor(255,0,0),
        createColor(255,0,255),
        createColor(125,150,255),
        createColor(75,175,200),
        createColor(125,255,125),
        createColor(200,175,50),
        createColor(255,100,100),
        createColor(175,100,175)
        };

    private static ColorPalette palette = Resources.Load<ColorPalette>("ColorData");

    public static int offset;

    public static Color getJunkColor(int index){
        return palette.junkColors[index + offset * 6];
    }

    public static Color getGuiColors(int index){
        return guiColors[index + offset * 6];
    }

    public static Color getGuiColors(int index, int explicitOffset){
        return guiColors[index + explicitOffset * 6];
    }

    public static Gradient getProjectileColors(int index){
        return palette.projectileColors[index + offset * 6];
    }

    public static Gradient getBurningColors(int index){
        return palette.burningColors[index];
    }

    static Color createColor(float red, float green, float blue){
        return new Color(red/255f, green/255f, blue/255f);
    }

       public static Color getDefaultButtonColor(){
        return palette.buttonColors[0];
    }


    public static Color getHighlightedButtonColor(){
        return palette.buttonColors[1];
    }
}