using System.IO;
using UnityEditor;
using UnityEngine;

namespace Com.AFBiyik.AudioSystem.Editor {
    /// <summary>
    /// Editor for <see cref="AudioConfig"/> ScriptableObject.
    /// </summary>
    [CustomEditor(typeof(AudioConfig))]
    public class AudioConfigEditor : UnityEditor.Editor {
        public override void OnInspectorGUI() {
            serializedObject.Update();

            AudioConfig audioConfig = (AudioConfig)target;

            var musicSource = serializedObject.FindProperty("musicSource");

            EditorGUILayout.LabelField("Music");

            EditorGUILayout.PropertyField(musicSource, new GUIContent("Music Source (IFxSource)"));

            if (GUILayout.Button("Generate Music Source")) {
                var source = CreateFxSource("MusicSource", false);
                musicSource.objectReferenceValue = source;
            }


            var musicConfig = serializedObject.FindProperty("musicConfig");
            EditorGUILayout.PropertyField(musicConfig);

            if (GUILayout.Button("Generate Music Config")) {
                var musicConfigObject = ScriptableObject.CreateInstance<MusicConfig>();
                AssetDatabase.CreateAsset(musicConfigObject, "Assets/Audio/MusicConfig.asset");
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
                musicConfig.objectReferenceValue = musicConfigObject;
            }

            EditorGUILayout.Space(40);

            var soundSource2d = serializedObject.FindProperty("soundSource2d");

            EditorGUILayout.LabelField("Sound 2d");

            EditorGUILayout.PropertyField(soundSource2d, new GUIContent("Sound Source 2d (IFxSource)"));

            if (GUILayout.Button("Generate Sound Source 2d")) {
                var source = CreateFxSource("SoundSource2d", false);
                soundSource2d.objectReferenceValue = source;
            }

            EditorGUILayout.Space(40);

            var soundSource3d = serializedObject.FindProperty("soundSource3d");

            EditorGUILayout.LabelField("Sound 3d");

            EditorGUILayout.PropertyField(soundSource3d, new GUIContent("Sound Source 3d (IFxSource)"));

            if (GUILayout.Button("Generate Sound Source 3d")) {
                var source = CreateFxSource("SoundSource3d", true);
                soundSource3d.objectReferenceValue = source;
            }

            serializedObject.ApplyModifiedProperties();
        }

        public virtual FxSource CreateFxSource(string name, bool is3d) {
            if (!AssetDatabase.IsValidFolder("Assets/Audio")) {
                AssetDatabase.CreateFolder("Assets", "Audio");
            }

            string path = "Assets/Audio/" + name + ".prefab";

            if (!File.Exists(path)) {

                GameObject go = new GameObject("FxSource");
                var audioSource = go.AddComponent<AudioSource>();
                audioSource.spatialBlend = is3d ? 1 : 0;
                var fxSource = go.AddComponent<FxSource>();

                PrefabUtility.SaveAsPrefabAsset(go, path);

                DestroyImmediate(go);
            }

            var source = AssetDatabase.LoadAssetAtPath<FxSource>(path);
            return source;
        }
    }
}

