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
	/// 名前の後処理を行う
	/// </summary>
    public class NamePostprocessor
    {
		private static string m_SceneName = "";

		/// <summary>
		/// シーン名設定
		/// </summary>
		public static void Initialize(string sceneName)
		{
			m_SceneName = sceneName;
		}

		/// <summary>
		/// 名前の置き換えを行う
		/// </summary>
		public static string Rename(string name)
		{
			return name.Replace("#SCENENAME#", m_SceneName);
		}
    }
}