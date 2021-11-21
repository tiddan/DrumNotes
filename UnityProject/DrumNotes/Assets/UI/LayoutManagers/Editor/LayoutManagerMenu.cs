using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI.LayoutManagers.Editor
{
    public class MenuTest : MonoBehaviour
    {
        // Add a menu item named "Do Something" to MyMenu in the menu bar.
        [MenuItem("PodPal/LayoutManager/UpdateAll %l")]
        static void DoSomething()
        {
            var layoutManagers = FindObjectsOfType<LayoutManager>();
            var sw = System.Diagnostics.Stopwatch.StartNew();
            foreach (var mgr in layoutManagers)
            {
                mgr.Refresh();
            }

            sw.Stop();
            Debug.Log($"Updating all LayoutManagers: {sw.ElapsedMilliseconds} ms");

            if (!EditorApplication.isPlaying)
            {
                EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());
            }
        }
    }
}
