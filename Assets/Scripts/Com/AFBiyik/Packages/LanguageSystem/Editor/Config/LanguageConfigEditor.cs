using System.Collections.Generic;
using System.IO;
using Com.AFBiyik.AssetSystem.Editor;
using Com.AFBiyik.JsonHelper;
using UnityEditor;
using UnityEngine;

namespace Com.AFBiyik.LanguageSystem.Editor
{
    [CustomEditor(typeof(LanguageConfig))]
    public class LanguageConfigEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            LanguageConfig languageConfig = (LanguageConfig)target;

            EditorGUILayout.PropertyField(serializedObject.FindProperty("defaultLanguage"));

            EditorGUILayout.PropertyField(serializedObject.FindProperty("languages"));


            if (GUILayout.Button("Generate Language Files"))
            {
                CreateLanguages(languageConfig.Languages);
            }

            serializedObject.ApplyModifiedProperties();
        }

        public virtual void CreateLanguages(IReadOnlyList<SystemLanguage> languages)
        {
            if (!AssetDatabase.IsValidFolder("Assets/Language"))
            {
                AssetDatabase.CreateFolder("Assets", "Language");
            }

            for (int i = 0; i < languages.Count; i++)
            {
                SystemLanguage lng = languages[i];
                EditorUtility.DisplayProgressBar("Creating language files", "Language: " + i + "/" + languages.Count, i / (float)languages.Count);

                string name = "Language_" + lng.ToIsoString();
                string path = Path.Combine(Application.dataPath, "Language/" + name + ".json");

                if (!File.Exists(path))
                {
                    var tmp = new Dictionary<string, string>();
                    tmp["YourTag"] = "Your String";
                    JsonSaver.Save(tmp, path, true);

                    AssetDatabase.Refresh();

                    var textAsset = AssetDatabase.LoadAssetAtPath<TextAsset>("Assets/Language/" + name + ".json");

                    var addressable = textAsset.AddAssetToAddressables();
                    addressable.SetAddress(name);
                }
                else
                {
                    Debug.LogWarning("File already exist cannot overwrite: " + path);
                }
            }

            EditorUtility.ClearProgressBar();
        }
    }
}

