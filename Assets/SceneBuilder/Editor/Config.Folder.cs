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
    /// 設定の定義
    /// </summary>
    public static partial class Config
    {
        /// <summary>
        /// スクリプト保存先のフォルダ
        /// </summary>
        public const string ScriptFolder = "Scripts";

        /// <summary>
        /// 作成するサブフォルダ名
        /// </summary> 
        public static readonly string[] SubFolderNameArray = new string[]
        {
            "Animations",
            "Scripts",
            "Shaders",
            "Prefabs",
            "Materials",
            "Textures",
        };

    }
}
