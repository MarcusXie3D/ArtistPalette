// author: Marcus Xie
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

// [ArtistColor] attribute is generated and can be attached to variables
public class ArtistColorAttribute : PropertyAttribute
{
}

#if UNITY_EDITOR

// this line is to make every variable attached by [ArtistColor] to be drawn in the inspector in the way described in the following class
[CustomPropertyDrawer(typeof(ArtistColorAttribute))]
public class ArtistColorDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        // check if the variable attached by [ArtistColor] is Color or not
        if (property.propertyType == SerializedPropertyType.Color)
        {
            // overwrite the default color value to white
            if (property.colorValue == new Color(0f, 0f, 0f, 0f))
                property.colorValue = Color.white;
            Rect defaultRect = position;
            defaultRect.width = position.width * 0.64f;
            // draw the color its self, but without the alpha bar
            property.colorValue = EditorGUI.ColorField(defaultRect, label, property.colorValue, true, false, false);
            Rect customRect = position;
            customRect.x = defaultRect.width + 16;
            customRect.width = position.width * 0.18f - 2.0f;
            float lightness, saturation, hue;
            // get the color's saturation and lightness value for later display
            Color.RGBToHSV(property.colorValue, out hue, out saturation, out lightness);
            // draw the 2 values as gray values, which is more intuitive
            Color lightnessColor = new Color(lightness, lightness, lightness);
            Color saturationColor = new Color(saturation, saturation, saturation);
            EditorGUI.ColorField(customRect, GUIContent.none, lightnessColor, false, false, false);
            customRect.x += customRect.width + 4;
            customRect.width = position.width * 0.18f - 2.0f;
            EditorGUI.ColorField(customRect, GUIContent.none, saturationColor, false, false, false);
        }
        else
            EditorGUI.HelpBox(position, "Error: [ArtistColor] must be put on a Color!", MessageType.Error);
    }
}

#endif