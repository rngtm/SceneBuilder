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
            // public MonoScript MonoScript;

            // #warning TODO: 作成したScriptの代入処理
            public MonoScriptData[] Scripts;
            // public MonoScript MonoScript { get { return this.Scripts[0]; } }
        }
    }
}