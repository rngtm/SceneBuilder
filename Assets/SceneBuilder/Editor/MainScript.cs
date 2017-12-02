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

    public class MainScript
    {
        /// <summary>
        /// メニューから実行
        /// </summary>
        [MenuItem(Config.MENU_TEXT, false, Config.MENU_PRIORITY)]
        private static void BuildSceneFromMenu()
        {
            BuildSceneSet("Assets");
        }

        /// <summary>
        /// Createメニューから実行
        /// </summary>
        [MenuItem(Config.MENU_ASSET_TEXT, false, Config.MENU_ASSET_PRIORITY)]
        private static void BuildSceneFromAssetMenu()
        {
            string path = "";
            if (Selection.activeObject == null)
            {
                BuildSceneSet("Assets");
            }
            else
            {
                path = AssetDatabase.GetAssetPath(Selection.activeObject);
                if (AssetDatabase.IsValidFolder(path))
                {
                    BuildSceneSet(path);
                }
                else
                if (AssetDatabase.IsMainAsset(Selection.activeObject))
                {
                    BuildSceneSet(Directory.GetParent(path).ToString());
                }
            }
        }

        /// <summary>
        /// シーンの作成
        /// </summary>
        private static void BuildSceneSet(string directory)
        {
            // ダイアログを開く
            var rootFolderPath = DisplayCreateFolderDialog(directory);
            if (string.IsNullOrEmpty(rootFolderPath)) { return; }

            // シーンを開く            
            var templateScene = DataLoader.LoadSceneTemplate();
            EditorSceneManager.OpenScene(AssetDatabase.GetAssetPath(templateScene), OpenSceneMode.Additive);

            // スクリプト作成
            var rootFolderName = rootFolderPath.Split('/').Last();
            var newScript = ScriptBuilder.CreateScriptAsset(rootFolderPath + "/" + Config.ScriptFolder, rootFolderName);

            // コンパイル終了時の処理 設定
            var file = new TemporaryFileData
            {
                SceneName = rootFolderName,
                FolderPath = rootFolderPath,
                MonoScript = newScript,

            };
            UnityCallback.SetActionOnCompiled(file);

            // シーンを保存
            var scenePath = string.Format("{0}/{1}.unity", rootFolderPath, rootFolderName);
            EditorSceneManager.SetActiveScene(EditorSceneManager.GetSceneByName(templateScene.name));
            EditorSceneManager.SaveScene(EditorSceneManager.GetSceneByName(templateScene.name), scenePath);

            // プログレスバー
            EditorUtility.DisplayProgressBar(string.Format("シーン\"{0}\"の作成中...", rootFolderName), "", 0f);
        }

        /// <summary>
        /// フォルダ作成ダイアログ表示
        /// </summary>
        static string DisplayCreateFolderDialog(string dialogDirectory)
        {
            var fullpath = EditorUtility.SaveFilePanel("作成先", dialogDirectory, "", "");
            if (string.IsNullOrEmpty(fullpath)) { return ""; }

            string path = "Assets" + fullpath.Substring(Application.dataPath.Length);
            var fileName = Path.GetFileName(path);
            var directory = Path.GetDirectoryName(path);
            var folderGUID = AssetDatabase.CreateFolder(directory, fileName); // ルートフォルダ作成
            var folderPath = AssetDatabase.GUIDToAssetPath(folderGUID);

            // サブフォルダ作成
            foreach (var subFolderName in Config.SubFolderNameArray)
            {
                AssetDatabase.CreateFolder(folderPath, subFolderName);
            }

            return folderPath;
        }
    }
}
