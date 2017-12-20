///-------------------------------------
/// SceneBuilder
/// @ 2017 RNGTM(https://github.com/rngtm)
///-------------------------------------
namespace EditorSceneBuilder
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Reflection;
    using UnityEngine;
    using UnityEditor;
    using UnityEditor.Callbacks;
    using UnityEditor.SceneManagement;
    using System;
    using System.IO;
    using System.Linq;
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
        static void BuildScene(TemporaryFileData.Data temporaryData)
        {
            NamePostprocessor.Initialize(temporaryData.SceneName);

            var scenePath = string.Format("{0}/{1}.unity", temporaryData.FolderPath, temporaryData.SceneName);
            var scene = EditorSceneManager.GetSceneByName(temporaryData.SceneName);
            EditorSceneManager.SetActiveScene(scene);

            // オブジェクトを作成
            var scriptDependency = DataLoader.LoadScriptDependency();
            var gameObjectDict = GameObjectBuilder.BuildGameObjects(scriptDependency)
            .ToDictionary(gameObject => gameObject.name, g => g);

            var scriptDict = temporaryData.Scripts.ToDictionary(script => script.RawComponentName, script => script);
            var componentDict = new Dictionary<string, Component>();

            // オブジェクトへスクリプトをアタッチ
            foreach (var dependencyData in scriptDependency.DataList)
            {
                var gameObject = gameObjectDict[dependencyData.RawGameObjectName];

                // dependencyに定義されているコンポーネントをアタッチ
                foreach (var componentData in dependencyData.ComponentDataList)
                {
                    var rawComponentName = componentData.RawComponentName;
                    var component = scriptDict[rawComponentName].Script.GetClass();
                    var addComponent = gameObject.AddComponent(component);
                    componentDict.Add(rawComponentName, addComponent); // 追加したコンポーネントを登録
                }
            }

            // コンポーネントの参照関係の設定
            foreach (var dependencyData in scriptDependency.DataList)
            {
                foreach (var componentData in dependencyData.ComponentDataList)
                {
                    foreach (var refData in componentData.ReferenceList)
                    {
                        // 参照先GameObject
                        var refGameObject = gameObjectDict[refData.RawGameObjectName];

                        // 参照先コンポーネント名
                        var refComponentName = NamePostprocessor.Rename(refData.RawComponentName);

                        // リフレクション経由で設定
                        var refComponent = refGameObject.GetComponent(refComponentName);
                        var component = componentDict[componentData.RawComponentName];

                        var flags = BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public;
                        var fields = component.GetType().GetFields(flags).Where(f => f != null);

                        var field = fields.FirstOrDefault(f => f.FieldType.ToString().Contains(refComponentName));
                        if (field != null)
                        {
                            // 値の設定
                            field.SetValue(component, refComponent);
                        }
                    }
                }
            }

            // GameObjectの名前修正
            foreach (var item in gameObjectDict)
            {
                item.Value.name = NamePostprocessor.Rename(item.Value.name);
            }

            // シーンアセット 保存
            EditorSceneManager.SaveScene(scene, scenePath);
            EditorSceneManager.CloseScene(scene, true);
            EditorUtility.ClearProgressBar();

            // 作成したシーンをハイライトする
            EditorApplication.delayCall += () =>
            EditorApplication.delayCall += () =>
            {
                Debug.LogFormat("Create: {0}", temporaryData.FolderPath);
                EditorGUIUtility.PingObject(AssetDatabase.LoadAssetAtPath(scenePath, typeof(UnityEngine.Object)));
            };
        }

    }
}