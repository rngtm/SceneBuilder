///-------------------------------------
/// SceneBuilder
/// @ 2017 RNGTM(https://github.com/rngtm)
///-------------------------------------
namespace EditorSceneBuilder
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    /// <summary>
    /// 生成オブジェクトや生成コンポーネントの定義
    /// </summary>
    public class ScriptDependency : ScriptableObject
    {
        public List<ObjectData> DataList;

        /// <summary>
        /// オブジェクトデータ
        /// </summary>
        [System.Serializable]
        public struct ObjectData
        {
            [Header("ゲームオブジェクト名")]
            public string RawGameObjectName;


            [Header("コンポーネント")]
            public List<ComponentData> ComponentDataList;

            /// <summary>
            /// コンポーネント
            /// </summary>
            [System.Serializable]
            public struct ComponentData
            {
                [Header("テンプレートファイル")]
                public string TemplateName; // テンプレートファイル名

                [Header("コンポーネント名")]
                public string RawComponentName; // コンポーネント名
                
                [Header("参照データ")]                
                public List<ReferenceData> ReferenceList;

                /// <summary>
                /// どのコンポーネントを参照するか
                /// </summary>
                [System.Serializable]
                public struct ReferenceData
                {
                    [Header("参照オブジェクト名")]
                    public string RawGameObjectName;

                    [Header("参照コンポーネント名")]
                    public string RawComponentName;
                }

            }
        }
    }


}