///-------------------------------------
/// SceneBuilder
/// @ 2017 RNGTM(https://github.com/rngtm)
///-------------------------------------
namespace EditorSceneBuilder
{
    using UnityEditor;
    using System.Collections.Generic;

    /// <summary>
    /// 一時ファイルのデータ
    /// </summary>
    [System.Serializable]
    public struct TemporaryFileData
    {
        public Data[] DataArray;

        public TemporaryFileData(Data data)
        {
            this.DataArray = new Data[] { data };
        }

        public TemporaryFileData(Data[] data)
        {
            this.DataArray = data;
        }

        [System.Serializable]
        public struct Data
        {
            public string SceneName;
            public string FolderPath;
            public MonoScriptData[] Scripts;
        }
    }
}