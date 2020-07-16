using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(LightColorController))]
public class LightColorControllerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        EditorGUI.BeginChangeCheck();

        base.OnInspectorGUI();

        if (EditorGUI.EndChangeCheck())
        {
            var lightColorController = target as LightColorController;
            lightColorController.GetSetters();
        }
    }
}
