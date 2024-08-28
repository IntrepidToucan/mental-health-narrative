using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;

[CustomEditor(typeof(RedPlatformMovement))]
public class PlatformEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        RedPlatformMovement redPlatformMovement = (RedPlatformMovement)target;

        // Removed the buttons for capturing target transforms and resetting position
        // as those methods are no longer part of the RedPlatformMovement script.

        // Example of adding a button for a different function, if needed
        if (GUILayout.Button("Release Platform"))
        {
            redPlatformMovement.ReleasePlatform();
        }
    }
}
#endif
