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

    /// <summary>
    /// テンプレートの設定
    /// </summary>
    public class TemplateConfig : ScriptableObject
    {
        [SerializeField, Header("テンプレートファイル")] private TextAsset templateFile;
        [SerializeField, Header("SceneAsset")] private SceneAsset sceneAsset;

        public TextAsset TemplateFile { get { return this.templateFile; } }
        public SceneAsset SceneAsset { get { return this.sceneAsset; } }
    }
}
