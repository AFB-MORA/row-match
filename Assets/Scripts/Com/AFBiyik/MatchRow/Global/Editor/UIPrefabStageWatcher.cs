#if UNITY_EDITOR
using System.Linq;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

/// <summary>
/// Watches for state changes of prefab stage.
/// Adds parent to ui prefabs.
/// </summary>
[InitializeOnLoad]
static class UIPrefabStageWatcher
{
    static UIPrefabStageWatcher()
    {
        PrefabStage.prefabStageOpened += OnStageOpened;
    }

    private static void OnStageOpened(PrefabStage stage)
    {
        var prefab = stage.prefabContentsRoot;

        if (prefab.GetComponent<RectTransform>() != null)
        {
            var scale = prefab.transform.localScale;
            var rootObjects = stage.scene.GetRootGameObjects();
            var parent = stage.scene.GetRootGameObjects().FirstOrDefault(go => go.name == "UIParent (Environment)");
            prefab.transform.SetParent(parent.transform, false);
            prefab.transform.localScale = scale;
        }
    }
}
#endif