using UnityEngine;
using UnityEditor;
using Scripts.Utility; // Make sure this matches your namespace and folder structure

[InitializeOnLoad]
public static class HierarchyHighlighterEditor
{
    static HierarchyHighlighterEditor()
    {
        EditorApplication.hierarchyWindowItemOnGUI += HighlightHierarchyItem;
    }

    private static void HighlightHierarchyItem(int instanceID, Rect selectionRect)
    {
        GameObject obj = EditorUtility.InstanceIDToObject(instanceID) as GameObject;

        if (obj != null)
        {
            HierarchyHighlighter highlighter = obj.GetComponent<HierarchyHighlighter>();

            if (highlighter != null)
            {
                // Determine whether the object is active or inactive
                bool isActive = obj.activeInHierarchy;

                // Set background color based on active state
                Color backgroundColor = isActive ? highlighter.Background_Color : highlighter.Background_Color_Inactive;

                // Draw the background if color has an alpha greater than 0
                if (backgroundColor.a > 0)
                {
                    EditorGUI.DrawRect(selectionRect, backgroundColor);
                }

                // Set text color and style based on active state
                GUIStyle style = new GUIStyle();
                style.normal.textColor = isActive ? highlighter.Text_Color : highlighter.Text_Color_Inactive;
                style.fontStyle = isActive ? highlighter.TextStyle : highlighter.TextStyle_Inactive;

                // Draw the label with the chosen style
                EditorGUI.LabelField(selectionRect, obj.name, style);
            }
        }
    }
}
