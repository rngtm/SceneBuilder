///-------------------------------------
/// SceneBuilder
/// @ 2017 RNGTM(https://github.com/rngtm)
///-------------------------------------
namespace EditorSceneBuilder
{
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
        /// スクリプト作成
        /// </summary>
        public static MonoScript CreateScriptAsset(string folderPath, string folderName)
        {
            // 補正
            folderName = NameCorrector.CorrectNameIfInvalid(folderName);
            
            var scriptName = string.Format("{0}Manager.cs", folderName);
            var scriptPath = string.Format("{0}/{1}", folderPath, scriptName);
            var templatePath = AssetDatabase.GetAssetPath(DataLoader.LoadScriptTemplate());

            return CreateScriptAssetFromTemplate(
                @namespace: folderName,
                scriptPath: scriptPath,
                templatePath: templatePath
            );
        }

        /// <summary>
        /// テンプレートファイルからスクリプトを生成
        /// </summary>
        private static MonoScript CreateScriptAssetFromTemplate(string @namespace, string scriptPath, string templatePath)
        {
            StreamReader streamReader = new StreamReader(templatePath);
            string text = streamReader.ReadToEnd();
            streamReader.Close();

            string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(scriptPath);

            text = Regex.Replace(text, "#SCRIPTNAME#", fileNameWithoutExtension);
            text = Regex.Replace(text, "#NAMESPACE#", @namespace);

            bool encoderShouldEmitUTF8Identifier = true;
            bool throwOnInvalidBytes = false;
            UTF8Encoding encoding = new UTF8Encoding(encoderShouldEmitUTF8Identifier, throwOnInvalidBytes);
            bool append = false;
            StreamWriter streamWriter = new StreamWriter(Path.GetFullPath(scriptPath), append, encoding);
            streamWriter.Write(text);
            streamWriter.Close();
            AssetDatabase.ImportAsset(scriptPath);
            AssetDatabase.Refresh();
            
            // Debug.LogFormat("scriptPath: {0}", scriptPath);
            return (MonoScript)AssetDatabase.LoadAssetAtPath(scriptPath, typeof(MonoScript));
        }
    }
}
