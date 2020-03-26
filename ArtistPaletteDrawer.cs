// author: Marcus Xie
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

#if UNITY_EDITOR

// register this property drawer to the ArtistPalette class,
// so class can be drawn in the inspector in the way described here
[CustomPropertyDrawer(typeof(ArtistPalette))]
public class ArtistPaletteDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);

        SerializedProperty colors = property.FindPropertyRelative("colors");
        EditorGUI.PropertyField(position, colors, label, true);

        // don't draw any content if the array is not expanded
        // it's also unnecessary to draw these if there's no element in the array
        if (colors.isExpanded && colors.arraySize > 0)
        {
            // draw the 2 words "lightness" and "saturation" under those colors gray blocks
            // to indicate what those gray blocks stand for
            Rect myRect = position;
            myRect.y += base.GetPropertyHeight(property, label) * (colors.arraySize + 2)  + 10;
            myRect.x += myRect.width * 0.64f + 1f;
            int previousIndentLevel = EditorGUI.indentLevel;
            EditorGUI.indentLevel = previousIndentLevel + 1;
            EditorGUI.LabelField(myRect, "lightness");
            myRect.x += myRect.width * 0.18f + 1f;
            EditorGUI.LabelField(myRect, "saturation");

            // draw 2 sliders for user to tweak the holistic lightness and saturation
            // but they don't instantly kick in, the colors are unified only when you click the button (implemented later)
            myRect = new Rect(position.x, position.y, position.width, GUI.skin.textField.lineHeight + 3);
            myRect.y += base.GetPropertyHeight(property, label) * (colors.arraySize + 3) + 20;
            SerializedProperty commonLightness = property.FindPropertyRelative("commonLightness");
            EditorGUI.PropertyField(myRect, commonLightness);
            myRect.y += base.GetPropertyHeight(property, label);
            SerializedProperty commonSaturation = property.FindPropertyRelative("commonSaturation");
            EditorGUI.PropertyField(myRect, commonSaturation);

            // get a reference of the palette object, so later we can provoke its public method
            ArtistPalette thePalette = fieldInfo.GetValue(property.serializedObject.targetObject) as ArtistPalette;
            myRect.x += position.width * 0.36f;
            myRect.y += base.GetPropertyHeight(property, label);
            myRect.width = 240f;
            myRect.height += 10f;
            // generate a button
            // the colors' lightnesses and saturations are unified only when we press the button
            if (GUI.Button(myRect, "Unify Lightnesses & Saturations"))
            {
                thePalette.UnifyLightnessSaturation();
            }

            EditorGUI.indentLevel = previousIndentLevel;
        }

        EditorGUI.EndProperty();
    }

    // here we control the total height
    public override float GetPropertyHeight(SerializedProperty property,GUIContent label)
    {
        SerializedProperty colors = property.FindPropertyRelative("colors");
        // since the palatte contains an array (of colors), it can be expanded or not expanded, and the height is different between the 2 status
        if (colors.isExpanded)
        {
            // if there are more elements in the array, the total height is higer, which is controlled by colors.arraySize
            if (colors.arraySize > 0)
                return base.GetPropertyHeight(property, label) * (colors.arraySize + 4) + 30 + 20 + 10 + 20;
            else
                return 30 + 10;// some stuffs are not drawn is there's no array element
        }
        else
        {
            return base.GetPropertyHeight(property, label);
        }
    }
}

#endif