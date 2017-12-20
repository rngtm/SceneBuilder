///-------------------------------------
/// SceneBuilder
/// @ 2017 RNGTM(https://github.com/rngtm)
///-------------------------------------
namespace EditorSceneBuilder
{
    using System;
    using System.IO;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using UnityEngine;
    using UnityEditor;
    using UnityEditor.Callbacks;
    using UnityEditor.SceneManagement;
    using UnityEditor.ProjectWindowCallback;
    using UnityEditorInternal;

    /// <summary>
    /// GameObjectの作成を行う
    /// </summary>
    public static class GameObjectBuilder
    {
        /// <summary>
        /// GameObjectの作成を行う(既に存在する場合は取得)
        /// </summary>
        public static IEnumerable<GameObject> BuildGameObjects(ScriptDependency dependency)
        {
            foreach (var data in dependency.DataList)
            {
                // Debug.LogFormat("GameObjectNameSuffix = {0}", data.GameObjectNameSuffix);
                var gameObject = GameObject.Find(data.RawGameObjectName);
                if (gameObject == null) { gameObject = new GameObject(data.RawGameObjectName); }
                gameObject.transform.SetSiblingIndex(0);

                yield return gameObject;
            }
        }
    }
}
