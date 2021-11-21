using UnityEditor;
using UnityEngine;

namespace UI.LayoutManagers.Editor
{
    [CustomEditor(typeof(AbilityLayoutManager))]
    public class AbilityLayoutManagerEditor : UnityEditor.Editor
    {
        private SerializedProperty defaultSize;

        void OnEnable()
        {
            defaultSize = serializedObject.FindProperty("defaultSize");
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
            EditorGUILayout.PropertyField(defaultSize);
            EditorGUI.indentLevel--;
            EditorGUILayout.Space();
            
            serializedObject.ApplyModifiedProperties();
            
            EditorGUILayout.LabelField(new GUIContent("Debug"),foldoutHeader);
            if (GUILayout.Button("Test"))
            {
                var sw = System.Diagnostics.Stopwatch.StartNew();
                (target as AbilityLayoutManager).Refresh();
                sw.Stop();
                Debug.Log("Rebuild took : " +sw.ElapsedMilliseconds + " ms");
            }
        }
    }
}