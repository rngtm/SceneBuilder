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
        public static TextAsset LoadScriptTemplate(string templateName)
        {
            return LoadConfig().TemplateScripts
            .FirstOrDefault(template => template.name == templateName);
        }

        /// <summary>
        /// スクリプト依存関係JSONの取得
        /// </summary>
        public static ScriptDependency LoadScriptDependency()
        {
            return LoadConfig().ScriptDependency;
        }

        public static SceneAsset LoadSceneTemplate()
        {
            return LoadConfig().TemplateScene;
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
