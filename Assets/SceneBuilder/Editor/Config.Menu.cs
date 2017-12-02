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
        public const string MENU_TEXT = "Tools/Scene Builder";
        public const string MENU_ASSET_TEXT = "Assets/Create/Scene Builder";
        public const int MENU_PRIORITY = 100;
        public const int MENU_ASSET_PRIORITY = 10;
    }
}
