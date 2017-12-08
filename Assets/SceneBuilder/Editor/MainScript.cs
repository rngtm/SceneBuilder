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
        /// 複数のシーン一式の作成
        /// </summary>
        public static void BuildSceneSets(string[] sceneNames)
        {
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
                if (AssetChecker.Exists(Path.Combine(path, sceneName))) { continue; }

                // フォルダ作成
                string rootFolderPath = FolderBuilder.BuildFolderSet(path, sceneName);
                if (string.IsNullOrEmpty(rootFolderPath)) { continue; }

                // シーンを開く            
                var templateScene = DataLoader.LoadSceneTemplate();
                EditorSceneManager.OpenScene(AssetDatabase.GetAssetPath(templateScene), OpenSceneMode.Additive);

                // スクリプト作成
                var rootFolderName = rootFolderPath.Split('/').Last();
                var newScript = ScriptBuilder.CreateScriptAsset(rootFolderPath + "/" + Config.ScriptFolder, rootFolderName);

                // シーンを保存
                var scenePath = string.Format("{0}/{1}.unity", rootFolderPath, rootFolderName);
                EditorSceneManager.SetActiveScene(EditorSceneManager.GetSceneByName(templateScene.name));
                EditorSceneManager.SaveScene(EditorSceneManager.GetSceneByName(templateScene.name), scenePath);

                dataList.Add(new TemporaryFileData.Data
                {
                    SceneName = rootFolderName,
                    FolderPath = rootFolderPath,
                    MonoScript = newScript,
                }
                );
            }

            // コンパイル終了時の処理 設定
            EditorUtility.DisplayProgressBar("マネージャオブジェクトの作成中...", "", 1f);
            var file = new TemporaryFileData(dataList.ToArray());
            UnityCallback.SetActionOnCompiled(file);
        }

        /// <summary>
        /// シーン一式の作成
        /// </summary>
        public static void BuildSceneSet(string directory)
        {
            // ダイアログを開く
            string fullpath = EditorUtility.SaveFilePanel("シーン作成先のフォルダ選択", directory, "", "");
            if (string.IsNullOrEmpty(fullpath)) { return; }

            string path = "Assets" + fullpath.Substring(Application.dataPath.Length);
            if (string.IsNullOrEmpty(path)) { return; }
            if (AssetChecker.Exists(path)) { return; }

            // フォルダ作成
            string rootFolderPath = FolderBuilder.BuildFolderSet(path);
            if (string.IsNullOrEmpty(rootFolderPath)) { return; }

            // シーンを開く            
            var templateScene = DataLoader.LoadSceneTemplate();
            EditorSceneManager.OpenScene(AssetDatabase.GetAssetPath(templateScene), OpenSceneMode.Additive);

            // スクリプト作成
            var rootFolderName = rootFolderPath.Split('/').Last();
            var newScript = ScriptBuilder.CreateScriptAsset(rootFolderPath + "/" + Config.ScriptFolder, rootFolderName);

            // コンパイル終了時の処理 設定
            var file = new TemporaryFileData(
                new TemporaryFileData.Data
                {
                    SceneName = rootFolderName,
                    FolderPath = rootFolderPath,
                    MonoScript = newScript,
                }
            );
            UnityCallback.SetActionOnCompiled(file);

            // シーンを保存
            var scenePath = string.Format("{0}/{1}.unity", rootFolderPath, rootFolderName);
            EditorSceneManager.SetActiveScene(EditorSceneManager.GetSceneByName(templateScene.name));
            EditorSceneManager.SaveScene(EditorSceneManager.GetSceneByName(templateScene.name), scenePath);

            // プログレスバー
            EditorUtility.DisplayProgressBar(string.Format("シーン\"{0}\"の作成中...", rootFolderName), "", 0f);
        }
    }
}
