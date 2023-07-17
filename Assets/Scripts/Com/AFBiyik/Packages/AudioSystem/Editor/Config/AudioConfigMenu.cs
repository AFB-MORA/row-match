using UnityEditor;
using UnityEngine;

namespace Com.AFBiyik.AudioSystem.Editor {
    public class AudioConfigMenu {
        [MenuItem("AFBiyik/AudioSystem/AudioConfig", false, 0)]
        public static void Settings() {
            AudioConfig audioConfig = AssetDatabase.LoadAssetAtPath<AudioConfig>("Assets/Audio/AudioConfig.asset");

            if (audioConfig == null) {
                if (!AssetDatabase.IsValidFolder("Assets/Audio")) {
                    AssetDatabase.CreateFolder("Assets", "Audio");
                }

                audioConfig = ScriptableObject.CreateInstance<AudioConfig>();
                AssetDatabase.CreateAsset(audioConfig, "Assets/Audio/AudioConfig.asset");
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
            }

            Selection.activeObject = audioConfig;
        }
    }
}