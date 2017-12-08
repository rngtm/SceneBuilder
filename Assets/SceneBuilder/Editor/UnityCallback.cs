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
    using UnityEditor.Callbacks;
    using UnityEditor.SceneManagement;
    using System;
    using System.IO;
    using System.Text;

    public static class UnityCallback
    {
        /// <summary>
        /// 一時ファイルのパス
        /// </summary>
        const string tempFilePath = "Temp/SceneBuilderTemplate";

        /// <summary>
        /// コンパイル完了時処理の設定
        /// </summary>
        public static void SetActionOnCompiled(TemporaryFileData file)
        {
            var json = JsonUtility.ToJson(file);

            // 一時ファイルを生成して情報を書き込んで保存
            File.WriteAllText(tempFilePath, json, Encoding.UTF8);
        }
    
        /// <summary>
        /// 一時ファイル 取得
        /// </summary>
        public static TemporaryFileData LoadTempFile()
        {
            StreamReader reader = new StreamReader(tempFilePath, Encoding.GetEncoding("Shift_JIS"));

            // ファイル名を読み込み
            var json = reader.ReadLine();

            reader.Close();

            return JsonUtility.FromJson<TemporaryFileData>(json);
        }

        [DidReloadScripts]
        static void OnCompiled()
        {
            // 一時ファイルがあれば処理
            if (File.Exists(tempFilePath))
            {
                var file = LoadTempFile();

                BuildScene(file);

                // 一時ファイルの削除
                File.Delete(tempFilePath);
            }
        }

        static void BuildScene(TemporaryFileData file)
        {
            foreach (var data in file.DataArray)
            {
                BuildScene(data);
            }
        }

        /// <summary>
        /// シーンの構築
        /// </summary>
        static void BuildScene(TemporaryFileData.Data data)
        {
            var scenePath = string.Format("{0}/{1}.unity", data.FolderPath, data.SceneName);
            var scene = EditorSceneManager.GetSceneByName(data.SceneName);
            EditorSceneManager.SetActiveScene(scene);

            // オブジェクトを作成してスクリプトをアタッチ
            var newGameObject = new GameObject(string.Format("{0}Manager", data.SceneName));
            newGameObject.AddComponent(data.MonoScript.GetClass());
            newGameObject.transform.SetSiblingIndex(0);

            // シーンアセット 保存
            EditorSceneManager.SaveScene(scene, scenePath);
            EditorSceneManager.CloseScene(scene, true);
            EditorUtility.ClearProgressBar();

            // 作成したシーンをハイライトする
            EditorApplication.delayCall += () =>
            EditorApplication.delayCall += () =>
            {
                Debug.LogFormat("Create: {0}", data.FolderPath);
                EditorGUIUtility.PingObject(AssetDatabase.LoadAssetAtPath(scenePath, typeof(UnityEngine.Object)));
            };
        }

    }
}