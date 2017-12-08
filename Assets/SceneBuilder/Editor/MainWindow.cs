///-------------------------------------
/// SceneBuilder
/// @ 2017 RNGTM(https://github.com/rngtm)
///-------------------------------------
namespace EditorSceneBuilder
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using UnityEngine;
    using UnityEditor;
    using UnityEditorInternal;

    public class MainWindow : EditorWindow
    {
        private List<SceneNameData> sceneNameList;
        private ReorderableList reorderableList;

        [MenuItem(Config.MENU_TEXT, false, Config.MENU_PRIORITY)]
        static void Open()
        {
            GetWindow<MainWindow>("SceneBuilder");
        }

        void OnGUI()
        {
            if (this.sceneNameList == null)
            {
                this.sceneNameList = new List<SceneNameData>();
            }

            if (this.reorderableList == null)
            {
                this.reorderableList = this.CreateReorderableList();
            }

            EditorGUILayout.LabelField("シーンを一括で生成します");
            EditorGUI.BeginDisabledGroup(this.sceneNameList.Count == 0);
            if (GUILayout.Button("シーン作成", GUILayout.Height(32f)))
            {
                MainScript.BuildSceneSets(this.sceneNameList.Select(d => d.SceneName).ToArray());
            }
            EditorGUI.EndDisabledGroup();

            this.reorderableList.DoLayoutList();
        }

        /// <summary>
        /// ReorderableList作成
        /// </summary>
        private ReorderableList CreateReorderableList()
        {
            var reorderableList = new ReorderableList(this.sceneNameList, typeof(SceneNameData));

            // ヘッダー描画
            var headerRect = default(Rect);
            reorderableList.drawHeaderCallback = (rect) =>
            {
                headerRect = rect;
                EditorGUI.LabelField(rect, "シーン名");
            };

            // フッター描画
            reorderableList.drawFooterCallback = (rect) =>
            {
                rect.y = headerRect.y + 3;
                ReorderableList.defaultBehaviours.DrawFooter(rect, reorderableList);
            };

            // 要素の描画
            reorderableList.drawElementCallback = (rect, index, isActive, isFocused) =>
            {
                rect.height -= 6f;
                rect.y += 3f;

                var labelRect = new Rect(rect);
                labelRect.width = 65f;
                EditorGUI.LabelField(labelRect, string.Format("Scene {0}", index));

                var textRect = new Rect(rect);
                textRect.width -= labelRect.width;
                textRect.x += labelRect.width;
                this.sceneNameList[index].SceneName = EditorGUI.TextField(textRect, this.sceneNameList[index].SceneName);
            };

            return reorderableList;
        }
    }
}
