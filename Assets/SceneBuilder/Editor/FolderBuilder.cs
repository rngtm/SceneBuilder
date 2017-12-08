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
    /// フォルダの作成を行う
    /// </summary>
    public static class FolderBuilder
    {
        /// <summary>
        /// シーン格納用のフォルダ一式作成
        /// </summary>
        public static string BuildFolderSet(string folderPath)
        {
            var folderName = Path.GetFileName(folderPath); // フォルダ名
            var folderParent = Path.GetDirectoryName(folderPath); // 親ディレクトリ

            return BuildFolderSet(folderParent, folderName);
        }

        /// <summary>
        /// シーン格納用のフォルダ一式作成
        /// </summary>
		public static string BuildFolderSet(string folderParent, string folderName)
        {
            var newFolderGUID = AssetDatabase.CreateFolder(folderParent, folderName);
            var newFolderPath = AssetDatabase.GUIDToAssetPath(newFolderGUID);

            // サブフォルダ作成
            foreach (var subFolderName in Config.SubFolderNameArray)
            {
                AssetDatabase.CreateFolder(newFolderPath, subFolderName);
            }

            return newFolderPath;
        }

    }
}
