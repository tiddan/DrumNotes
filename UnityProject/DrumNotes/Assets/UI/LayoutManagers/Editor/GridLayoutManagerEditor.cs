using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI.LayoutManagers.Editor
{
    [CustomEditor(typeof(GridLayoutManager))]
    public class GridLayoutManagerEditor : UnityEditor.Editor
    {
        private SerializedProperty tileSize;
        private SerializedProperty columnCount;
        private SerializedProperty marginTop;
        private SerializedProperty marginBottom;
        private SerializedProperty marginLeft;
        private SerializedProperty marginRight;
        private SerializedProperty spacing;
        
        private SerializedProperty tilePrefab;
        private SerializedProperty spawnCount;

        void OnEnable()
        {
            tileSize = serializedObject.FindProperty("tileSize");
            columnCount = serializedObject.FindProperty("columnCount");
            marginTop = serializedObject.FindProperty("marginTop");
            marginBottom = serializedObject.FindProperty("marginBottom");
            marginLeft = serializedObject.FindProperty("marginLeft");
            marginRight = serializedObject.FindProperty("marginRight");
            spacing = serializedObject.FindProperty("spacing");
            
            tilePrefab = serializedObject.FindProperty("tilePrefab");
            spawnCount = serializedObject.FindProperty("spawnCount");
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
            EditorGUILayout.PropertyField(tileSize);
            EditorGUILayout.PropertyField(columnCount);
            EditorGUILayout.PropertyField(marginTop);
            EditorGUILayout.PropertyField(marginBottom);
            EditorGUILayout.PropertyField(marginLeft);
            EditorGUILayout.PropertyField(marginRight);
            EditorGUILayout.PropertyField(spacing);
            EditorGUI.indentLevel--;
            EditorGUILayout.Space();
            
            EditorGUILayout.LabelField(new GUIContent("Debug"),foldoutHeader);
            EditorGUI.indentLevel++;
            EditorGUILayout.PropertyField(tilePrefab);
            EditorGUILayout.PropertyField(spawnCount);
            
            serializedObject.ApplyModifiedProperties();
            
            if (GUILayout.Button("Refresh"))
            {
                var gridLayoutMgr = (target as GridLayoutManager);
                if (gridLayoutMgr == null)
                    return;
                
                var sw = System.Diagnostics.Stopwatch.StartNew();
                gridLayoutMgr.Refresh();
                sw.Stop();
                Debug.Log("Rebuild took : " +sw.ElapsedMilliseconds + " ms");
                
                EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());
            }
            
            if (GUILayout.Button("Build tiles"))
            {
                var gridLayoutMgr = (target as GridLayoutManager);
                if (gridLayoutMgr == null)
                    return;

                // var listOfChildren = new List<Transform>();
                // for (var i = 0; i < gridLayoutMgr.transform.childCount; i++)
                // {
                //     listOfChildren.Add(gridLayoutMgr.transform.GetChild(i));
                // }
                // listOfChildren.ForEach(x=>DestroyImmediate(x.gameObject));
                //
                // for (var i = 0; i < spawnCount.intValue; i++)
                // {
                //     Instantiate(tilePrefab.objectReferenceValue, gridLayoutMgr.transform);
                // }
                // gridLayoutMgr.Refresh();
                gridLayoutMgr.BuildTiles();

            }
            EditorGUI.indentLevel--;
        }
    }
}