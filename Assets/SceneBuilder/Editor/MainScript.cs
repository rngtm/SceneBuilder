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
            DisplayBuildDialog("Assets");
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
                DisplayBuildDialog("Assets");
            }
            else
            {
                path = AssetDatabase.GetAssetPath(Selection.activeObject);
                if (AssetDatabase.IsValidFolder(path))
                {
                    DisplayBuildDialog(path);
                }
                else
                if (AssetDatabase.IsMainAsset(Selection.activeObject))
                {
                    DisplayBuildDialog(Directory.GetParent(path).ToString());
                }
            }
        }

        /// <summary>
        /// 複数シーン作成ダイアログ表示
        /// </summary>
        public static void DisplayBuildDialogMulti(string[] sceneNames)
        {
            // スクリプトの依存関係取得
            var scriptDependency = DataLoader.LoadScriptDependency();

            // ダイアログを開く
            var fullpath = EditorUtility.SaveFolderPanel("シーン作成先のフォルダ選択", "Assets", "");
            string path = "Assets" + fullpath.Substring(Application.dataPath.Length);
            if (string.IsNullOrEmpty(path)) { return; }

            var corrctedSceneNames = sceneNames.Select(name => NameCorrector.CorrectNameIfInvalid(name)).ToArray();
            if (corrctedSceneNames.Length == 0) { return; }


            // 作成
            var dataList = new List<TemporaryFileData.Data>();
            float progressDelta = 1f / sceneNames.Length;
            float progress = 0f;
            foreach (var sceneName in corrctedSceneNames)
            {
                    // プログレスバー
                progress += progressDelta;
                EditorUtility.DisplayProgressBar(string.Format("シーン\"{0}\"の作成中...", sceneName), "", progress);

                if (string.IsNullOrEmpty(sceneName)) { continue; }
                // if (AssetChecker.Exists(Path.Combine(path, sceneName))) { continue; }

                bool success;
                var data = TryBuildScene(Path.Combine(path, sceneName), out success);
                if (success)
                {
                    dataList.Add(data);

                }
            }

            // コンパイル終了時の処理 設定
            EditorUtility.DisplayProgressBar("マネージャオブジェクトの作成中...", "", 1f);
            var file = new TemporaryFileData(dataList.ToArray());
            UnityCallback.SetActionOnCompiled(file);
        }

        /// <summary>
        /// シーン作成ダイアログ表示
        /// </summary>
        public static void DisplayBuildDialog(string directory)
        {
            // ダイアログを開く
            string fullpath = EditorUtility.SaveFilePanel("シーン作成先のフォルダ選択", directory, "", "");
            if (string.IsNullOrEmpty(fullpath)) { return; }

            string path = "Assets" + fullpath.Substring(Application.dataPath.Length);
            if (string.IsNullOrEmpty(path)) { return; }
            if (AssetChecker.Exists(path)) { return; }

            // シーン作成
            bool success;
            var data = TryBuildScene(path, out success);
            if (success)
            {
                UnityCallback.SetActionOnCompiled(new TemporaryFileData(data));
            }
        }

        /// <summary>
        /// シーンの作成
        /// </summary>
        /// <param name="path">作成先のファイルパス</param>
        static TemporaryFileData.Data TryBuildScene(string path, out bool success)
        {
            if (AssetChecker.Exists(path))
            {
                Debug.LogFormat("Exist:{0}", path);
                success = false;
                return default(TemporaryFileData.Data);
            }

            // フォルダ作成
            string rootFolderPath = FolderBuilder.BuildFolderSet(path);
            if (string.IsNullOrEmpty(rootFolderPath))
            {
                success = false;
                return default(TemporaryFileData.Data);
            }

            // シーンを開く            
            var templateScene = DataLoader.LoadSceneTemplate();
            EditorSceneManager.OpenScene(AssetDatabase.GetAssetPath(templateScene), OpenSceneMode.Additive);

            #warning TODO: JSONを取得してScriptDependencyからスクリプトを作成する
            
            var rootFolderName = rootFolderPath.Split('/').Last();

            // シーンを保存
            var scenePath = string.Format("{0}/{1}.unity", rootFolderPath, rootFolderName);
            EditorSceneManager.SetActiveScene(EditorSceneManager.GetSceneByName(templateScene.name));
            EditorSceneManager.SaveScene(EditorSceneManager.GetSceneByName(templateScene.name), scenePath);

            // プログレスバー
            EditorUtility.DisplayProgressBar(string.Format("シーン\"{0}\"の作成中...", rootFolderName), "", 0f);
            
            // スクリプト作成
            var scriptDependency = DataLoader.LoadScriptDependency();
            var scripts = ScriptBuilder.BuildScripts(rootFolderPath + "/" + Config.ScriptFolder, rootFolderName, scriptDependency);

            // コンパイル終了時の処理 設定
            var data = new TemporaryFileData.Data
            {
                SceneName = rootFolderName,
                FolderPath = rootFolderPath,
                Scripts = scripts.ToArray(),
            };

            success = true;
            return data;
        }
    }
}
