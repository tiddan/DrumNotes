using UI.UIComponents;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(PodPalButton))]
public class PodPalButtonEditor : UnityEditor.Editor
{
    // private SerializedProperty sfxPress;
    // private SerializedProperty sfxHover;
    //
    // private void OnEnable()
    // {
    //     sfxPress = serializedObject.FindProperty("customButtonPressSfx");
    //     sfxHover = serializedObject.FindProperty("customButtonHoverSfx");
    // }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        // GUILayout.Label("Customization");
        // EditorGUILayout.PropertyField(sfxPress);
        // EditorGUILayout.PropertyField(sfxHover);
    }
}
