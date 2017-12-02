///-------------------------------------
/// SceneBuilder
/// @ 2017 RNGTM(https://github.com/rngtm)
///-------------------------------------
namespace EditorSceneBuilder
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEditor;
    using System.Linq;

    /// <summary>
    /// データのロードを行うクラス
    /// </summary>
    public class DataLoader
    {
        /// <summary>
        /// スクリプトテンプレートのロード
        /// </summary>
        public static TextAsset LoadScriptTemplate()
        {
            return LoadConfig().TemplateFile;
        }

        public static SceneAsset LoadSceneTemplate()
        {
            return LoadConfig().SceneAsset;
        }

        public static TemplateConfig LoadConfig()
        {
            return AssetDatabase.FindAssets("t:ScriptableObject")
           .Select(guid => AssetDatabase.GUIDToAssetPath(guid))
           .Select(path => AssetDatabase.LoadAssetAtPath(path, typeof(TemplateConfig)))
           .Where(obj => obj != null)
           .Select(obj => (TemplateConfig)obj)
           .FirstOrDefault();
        }

    }
}
