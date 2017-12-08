///-------------------------------------
/// SceneBuilder
/// @ 2017 RNGTM(https://github.com/rngtm)
///-------------------------------------
namespace EditorSceneBuilder
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

	[System.Serializable]
    public class SceneNameData
    {
		[SerializeField] private string sceneName = "";
		public string SceneName { get { return this.sceneName; } set { this.sceneName = value; } }
    }
}
