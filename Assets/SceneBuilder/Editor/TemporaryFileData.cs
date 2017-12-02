///-------------------------------------
/// SceneBuilder
/// @ 2017 RNGTM(https://github.com/rngtm)
///-------------------------------------
namespace EditorSceneBuilder
{
    using UnityEditor;

    /// <summary>
    /// 一時ファイルのデータ
    /// </summary>
    [System.Serializable]
    public struct TemporaryFileData
    {
        public string SceneName;
        public string FolderPath;
        public MonoScript MonoScript;
    }
}