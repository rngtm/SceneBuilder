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
        [SerializeField, Header("テンプレートスクリプト")] private TextAsset[] templateScripts;
        [SerializeField, Header("スクリプトの依存関係JSONファイル")] private ScriptDependency scriptDependency;
        [SerializeField, Header("テンプレートシーン")] private SceneAsset templateScene;

        public TextAsset TemplateFile { get { return this.templateFile; } }
        public TextAsset[] TemplateScripts { get { return this.templateScripts; } }
        public SceneAsset TemplateScene { get { return this.templateScene; } }
        public ScriptDependency ScriptDependency { get { return this.scriptDependency; } }
    }
}
