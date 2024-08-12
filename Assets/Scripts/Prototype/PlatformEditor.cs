using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(PlatformExplosion))]
public class PlatformExplosionEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        PlatformExplosion platformExplosion = (PlatformExplosion)target;

        if (GUILayout.Button("Capture Target Transforms"))
        {
            platformExplosion.CaptureTargetTransforms();
            platformExplosion.ResetToInitialPositions();
        }
    }
}
