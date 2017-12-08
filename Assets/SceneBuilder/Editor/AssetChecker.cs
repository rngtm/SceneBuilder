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

    public static class AssetChecker
    {
		/// <summary>
		/// アセットが存在するか判定
		/// </summary>
        public static bool Exists(string path)
        {
            return AssetDatabase.LoadAssetAtPath(path, typeof(UnityEngine.Object)) != null;
        }
    }
}
