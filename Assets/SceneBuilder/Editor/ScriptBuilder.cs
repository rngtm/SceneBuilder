///-------------------------------------
/// SceneBuilder
/// @ 2017 RNGTM(https://github.com/rngtm)
///-------------------------------------
namespace EditorSceneBuilder
{
    using System.Collections;
    using System.Collections.Generic;
    using System.IO;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Linq;
    using UnityEngine;
    using UnityEditor;
    using UnityEditor.Callbacks;

    /// <summary>
    /// スクリプトの作成を行う
    /// </summary>
    public static class ScriptBuilder
    {
        /// <summary>
        /// シーン内のスクリプト作成を行う
        /// </summary>
        public static IEnumerable<MonoScriptData> BuildScripts(string folderPath, string folderName, ScriptDependency dependency)
        {
            var datas = dependency.DataList
            .SelectMany(data => data.ComponentDataList);

            var classNameList = new List<string>();

            foreach (var data in datas)
            {
                NamePostprocessor.Initialize(folderName); // 設定

                var className = data.RawComponentName;
                className = NamePostprocessor.Rename(className); // リネーム
                className = NameCorrector.CorrectNameIfInvalid(className);

                if (classNameList.Contains(className)) 
                {
                    Debug.LogErrorFormat("クラス名重複 : className = {0}", className); 
                    continue; 
                }
                classNameList.Add(className);

                var templateName = data.TemplateName;
                var scriptName = string.Format("{0}.cs", className);
                var script = CreateScriptAsset(folderPath, folderName, scriptName, templateName);

                yield return new MonoScriptData
                {
                    Script = script,
                    RawComponentName = data.RawComponentName
                };
            }
        }

        /// <summary>
        /// スクリプト作成
        /// </summary>
        /// <param name="folderPath">フォルダパス</param>
        /// <param name="folderName">作成先のフォルダ名</param>
        public static MonoScript CreateScriptAsset(string folderPath, string folderName, string scriptName, string templateName)
        {
            // 補正
            folderName = NameCorrector.CorrectNameIfInvalid(folderName);
            
            var scriptPath = string.Format("{0}/{1}", folderPath, scriptName);
            var template =DataLoader.LoadScriptTemplate(templateName);

            if (template == null)
            {
                Debug.LogErrorFormat("template \"{0}\"not found", templateName);
                return null;
            }

            var templatePath = AssetDatabase.GetAssetPath(template);

            return CreateScriptAssetFromTemplate(
                @namespace: folderName,
                sceneName: folderName,
                scriptPath: scriptPath,
                templatePath: templatePath
            );
        }

        /// <summary>
        /// テンプレートファイルからスクリプトを生成
        /// </summary>
        private static MonoScript CreateScriptAssetFromTemplate(string @namespace, string sceneName, string scriptPath, string templatePath)
        {
            StreamReader streamReader = new StreamReader(templatePath);
            string text = streamReader.ReadToEnd();
            streamReader.Close();

            string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(scriptPath);
            text = Regex.Replace(text, "#SCRIPTNAME#", fileNameWithoutExtension);
            text = Regex.Replace(text, "#NAMESPACE#", @namespace);
            text = Regex.Replace(text, "#SCENENAME#", sceneName);

            bool encoderShouldEmitUTF8Identifier = true;
            bool throwOnInvalidBytes = false;
            UTF8Encoding encoding = new UTF8Encoding(encoderShouldEmitUTF8Identifier, throwOnInvalidBytes);
            bool append = false;
            StreamWriter streamWriter = new StreamWriter(Path.GetFullPath(scriptPath), append, encoding);
            streamWriter.Write(text);
            streamWriter.Close();
            AssetDatabase.ImportAsset(scriptPath);
            AssetDatabase.Refresh();
            
            return (MonoScript)AssetDatabase.LoadAssetAtPath(scriptPath, typeof(MonoScript));
        }
    }
}
