// author: Marcus Xie
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// to use property drawer for a class, the class needs to be serializable
[System.Serializable]
public class ArtistPalette
{
    [ArtistColor]
    public Color[] colors;

#if UNITY_EDITOR
    // the lightness to be assigned to all colors in the array
    // avoid dividing by zero
    [Range(0.001f, 1.0f)]
    public float commonLightness;

    // the saturation to be assigned to all colors in the array
    // avoid dividing by zero
    [Range(0.001f, 1.0f)]
    public float commonSaturation;

    // make the lightness and saturation of the colors all the same
    // this method is provoked by a button in the property drawer
    public void UnifyLightnessSaturation()
    {
        for (int i = 0; i < colors.Length; i++)
        {
            float h, s, v;
            Color.RGBToHSV(colors[i], out h, out s, out v);
            s = commonSaturation;
            v = commonLightness;
            colors[i] = Color.HSVToRGB(h, s, v);
        }
    }
#endif

}
