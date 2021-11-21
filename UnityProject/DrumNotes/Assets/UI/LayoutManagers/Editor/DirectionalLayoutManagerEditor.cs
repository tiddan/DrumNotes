using UnityEditor;
using UnityEngine;

namespace UI.LayoutManagers.Editor
{
    [CustomEditor(typeof(DirectionalLayoutManager))]
    public class DirectionalLayoutManagerEditor : UnityEditor.Editor
    {
        private SerializedProperty isVertical;
        private SerializedProperty defaultSize;
        private SerializedProperty marginTop;
        private SerializedProperty marginBottom;
        private SerializedProperty marginLeft;
        private SerializedProperty marginRight;
        private SerializedProperty spacing;

        void OnEnable()
        {
            isVertical = serializedObject.FindProperty("isVertical");
            defaultSize = serializedObject.FindProperty("defaultSize");
            marginTop = serializedObject.FindProperty("marginTop");
            marginBottom = serializedObject.FindProperty("marginBottom");
            marginLeft = serializedObject.FindProperty("marginLeft");
            marginRight = serializedObject.FindProperty("marginRight");
            spacing = serializedObject.FindProperty("spacing");
        }
        
        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            
            GUIStyle foldoutHeader = new GUIStyle(EditorStyles.label);
            foldoutHeader.fontStyle = FontStyle.Bold;
            foldoutHeader.margin = new RectOffset(0,5,0,0);
            
            EditorGUILayout.Space();
            EditorGUILayout.LabelField(new GUIContent("Settings"), foldoutHeader);
            EditorGUI.indentLevel++;
            EditorGUILayout.PropertyField(isVertical);
            EditorGUILayout.PropertyField(defaultSize);
            EditorGUILayout.PropertyField(marginTop);
            EditorGUILayout.PropertyField(marginBottom);
            EditorGUILayout.PropertyField(marginLeft);
            EditorGUILayout.PropertyField(marginRight);
            EditorGUILayout.PropertyField(spacing);
            EditorGUI.indentLevel--;
            EditorGUILayout.Space();
            
            serializedObject.ApplyModifiedProperties();
            
            EditorGUILayout.LabelField(new GUIContent("Debug"),foldoutHeader);
            if (GUILayout.Button("Test"))
            {

                var sw = System.Diagnostics.Stopwatch.StartNew();
                (this.target as DirectionalLayoutManager).Refresh();
                sw.Stop();
                Debug.Log("Rebuild took : " +sw.ElapsedMilliseconds + " ms");
            }
        }
    }
}